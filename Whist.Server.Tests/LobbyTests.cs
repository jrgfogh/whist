using Microsoft.AspNetCore.SignalR.Client;

namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5001";

        [Test]
        public async Task PlayerAIsPrompted()
        {
            var gameEvent = _receivedEvents.Take();
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "PromptForBid")));
        }
    }
}
