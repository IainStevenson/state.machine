using System.Collections;

namespace State.Machine.Messages.Commands
{
    [ContractTrigger(ContractTriggers.Fulfill)]
    public class FulfillContractCommand : IContractCommand
    {
        public FulfillContractCommand(IDictionary properties)
        {
            Properties = properties;
        }

        public IDictionary Properties { get; set; }
    }
}