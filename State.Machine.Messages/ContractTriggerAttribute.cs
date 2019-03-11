using System;

namespace State.Machine.Messages
{
    /// <summary>
    ///     When decorating an IContractCommand class for Contracts, will fire the specified Trigger
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContractTriggerAttribute : Attribute
    {
        public ContractTriggerAttribute(ContractTriggers trigger)
        {
            Trigger = trigger;
        }

        public ContractTriggers Trigger { get; private set; }
    }
}