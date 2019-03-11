using System.Collections;

namespace State.Machine.Messages.Events
{
     [ContractTrigger(ContractTriggers.Refulfill)]
    public class RefulfilledContractEvent : IContractEvent
    {
        public RefulfilledContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}