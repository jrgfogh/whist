using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Whist.Server.Tests
{
    public class BiddingTests : IntegrationTest<GameConductorService>
    {
        protected override string TestUrl => "http://localhost:5000";

        [Test]
        [TestCase(
@"To All: Please take your cards!
To Player A: Please bid!
Player A: Pass!
To All: Player A chooses pass
To Player B: Please bid!
Player B: Pass!
To All: Player B chooses pass
To Player C: Please bid!
Player C: Pass!
To All: Player C chooses pass
To Player D: Please bid!
Player D: 9 common!
To All: Player D chooses 9 common
To All: Player D wins bidding, 9 common
To Player D: Please choose the trump!
Player D: Trump is S!
To All: Player D chooses Trump is S
To Player D: Please choose the buddy ace!
Player D: Buddy ace is H!
To All: Player D chooses Buddy ace is H
To All: Start playing!
To Player A: Please play a card!
Player A: H3
To All: Player A chooses H3
To Player B: Please play a card!
Player B: C5
To All: Player B chooses C5
To Player C: Please play a card!
Player C: D5
To All: Player C chooses D5
To Player D: Please play a card!
Player D: H5
To All: Player D chooses H5
To All: Player D wins the trick
To Player D: Please play a card!
Player D: C2
To All: Player D chooses C2
To Player A: Please play a card!
Player A: C3")]
        public async Task BiddingRound(string input)
        {
            await PlayersConnect();
            PlayersJoinTable();
            await ProcessEvents(input);
        }

        private async Task PlayersConnect()
        {
            foreach (var connection in OrderedConnections())
                await connection.StartAsync().ConfigureAwait(false);
        }

        private void PlayersJoinTable()
        {
            var conductorService = Host.Services.GetRequiredService<IConductorService>();
            foreach (var connection in OrderedConnections())
                conductorService.JoinTable(connection.ConnectionId!);
        }

        private IEnumerable<HubConnection> OrderedConnections()
        {
            return TestPlayers.OrderBy(pair => pair.Key).Select(pair => pair.Value.Connection);
        }
    }
}