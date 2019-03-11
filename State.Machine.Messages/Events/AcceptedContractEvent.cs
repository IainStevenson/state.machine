using System.Collections;

namespace State.Machine.Messages.Events
{

    [ContractTrigger(ContractTriggers.Accept)]
    public class AcceptedContractEvent : IContractEvent
    {
        public AcceptedContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}