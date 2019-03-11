using System.Collections;
using State.Machine.Messaging;

namespace State.Machine.Messages
{
    public interface IContractCommand : ICommand
    {
        IDictionary Properties { get; set; }
    }
}