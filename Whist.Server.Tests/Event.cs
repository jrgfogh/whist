namespace Whist.Server.Tests
{
    internal class Event
    {
        public readonly string Sender;

        public readonly string Message;

        public Event(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}