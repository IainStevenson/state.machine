using System.Collections;
using State.Machine.Messaging;

namespace State.Machine.Messages
{
    public interface IContractEvent : IEvent
    {
        IDictionary Properties { get; }
    }
}