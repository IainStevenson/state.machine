using System;
using System.Collections.Specialized;
using System.Reflection;
using NLog.Interface;
using State.Machine.Messages;
using State.Machine.Messages.Events;
using State.Machine.Messaging;
using Stateless;

namespace State.Machine.Machines
{
    public class Contract
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;
        private readonly StateMachine<ContractStates, ContractTriggers> _machine;
        private ICommand _command;

        /// <summary>
        ///     Constructor, initialises and Configures the State machine for Contract's
        /// </summary>
        /// <param name="machine">The state machine</param>
        /// <param name="eventBus">The message bus for publishing events from successful transitions</param>
        /// <param name="logger">The logger to use when unsupported transitions are requested, or failures occur</param>
        public Contract(StateMachine<ContractStates, ContractTriggers> machine, IEventBus eventBus, ILogger logger)
        {
            _machine = machine;
            
            _machine.Configure(ContractStates.Start)
                .Permit(ContractTriggers.Create, ContractStates.Created);

            _machine.Configure(ContractStates.Created)
                .OnEntry(s =>
                {
                    var @event = new CreatedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Delete, ContractStates.Deleted)
                .Permit(ContractTriggers.Submit, ContractStates.Submitted)
                ;

            _machine.Configure(ContractStates.Submitted)
                .OnEntryFrom(ContractTriggers.Submit, () =>
                {
                    var @event = new SubmittedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .OnEntryFrom(ContractTriggers.Resubmit, () =>
                {
                    var @event = new ResubmittedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Delete, ContractStates.Deleted)
                .Permit(ContractTriggers.Decline, ContractStates.Declined)
                .Permit(ContractTriggers.Accept, ContractStates.Accepted)
                .Permit(ContractTriggers.Expire, ContractStates.Expired)
                ;


            _machine.Configure(ContractStates.Accepted)
                .OnEntry(() =>
                {
                    var @event = new AcceptedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Fulfill, ContractStates.Fulfilled)
                .Permit(ContractTriggers.Cancel, ContractStates.Cancelled)
                ;


            _machine.Configure(ContractStates.Fulfilled)
                .OnEntryFrom(ContractTriggers.Fulfill, () =>
                {
                    var @event = new FulfilledContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .OnEntryFrom(ContractTriggers.Refulfill, () =>
                {
                    var @event = new RefulfilledContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Approve, ContractStates.Approved)
                .Permit(ContractTriggers.Reject, ContractStates.Rejected)
                ;

            _machine.Configure(ContractStates.Approved).OnEntry(() =>
            {
                var @event = new ApprovedContractEvent(_command.Properties);
                _eventBus.Publish(@event);
            })
                .Permit(ContractTriggers.Pay, ContractStates.Payed)
                ;


            _machine.Configure(ContractStates.Rejected).OnEntry(() =>
            {
                var @event = new RejectedContractEvent(_command.Properties);
                _eventBus.Publish(@event);
            })
                .Permit(ContractTriggers.Refulfill, ContractStates.Fulfilled)
                ;


            _machine.Configure(ContractStates.Deleted)
                .OnEntryFrom(ContractTriggers.Delete, () =>
                {
                    var @event = new DeletedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                });
            
            _machine.Configure(ContractStates.Declined)
                .OnEntry(() =>
                {
                    var @event = new DeclinedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Resubmit, ContractStates.Submitted)
                ;
            
            _machine.Configure(ContractStates.Expired)
                .OnEntry(() =>
                {
                    var @event = new ExpiredContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Resubmit, ContractStates.Submitted)
                ;

            _machine.Configure(ContractStates.Cancelled)
                .OnEntry(() =>
                {
                    var @event = new CancelledContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                })
                .Permit(ContractTriggers.Resubmit, ContractStates.Submitted);

            _machine.Configure(ContractStates.Payed)
                .OnEntry(() =>
                {
                    var @event = new PayedContractEvent(_command.Properties);
                    _eventBus.Publish(@event);
                });

            _machine.OnUnhandledTrigger((state, trigger) =>
            {
                string message = String.Format(
                    "Trace: Contract State Machine, Unhandled Trigger {0} on state {1}.",
                    trigger.ToString(), state.ToString());
                _logger.Warn(message);
            });

            _machine.OnTransitioned(t => { });


            _eventBus = eventBus;
            _logger = logger;
        }


        /// <summary>
        /// This handles the state change requested by the command.
        /// </summary>
        /// <param name="command"></param>
        /// <remarks>Commands arriving without a ContractTriggerAttribute are logged and otherwise ignored.</remarks>
        public void Handle(ICommand command)
        {
            // restore the macines state
            if (command == null) return;
            _command =command;
            // extract the trigger from the command and fire it
            var triggerAttribute = command.GetType().GetCustomAttribute<ContractTriggerAttribute>();
            if (triggerAttribute != null)
            {
                ContractTriggers trigger = triggerAttribute.Trigger;
                _machine.Fire(trigger);
            }
            else
            {
                _logger.Warn(String.Format("Command [{0}] was not processed due to missing Trigger attribute", command.GetType().Name));
            }
            // Store the machines state
        }
    }
}