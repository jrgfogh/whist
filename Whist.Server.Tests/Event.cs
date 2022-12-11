namespace Whist.Server.Tests
{
    public sealed class Event
    {
        public readonly string Sender;

        public readonly string Message;

        public Event(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public override bool Equals(object? obj)
        {
            return obj is Event @event &&
                   Sender == @event.Sender &&
                   Message == @event.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sender, Message);
        }
    }
}