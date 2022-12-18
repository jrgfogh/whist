using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace Whist.Server.Tests
{
    public abstract class IntegrationTest
    {
        // The different tests can't bind to the same port:
        protected abstract string TestUrl { get; }

        protected sealed class TestPlayer
        {
            public readonly BlockingCollection<Event> ReceivedEvents = new();
            // TODO(jrgfogh): Use System.Threading.Channels instead?
            public readonly HubConnection Connection;

            public TestPlayer(HubConnection connection)
            {
                Connection = connection;
            }
        }

        protected GameConductorService ConductorService;
        protected Dictionary<string, TestPlayer> TestPlayers = new();

        private static Event ParseEvent(string line)
        {
            var parts = line.Split(": ");
            var sender = parts[0];
            var message = parts[1];
            return new Event(sender, message);
        }

        protected static IEnumerable<Event> ParseEvents(string input) =>
            input.Split(Environment.NewLine).Select(ParseEvent);

        private static string ServerPath() =>
            Path.GetFullPath(Path.Join(TestContext.CurrentContext.TestDirectory, @"../../../../Whist.Server"));

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.CurrentDirectory = ServerPath();
            var host = Program.CreateHostBuilder(null)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls(TestUrl);
                })
                .Build();
            ConductorService = host.Services.GetRequiredService<GameConductorService>();
            await host.StartAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            var uri = new Uri(TestUrl + "/WhistHub");
            await OpenConnection(uri, "Player A");
            await OpenConnection(uri, "Player B");
            await OpenConnection(uri, "Player C");
            await OpenConnection(uri, "Player D");
        }

        private async Task OpenConnection(Uri serverUri, string playerName)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(serverUri)
                .Build();


            void HandleEvent(string methodName) =>
                connection.On(methodName: methodName, () =>
                        TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, methodName)));

            // TODO(jrgfogh): Deal with parameters:
            HandleEvent("PromptForBid");
            HandleEvent("PromptForTrump");
            HandleEvent("PromptForBuddyAce");
            connection.On("UpdatePlayersAtTable", (IEnumerable<string> players) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, "UpdatePlayersAtTable")));
            connection.On("ReceiveDealtCards", (IEnumerable<string> cards) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, "ReceiveDealtCards")));
            connection.On("ReceiveBid", (string user, string bid) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, user + " bids " + bid)));
            connection.On("AnnounceWinner", (string user, string bid) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, user + " wins, " + bid)));
            HandleEvent("ReceiveTrump");
            HandleEvent("ReceiveBuddyAce");
            await connection.StartAsync().ConfigureAwait(false);
            TestPlayers[playerName] = new TestPlayer(connection);
        }

        protected HubConnection GetConnection(string sender) =>
            TestPlayers[sender].Connection;
    }
}