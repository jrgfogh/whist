using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        protected GameConductorService ConductorService = null!;
        private IHost _host = null!;
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
            _host = Program.CreateHostBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls(TestUrl);
                })
                .Build();
            ConductorService = _host.Services.GetRequiredService<GameConductorService>();
            await _host.StartAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _host.StopAsync();
            _host.Dispose();
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

            void HandleEvent(string methodName, string message) =>
                connection.On(methodName: methodName, () =>
                        TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, message)));

            HandleEvent("PromptForBid", "Please bid!");
            HandleEvent("PromptForTrump", "Please choose the trump!");
            HandleEvent("PromptForBuddyAce", "Please choose the buddy ace!");
            HandleEvent("PromptForCard", "Please play a card!");
            HandleEvent("StartPlaying", "Start playing!");
            connection.On("UpdatePlayersAtTable", (IEnumerable<string> players) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, "UpdatePlayersAtTable")));
            connection.On("ReceiveDealtCards", (IEnumerable<string> cards) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, "Please take your cards!")));
            connection.On("AnnounceBiddingWinner", (string winner, string bid) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, winner + " wins bidding, " + bid)));
            connection.On("AnnounceWinner", (string winner) =>
                    TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName, winner + " wins the trick")));
            connection.On("ReceiveChoice", (string chooser, string choice) =>
                TestPlayers[playerName].ReceivedEvents.Add(new Event("To " + playerName,
                chooser + " chooses " + choice)));
            await connection.StartAsync().ConfigureAwait(false);
            TestPlayers[playerName] = new TestPlayer(connection);
        }

        protected HubConnection GetConnection(string sender) =>
            TestPlayers[sender].Connection;

        protected async Task SendChoice(Event expectedEvent)
        {
            await GetConnection(expectedEvent.Sender).SendAsync("SendChoice", TrimMessage(expectedEvent)).ConfigureAwait(false);
        }

        private static string TrimMessage(Event expectedEvent)
        {
            return expectedEvent.Message.TrimEnd('!');
        }
    }
}