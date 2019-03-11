using System.Collections;

namespace State.Machine.Messages.Commands
{
    [ContractTrigger(ContractTriggers.Approve)]
    public class ApproveContractCommand : IContractCommand
    {
        public ApproveContractCommand(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; set; }
    }
}