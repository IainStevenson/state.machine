using System.Collections;

namespace State.Machine.Messages.Events
{
     [ContractTrigger(ContractTriggers.Expire)]
    public class ExpiredContractEvent : IContractEvent
    {
        public ExpiredContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}