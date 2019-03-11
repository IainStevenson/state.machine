namespace State.Machine.Messaging
{
    /// <summary>
    ///     Defines command bus handling
    /// </summary>
    public interface ICommandBus
    {
        void Issue(ICommand @event);
    }
}