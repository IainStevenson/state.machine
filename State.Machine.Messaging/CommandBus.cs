using System;

namespace State.Machine.Messaging
{
    public class CommandBus : ICommandBus
    {
        public void Issue(ICommand @event)
        {
            throw new NotImplementedException();
        }
    }
}