using System.Collections;

namespace State.Machine.Messages.Events
{
     [ContractTrigger(ContractTriggers.Decline)]
    public class DeclinedContractEvent : IContractEvent
    {
        public DeclinedContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}