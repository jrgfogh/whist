using Microsoft.AspNetCore.SignalR.Client;

namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5001";

        [Test]
        public void PlayerAIsPrompted()
        {
            var gameEvent = _receivedEvents.Take();
            for (int i = 0; i < 4; i++)
            {
                Assert.That(gameEvent.Message, Is.EqualTo("UpdatePlayersAtTable"));
                gameEvent = _receivedEvents.Take();
            }
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "PromptForBid")));
        }
    }
}
