using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NLog.Interface;
using NSubstitute;
using NUnit.Framework;
using State.Machine.Machines;
using State.Machine.Messages;
using State.Machine.Messages.Commands;
using State.Machine.Messaging;
using Stateless;

namespace State.Machine.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TestContractStateMachine
    {
        /// <summary>
        ///     The state machine under test
        /// </summary>
        private Contract _unitUnderTest;

        private ILogger _logger;
        private StateMachine<ContractStates, ContractTriggers> _stateMachine;
        private IEventBus _bus;
        private Type _eventType;

        private void Arrange(ContractStates start)
        {
            _logger = Substitute.For<ILogger>();
            _stateMachine = new StateMachine<ContractStates, ContractTriggers>(start);
            _bus = Substitute.For<IEventBus>();
            _unitUnderTest = new Contract(_stateMachine, _bus, _logger);
        }


        private void Act(ContractTriggers trigger)
        {
            // find the command for the trigger and issue it
            ICommand command = FindCommandForTrigger(trigger);
            _eventType = FindEventFromTrigger(trigger);
            _unitUnderTest.Handle(command);
        }

        private Type FindEventFromTrigger(ContractTriggers trigger)
        {
            List<Type> commandTypes =
                TestExtensions.GetEventsForTrigger(Assembly.GetAssembly(typeof (CreateContractCommand)), trigger)
                    .ToList();
            if (commandTypes.Any())
            {
                return commandTypes.First();
            }
            return null;
        }

        /// <summary>
        ///     Finds the command that fires the specified trigger.
        /// </summary>
        /// <param name="trigger">The trigger to fire for the test</param>
        /// <returns></returns>
        private ICommand FindCommandForTrigger(ContractTriggers trigger)
        {
            List<Type> commandTypes =
                TestExtensions.GetCommandsForTrigger(Assembly.GetAssembly(typeof (CreateContractCommand)), trigger)
                    .ToList();
            if (commandTypes.Any())
            {
                return (ICommand) Activator.CreateInstance(commandTypes.First(), new ListDictionary());
            }
            return null;
        }

        [Test]
        [TestCase(1, ContractStates.Start, ContractTriggers.Create, ContractStates.Created)]
        [TestCase(2, ContractStates.Start, ContractTriggers.Delete, ContractStates.Start)]
        [TestCase(3, ContractStates.Start, ContractTriggers.Submit, ContractStates.Start)]
        [TestCase(4, ContractStates.Start, ContractTriggers.Decline, ContractStates.Start)]
        [TestCase(5, ContractStates.Start, ContractTriggers.Accept, ContractStates.Start)]
        [TestCase(6, ContractStates.Start, ContractTriggers.Expire, ContractStates.Start)]
        [TestCase(7, ContractStates.Start, ContractTriggers.Cancel, ContractStates.Start)]
        [TestCase(8, ContractStates.Start, ContractTriggers.Fulfill, ContractStates.Start)]
        [TestCase(9, ContractStates.Start, ContractTriggers.Reject, ContractStates.Start)]
        [TestCase(10, ContractStates.Start, ContractTriggers.Refulfill, ContractStates.Start)]
        [TestCase(11, ContractStates.Start, ContractTriggers.Approve, ContractStates.Start)]
        [TestCase(12, ContractStates.Start, ContractTriggers.Pay, ContractStates.Start)]
        [TestCase(13, ContractStates.Created, ContractTriggers.Create, ContractStates.Created)]
        [TestCase(14, ContractStates.Created, ContractTriggers.Delete, ContractStates.Deleted)]
        [TestCase(15, ContractStates.Created, ContractTriggers.Submit, ContractStates.Submitted)]
        [TestCase(16, ContractStates.Created, ContractTriggers.Decline, ContractStates.Created)]
        [TestCase(17, ContractStates.Created, ContractTriggers.Accept, ContractStates.Created)]
        [TestCase(18, ContractStates.Created, ContractTriggers.Expire, ContractStates.Created)]
        [TestCase(19, ContractStates.Created, ContractTriggers.Cancel, ContractStates.Created)]
        [TestCase(20, ContractStates.Created, ContractTriggers.Fulfill, ContractStates.Created)]
        [TestCase(21, ContractStates.Created, ContractTriggers.Reject, ContractStates.Created)]
        [TestCase(22, ContractStates.Created, ContractTriggers.Refulfill, ContractStates.Created)]
        [TestCase(23, ContractStates.Created, ContractTriggers.Approve, ContractStates.Created)]
        [TestCase(24, ContractStates.Created, ContractTriggers.Pay, ContractStates.Created)]
        [TestCase(25, ContractStates.Submitted, ContractTriggers.Create, ContractStates.Submitted)]
        [TestCase(26, ContractStates.Submitted, ContractTriggers.Delete, ContractStates.Deleted)]
        [TestCase(27, ContractStates.Submitted, ContractTriggers.Submit, ContractStates.Submitted)]
        [TestCase(28, ContractStates.Submitted, ContractTriggers.Decline, ContractStates.Declined)]
        [TestCase(29, ContractStates.Submitted, ContractTriggers.Accept, ContractStates.Accepted)]
        [TestCase(30, ContractStates.Submitted, ContractTriggers.Expire, ContractStates.Expired)]
        [TestCase(31, ContractStates.Submitted, ContractTriggers.Cancel, ContractStates.Submitted)]
        [TestCase(32, ContractStates.Submitted, ContractTriggers.Fulfill, ContractStates.Submitted)]
        [TestCase(33, ContractStates.Submitted, ContractTriggers.Reject, ContractStates.Submitted)]
        [TestCase(34, ContractStates.Submitted, ContractTriggers.Refulfill, ContractStates.Submitted)]
        [TestCase(35, ContractStates.Submitted, ContractTriggers.Approve, ContractStates.Submitted)]
        [TestCase(36, ContractStates.Submitted, ContractTriggers.Pay, ContractStates.Submitted)]
        [TestCase(37, ContractStates.Accepted, ContractTriggers.Create, ContractStates.Accepted)]
        [TestCase(38, ContractStates.Accepted, ContractTriggers.Delete, ContractStates.Accepted)]
        [TestCase(39, ContractStates.Accepted, ContractTriggers.Submit, ContractStates.Accepted)]
        [TestCase(40, ContractStates.Accepted, ContractTriggers.Decline, ContractStates.Accepted)]
        [TestCase(41, ContractStates.Accepted, ContractTriggers.Accept, ContractStates.Accepted)]
        [TestCase(42, ContractStates.Accepted, ContractTriggers.Expire, ContractStates.Accepted)]
        [TestCase(43, ContractStates.Accepted, ContractTriggers.Cancel, ContractStates.Cancelled)]
        [TestCase(44, ContractStates.Accepted, ContractTriggers.Fulfill, ContractStates.Fulfilled)]
        [TestCase(45, ContractStates.Accepted, ContractTriggers.Reject, ContractStates.Accepted)]
        [TestCase(46, ContractStates.Accepted, ContractTriggers.Refulfill, ContractStates.Accepted)]
        [TestCase(47, ContractStates.Accepted, ContractTriggers.Approve, ContractStates.Accepted)]
        [TestCase(48, ContractStates.Accepted, ContractTriggers.Pay, ContractStates.Accepted)]
        [TestCase(49, ContractStates.Fulfilled, ContractTriggers.Create, ContractStates.Fulfilled)]
        [TestCase(50, ContractStates.Fulfilled, ContractTriggers.Delete, ContractStates.Fulfilled)]
        [TestCase(51, ContractStates.Fulfilled, ContractTriggers.Submit, ContractStates.Fulfilled)]
        [TestCase(52, ContractStates.Fulfilled, ContractTriggers.Decline, ContractStates.Fulfilled)]
        [TestCase(53, ContractStates.Fulfilled, ContractTriggers.Accept, ContractStates.Fulfilled)]
        [TestCase(54, ContractStates.Fulfilled, ContractTriggers.Expire, ContractStates.Fulfilled)]
        [TestCase(55, ContractStates.Fulfilled, ContractTriggers.Cancel, ContractStates.Fulfilled)]
        [TestCase(56, ContractStates.Fulfilled, ContractTriggers.Fulfill, ContractStates.Fulfilled)]
        [TestCase(57, ContractStates.Fulfilled, ContractTriggers.Reject, ContractStates.Rejected)]
        [TestCase(58, ContractStates.Fulfilled, ContractTriggers.Refulfill, ContractStates.Fulfilled)]
        [TestCase(59, ContractStates.Fulfilled, ContractTriggers.Approve, ContractStates.Approved)]
        [TestCase(60, ContractStates.Fulfilled, ContractTriggers.Pay, ContractStates.Fulfilled)]
        [TestCase(61, ContractStates.Rejected, ContractTriggers.Create, ContractStates.Rejected)]
        [TestCase(62, ContractStates.Rejected, ContractTriggers.Delete, ContractStates.Rejected)]
        [TestCase(63, ContractStates.Rejected, ContractTriggers.Submit, ContractStates.Rejected)]
        [TestCase(64, ContractStates.Rejected, ContractTriggers.Decline, ContractStates.Rejected)]
        [TestCase(65, ContractStates.Rejected, ContractTriggers.Accept, ContractStates.Rejected)]
        [TestCase(66, ContractStates.Rejected, ContractTriggers.Expire, ContractStates.Rejected)]
        [TestCase(67, ContractStates.Rejected, ContractTriggers.Cancel, ContractStates.Rejected)]
        [TestCase(68, ContractStates.Rejected, ContractTriggers.Fulfill, ContractStates.Rejected)]
        [TestCase(69, ContractStates.Rejected, ContractTriggers.Reject, ContractStates.Rejected)]
        [TestCase(70, ContractStates.Rejected, ContractTriggers.Refulfill, ContractStates.Fulfilled)]
        [TestCase(71, ContractStates.Rejected, ContractTriggers.Approve, ContractStates.Rejected)]
        [TestCase(72, ContractStates.Rejected, ContractTriggers.Pay, ContractStates.Rejected)]
        [TestCase(73, ContractStates.Approved, ContractTriggers.Create, ContractStates.Approved)]
        [TestCase(74, ContractStates.Approved, ContractTriggers.Delete, ContractStates.Approved)]
        [TestCase(75, ContractStates.Approved, ContractTriggers.Submit, ContractStates.Approved)]
        [TestCase(76, ContractStates.Approved, ContractTriggers.Decline, ContractStates.Approved)]
        [TestCase(77, ContractStates.Approved, ContractTriggers.Accept, ContractStates.Approved)]
        [TestCase(78, ContractStates.Approved, ContractTriggers.Expire, ContractStates.Approved)]
        [TestCase(79, ContractStates.Approved, ContractTriggers.Cancel, ContractStates.Approved)]
        [TestCase(80, ContractStates.Approved, ContractTriggers.Fulfill, ContractStates.Approved)]
        [TestCase(81, ContractStates.Approved, ContractTriggers.Reject, ContractStates.Approved)]
        [TestCase(82, ContractStates.Approved, ContractTriggers.Refulfill, ContractStates.Approved)]
        [TestCase(83, ContractStates.Approved, ContractTriggers.Approve, ContractStates.Approved)]
        [TestCase(84, ContractStates.Approved, ContractTriggers.Pay, ContractStates.Payed)]
        [TestCase(85, ContractStates.Expired, ContractTriggers.Create, ContractStates.Expired)]
        [TestCase(86, ContractStates.Expired, ContractTriggers.Delete, ContractStates.Expired)]
        [TestCase(87, ContractStates.Expired, ContractTriggers.Submit, ContractStates.Expired)]
        [TestCase(88, ContractStates.Expired, ContractTriggers.Decline, ContractStates.Expired)]
        [TestCase(89, ContractStates.Expired, ContractTriggers.Accept, ContractStates.Expired)]
        [TestCase(90, ContractStates.Expired, ContractTriggers.Expire, ContractStates.Expired)]
        [TestCase(91, ContractStates.Expired, ContractTriggers.Cancel, ContractStates.Expired)]
        [TestCase(92, ContractStates.Expired, ContractTriggers.Fulfill, ContractStates.Expired)]
        [TestCase(93, ContractStates.Expired, ContractTriggers.Reject, ContractStates.Expired)]
        [TestCase(94, ContractStates.Expired, ContractTriggers.Refulfill, ContractStates.Expired)]
        [TestCase(95, ContractStates.Expired, ContractTriggers.Approve, ContractStates.Expired)]
        [TestCase(96, ContractStates.Expired, ContractTriggers.Pay, ContractStates.Expired)]
        [TestCase(97, ContractStates.Cancelled, ContractTriggers.Create, ContractStates.Cancelled)]
        [TestCase(98, ContractStates.Cancelled, ContractTriggers.Delete, ContractStates.Cancelled)]
        [TestCase(99, ContractStates.Cancelled, ContractTriggers.Submit, ContractStates.Cancelled)]
        [TestCase(100, ContractStates.Cancelled, ContractTriggers.Decline, ContractStates.Cancelled)]
        [TestCase(101, ContractStates.Cancelled, ContractTriggers.Accept, ContractStates.Cancelled)]
        [TestCase(102, ContractStates.Cancelled, ContractTriggers.Expire, ContractStates.Cancelled)]
        [TestCase(103, ContractStates.Cancelled, ContractTriggers.Cancel, ContractStates.Cancelled)]
        [TestCase(104, ContractStates.Cancelled, ContractTriggers.Fulfill, ContractStates.Cancelled)]
        [TestCase(105, ContractStates.Cancelled, ContractTriggers.Reject, ContractStates.Cancelled)]
        [TestCase(106, ContractStates.Cancelled, ContractTriggers.Refulfill, ContractStates.Cancelled)]
        [TestCase(107, ContractStates.Cancelled, ContractTriggers.Approve, ContractStates.Cancelled)]
        [TestCase(108, ContractStates.Cancelled, ContractTriggers.Pay, ContractStates.Cancelled)]
        [TestCase(109, ContractStates.Payed, ContractTriggers.Create, ContractStates.Payed)]
        [TestCase(110, ContractStates.Payed, ContractTriggers.Delete, ContractStates.Payed)]
        [TestCase(111, ContractStates.Payed, ContractTriggers.Submit, ContractStates.Payed)]
        [TestCase(112, ContractStates.Payed, ContractTriggers.Decline, ContractStates.Payed)]
        [TestCase(113, ContractStates.Payed, ContractTriggers.Accept, ContractStates.Payed)]
        [TestCase(114, ContractStates.Payed, ContractTriggers.Expire, ContractStates.Payed)]
        [TestCase(115, ContractStates.Payed, ContractTriggers.Cancel, ContractStates.Payed)]
        [TestCase(116, ContractStates.Payed, ContractTriggers.Fulfill, ContractStates.Payed)]
        [TestCase(117, ContractStates.Payed, ContractTriggers.Reject, ContractStates.Payed)]
        [TestCase(118, ContractStates.Payed, ContractTriggers.Refulfill, ContractStates.Payed)]
        [TestCase(119, ContractStates.Payed, ContractTriggers.Approve, ContractStates.Payed)]
        [TestCase(120, ContractStates.Payed, ContractTriggers.Pay, ContractStates.Payed)]
        [TestCase(121, ContractStates.Deleted, ContractTriggers.Create, ContractStates.Deleted)]
        [TestCase(122, ContractStates.Deleted, ContractTriggers.Delete, ContractStates.Deleted)]
        [TestCase(123, ContractStates.Deleted, ContractTriggers.Submit, ContractStates.Deleted)]
        [TestCase(124, ContractStates.Deleted, ContractTriggers.Decline, ContractStates.Deleted)]
        [TestCase(125, ContractStates.Deleted, ContractTriggers.Accept, ContractStates.Deleted)]
        [TestCase(126, ContractStates.Deleted, ContractTriggers.Expire, ContractStates.Deleted)]
        [TestCase(127, ContractStates.Deleted, ContractTriggers.Cancel, ContractStates.Deleted)]
        [TestCase(128, ContractStates.Deleted, ContractTriggers.Fulfill, ContractStates.Deleted)]
        [TestCase(129, ContractStates.Deleted, ContractTriggers.Reject, ContractStates.Deleted)]
        [TestCase(130, ContractStates.Deleted, ContractTriggers.Refulfill, ContractStates.Deleted)]
        [TestCase(131, ContractStates.Deleted, ContractTriggers.Approve, ContractStates.Deleted)]
        [TestCase(132, ContractStates.Deleted, ContractTriggers.Pay, ContractStates.Deleted)]
        [TestCase(133, ContractStates.Declined, ContractTriggers.Create, ContractStates.Declined)]
        [TestCase(134, ContractStates.Declined, ContractTriggers.Delete, ContractStates.Declined)]
        [TestCase(135, ContractStates.Declined, ContractTriggers.Submit, ContractStates.Declined)]
        [TestCase(136, ContractStates.Declined, ContractTriggers.Decline, ContractStates.Declined)]
        [TestCase(137, ContractStates.Declined, ContractTriggers.Accept, ContractStates.Declined)]
        [TestCase(138, ContractStates.Declined, ContractTriggers.Expire, ContractStates.Declined)]
        [TestCase(139, ContractStates.Declined, ContractTriggers.Cancel, ContractStates.Declined)]
        [TestCase(140, ContractStates.Declined, ContractTriggers.Fulfill, ContractStates.Declined)]
        [TestCase(141, ContractStates.Declined, ContractTriggers.Reject, ContractStates.Declined)]
        [TestCase(142, ContractStates.Declined, ContractTriggers.Refulfill, ContractStates.Declined)]
        [TestCase(143, ContractStates.Declined, ContractTriggers.Approve, ContractStates.Declined)]
        [TestCase(144, ContractStates.Declined, ContractTriggers.Pay, ContractStates.Declined)]
        [TestCase(144, ContractStates.Start, ContractTriggers.Resubmit, ContractStates.Start)]
        [TestCase(144, ContractStates.Created, ContractTriggers.Resubmit, ContractStates.Created)]
        [TestCase(144, ContractStates.Submitted, ContractTriggers.Resubmit, ContractStates.Submitted)]
        [TestCase(144, ContractStates.Accepted, ContractTriggers.Resubmit, ContractStates.Accepted)]
        [TestCase(144, ContractStates.Fulfilled, ContractTriggers.Resubmit, ContractStates.Fulfilled)]
        [TestCase(144, ContractStates.Approved, ContractTriggers.Resubmit, ContractStates.Approved)]
        [TestCase(144, ContractStates.Payed, ContractTriggers.Resubmit, ContractStates.Payed)]
        [TestCase(144, ContractStates.Deleted, ContractTriggers.Resubmit, ContractStates.Deleted)]
        [TestCase(144, ContractStates.Declined, ContractTriggers.Resubmit, ContractStates.Submitted)]
        [TestCase(144, ContractStates.Expired, ContractTriggers.Resubmit, ContractStates.Submitted)]
        [TestCase(144, ContractStates.Cancelled, ContractTriggers.Resubmit, ContractStates.Submitted)]
        [TestCase(144, ContractStates.Rejected, ContractTriggers.Resubmit, ContractStates.Rejected)]
        public void ItShouldHandleAllPossibleStateTransitions(int testNumber,
            ContractStates startState,
            ContractTriggers incomingTrigger,
            ContractStates finalState
            )
        {
            Trace.WriteLine(
                String.Format(
                    "Test # {0} transition from {1} with trigger {2} expecting final state of {3} with {4} raised",
                    testNumber, startState, incomingTrigger, finalState, _eventType == null ? "no event" : _eventType.Name));
            Arrange(startState);
            Act(incomingTrigger);
            bool pass = startState != finalState;
            Assert.AreEqual(finalState, _stateMachine.State);
            if (pass)
            {
                _bus.Received(1).Publish(Arg.Is<IEvent>(x => x.GetType() == _eventType));
                _logger.Received(0).Debug(Arg.Any<String>());
            }
            else
            {
                _bus.Received(0).Publish(Arg.Any<IEvent>());
                _logger.Received(1).Warn(Arg.Any<String>());
                _logger.Received(1).Warn(Arg.Is<String>(x => x.Contains("Trace: ")));
            }
        }
    }
}