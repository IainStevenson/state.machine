using System.Collections;

namespace State.Machine.Messages.Commands
{
    [ContractTrigger(ContractTriggers.Cancel)]
    public class CancelContractCommand : IContractCommand
    {
        public CancelContractCommand(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; set; }
    }
}