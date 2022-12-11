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
        protected readonly BlockingCollection<Event> _receivedEvents = new();
        protected GameConductorService _conductorService;
        protected HubConnection _connectionA;
        protected HubConnection _connectionB;
        protected HubConnection _connectionC;
        protected HubConnection _connectionD;

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
                connection.On(methodName: methodName, () =>
                    _receivedEvents.Add(new Event("To " + playerName, methodName)));

            // TODO(jrgfogh): Deal with parameters:
            HandleEvent("PromptForBid");
            HandleEvent("PromptForTrump");
            HandleEvent("PromptForBuddyAce");
            connection.On("UpdatePlayersAtTable", (IEnumerable<string> players) =>
                    _receivedEvents.Add(new Event("To " + playerName, "UpdatePlayersAtTable")));
            HandleEvent("ReceiveDealtCards");
            connection.On("ReceiveDealtCards", (IEnumerable<string> cards) =>
                    _receivedEvents.Add(new Event("To " + playerName, "ReceiveDealtCards")));
            HandleEvent("ReceiveDealtCards");
            HandleEvent("ReceiveBid");
            HandleEvent("ReceiveTrump");
            HandleEvent("ReceiveBuddyAce");
            await connection.StartAsync().ConfigureAwait(false);
            return connection;
        }

        protected HubConnection GetConnection(string sender) =>
            sender switch
            {
                "Player A" => _connectionA,
                "Player B" => _connectionB,
                "Player C" => _connectionC,
                "Player D" => _connectionD,
                _ => throw new ArgumentException("Invalid input"),
            };

        protected static async Task SendMessageAsync(HubConnection connection, string message)
        {
            switch (message)
            {
                case "pass":
                    await connection.SendAsync("SendBid", message);
                    break;
            }
        }
    }
}