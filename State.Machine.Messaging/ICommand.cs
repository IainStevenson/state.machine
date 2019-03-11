using System.Collections;

namespace State.Machine.Messaging
{
    public interface ICommand
    {
        IDictionary Properties { get; set; }
    }
}