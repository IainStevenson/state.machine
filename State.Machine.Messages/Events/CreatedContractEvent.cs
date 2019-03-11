using System.Collections;

namespace State.Machine.Messages.Events
{
     [ContractTrigger(ContractTriggers.Create)]
    public class CreatedContractEvent : IContractEvent
    {
        public CreatedContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}