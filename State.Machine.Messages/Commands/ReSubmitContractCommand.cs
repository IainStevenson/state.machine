using System.Collections;

namespace State.Machine.Messages.Commands
{
    [ContractTrigger(ContractTriggers.Resubmit)]
    public class ResubmitContractCommand : IContractCommand
    {
        public ResubmitContractCommand(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; set; }
    }
}