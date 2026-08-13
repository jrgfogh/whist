namespace Whist.Server.Tests
{
    public sealed class Event(string sender, string message)
    {
        public readonly string Sender = sender;

        public readonly string Message = message;

        public override bool Equals(object? obj)
        {
            return obj is Event @event &&
                   Sender == @event.Sender &&
                   String.Compare(Message, @event.Message, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sender, Message);
        }

        public override string ToString()
        {
            return "Sender: " + Sender + ", Message: " + Message;
        }
    }
}