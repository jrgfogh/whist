using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Whist.Server.Tests
{
    public class BiddingTests
    {
        private HubConnection _connectionA;
        private HubConnection _connectionB;
        private HubConnection _connectionC;
        private HubConnection _connectionD;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.CurrentDirectory = ServerPath();
            var host = Program.CreateHostBuilder(null)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://localhost:5000");
                })
                .Build();
            _ = Task.Factory.StartNew(host.Run);
        }

        private static string ServerPath() =>
            Path.GetFullPath(Path.Join(TestContext.CurrentContext.TestDirectory, @"../../../../Whist.Server"));

        [SetUp]
        public async Task Setup()
        {
            var uri = new Uri("http://localhost:5000/WhistHub");
            _connectionA = await OpenConnection(uri, "Player A");
            _connectionB = await OpenConnection(uri, "Player B");
            _connectionC = await OpenConnection(uri, "Player C");
            _connectionD = await OpenConnection(uri, "Player D");
        }

        [Test]
        [TestCase(
@"To All: Bidding Round
To Player A: Please bid
Player A: pass
To Player B: Please bid
Player B: pass
To Player C: Please bid
Player C: pass
To Player D: Please bid
Player D: 9 common
To All: Player D wins, 9 common")]
        public async Task BiddingRoundAsync(string input)
        {
            Assert.Pass();
        }

        private async Task PromptForBidAsync(HubConnection connection)
        {
            // await connection.InvokeAsync("Send", "foo");
            // TODO(JRGF): Find a way to prompt for a bid!
        }

        private static IEnumerable<Event> ParseEvents(string input) =>
            input.Split(Environment.NewLine).Select(ParseEvent);

        private static Event ParseEvent(string line)
        {
            var parts = line.Split(": ");
            var sender = parts[0];
            var message = parts[1];
            return new Event(sender, message);
        }

        private static async Task<HubConnection> OpenConnection(Uri serverUri, string playerName)
        {
            void WriteMessage(string message)
            {
                Console.WriteLine($"{playerName} received message: {message}");
            }

            var connection = new HubConnectionBuilder()
                .WithUrl(serverUri)
                .Build();

            connection.On("PromptForBid", async () =>
                await connection.SendAsync("SendBid", playerName, ""));
            connection.On("PromptForTrump", async () =>
                await connection.SendAsync("SendTrump", ""));
            connection.On("PromptForBuddyAce", async () =>
                await connection.SendAsync("SendBuddyAce", ""));

            connection.On("ReceiveDealtCards", (List<string> cards) => WriteMessage("You were dealt the cards: " + string.Join(", ", cards)));
            connection.On("ReceiveBid", (string user, string bid) => WriteMessage($"Received bid \"{bid}\" from \"{user}.\""));
            connection.On("ReceiveTrump", (string trump) => WriteMessage($"The trump is {trump}."));
            connection.On("ReceiveBuddyAce", (string buddyAce) => WriteMessage($"The buddy ace is {buddyAce}."));
            await connection.StartAsync();
            return connection;
        }
    }
}