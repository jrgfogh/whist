namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5001";

        [Test]
        public void PlayerAIsPrompted()
        {
            var receivedEvents = TestPlayers["Player A"].ReceivedEvents;
            var gameEvent = receivedEvents.Take();
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "ReceiveDealtCards")));
            gameEvent = receivedEvents.Take();
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "PromptForBid")));
        }
    }
}
