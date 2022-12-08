using Microsoft.AspNetCore.SignalR.Client;

namespace Whist.Server.Tests
{
    public class LobbyTests : IntegrationTest
    {
        protected override string TestUrl { get; } = "http://localhost:5001";

        [Test]
        public async Task EveryoneSelectsTable()
        {
            await Task.WhenAll(
                _connectionA.SendAsync("SelectTable", "Table"),
                _connectionB.SendAsync("SelectTable", "Table"),
                _connectionC.SendAsync("SelectTable", "Table"),
                _connectionD.SendAsync("SelectTable", "Table"));
            var (playerName, gameEvent) = _receivedEvents.Take();
            Assert.Pass();
        }
    }
}
