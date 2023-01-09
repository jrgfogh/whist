namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest<GameConductorService>
    {
        protected override string TestUrl => "http://localhost:5001";

        [Test]
        public void PlayerAIsPrompted()
        {
        }
    }
}
