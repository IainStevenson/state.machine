using System.Collections;

namespace State.Machine.Messages.Events
{
     [ContractTrigger(ContractTriggers.Resubmit)]
    public class ResubmittedContractEvent : IContractEvent
    {
        public ResubmittedContractEvent(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; private set; }
    }
}