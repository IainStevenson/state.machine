using System.Collections;

namespace State.Machine.Messages.Commands
{
    [ContractTrigger(ContractTriggers.Delete)]
    public class DeleteContractCommand : IContractCommand
    {
        public DeleteContractCommand(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; set; }
    }
}