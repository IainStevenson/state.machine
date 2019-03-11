using System;

namespace State.Machine.Messaging
{
    public class EventBus : IEventBus
    {
        public void Publish(IEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}