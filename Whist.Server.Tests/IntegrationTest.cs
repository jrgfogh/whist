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

        // TODO(jrgfogh): Use System.Threading.Channels instead?
        private readonly BlockingCollection<(string, Event)> _receivedEvents = new();
        private GameConductorService _conductorService;
        private HubConnection _connectionA;
        private HubConnection _connectionB;
        private HubConnection _connectionC;
        private HubConnection _connectionD;

        private static Event ParseEvent(string line)
        {
            var parts = line.Split(": ");
            var sender = parts[0];
            var message = parts[1];
            return new Event(sender, message);
        }

        private static IEnumerable<Event> ParseEvents(string input) =>
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
            _conductorService = host.Services.GetRequiredService<GameConductorService>();
            await host.StartAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            var uri = new Uri(TestUrl + "/WhistHub");
            _connectionA = await OpenConnection(uri, "Player A");
            _connectionB = await OpenConnection(uri, "Player B");
            _connectionC = await OpenConnection(uri, "Player C");
            _connectionD = await OpenConnection(uri, "Player D");
        }

        private async Task<HubConnection> OpenConnection(Uri serverUri, string playerName)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(serverUri)
                .Build();

            void HandleEvent(string methodName) =>
                connection.On(methodName, () =>
                    _receivedEvents.Add((playerName, new Event("To " + playerName, ""))));

            HandleEvent("PromptForBid");
            HandleEvent("PromptForTrump");
            HandleEvent("PromptForBuddyAce");
            HandleEvent("UpdatePlayersAtTable");
            HandleEvent("UpdateListOfTables");

            connection.On("ReceiveDealtCards", (List<string> cards) =>
                _receivedEvents.Add((playerName, new Event("To " + playerName, ""))));
            connection.On("ReceiveBid", (string user, string bid) =>
                _receivedEvents.Add((playerName, new Event("To " + playerName, ""))));
            connection.On("ReceiveTrump", (string trump) =>
                _receivedEvents.Add((playerName, new Event("To " + playerName, ""))));
            connection.On("ReceiveBuddyAce", (string buddyAce) =>
                _receivedEvents.Add((playerName, new Event("To " + playerName, ""))));
            await connection.StartAsync().ConfigureAwait(false);
            return connection;
        }
    }
}