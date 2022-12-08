namespace Whist.Server.Tests
{
    public class Event
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