namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest<GameConductorService>
    {
        protected override string TestUrl => "http://localhost:5001";

        [Test]
        [TestCase(
@"Player A: <connects>
Player A: My name is Albert!
Player B: <connects>
Player B: My name is Brigitte!
Player C: <connects>
Player C: My name is Charlie!
Player D: <connects>
Player D: My name is Dina!")]
        public async Task Lobby(string input)
        {
            await ProcessEvents(input);
        }
    }
}
