namespace State.Machine.Messaging
{
    /// <summary>
    ///     Defines event bus handling
    /// </summary>
    public interface IEventBus
    {
        void Publish(IEvent @event);
    }
}