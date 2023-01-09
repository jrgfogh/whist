namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest<GameConductorService>
    {
        protected override string TestUrl => "http://localhost:5001";

        [Test]
        public void PlayerAIsPrompted()
        {
            var receivedEvents = TestPlayers["Player A"].ReceivedEvents;
            var gameEvent = receivedEvents.Take();
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "Please take your cards!")));
            gameEvent = receivedEvents.Take();
            Assert.That(gameEvent, Is.EqualTo(new Event("To Player A", "Please bid!")));
        }
    }
}
