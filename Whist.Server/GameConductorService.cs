using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Whist.Rules;

namespace Whist.Server
{
    // TODO(jrgfogh): Extract a class for the IMovePrompter?
    public sealed class GameConductorService : BackgroundService, IMovePrompter
    {
        private readonly IHubContext<WhistHub, IWhistClient> _hubContext;
        private TaskCompletionSource<string> _promise;

        // TODO(jrgfogh): Rename!
        private Thread _gameConductorThread;

        public List<string> ConnectionIdsAtTable { get; } = new List<string>();

        public GameConductorService(IHubContext<WhistHub, IWhistClient> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public void StartGame()
        {
            _gameConductorThread = new Thread(async () =>
            {
                var gameConductor = new GameConductor(this);
                await gameConductor.ConductGame();
            });
            _gameConductorThread.Start();
        }

        private IWhistClient GetClient(int playerIndex)
        {
            var connectionId = ConnectionIdsAtTable[playerIndex];
            return _hubContext.Clients.Client(connectionId);
        }

        public async Task<string> PromptForBid(int playerToBid)
        {
            _promise = new TaskCompletionSource<string>();
            await GetClient(playerToBid).PromptForBid();
            return await _promise.Task;
        }

        public async Task<string> PromptForTrump(int winner)
        {
            _promise = new TaskCompletionSource<string>();
            await GetClient(winner).PromptForTrump();
            return await _promise.Task;
        }

        public async Task<string> PromptForBuddyAce(int winner)
        {
            _promise = new TaskCompletionSource<string>();
            await GetClient(winner).PromptForBuddyAce();
            return await _promise.Task;
        }

        public async Task DealCards(int playerIndex, List<Card> cards)
        {
            await GetClient(playerIndex).ReceiveDealtCards(cards.Select(c => c.ToString()));
        }

        // TODO(jrgfogh): Unduplicate!
        // TODO(jrgfogh): Check that we are expecting the message we received.
        public void ReceiveBid(string bid)
        {
            _promise.TrySetResult(bid);
        }

        public void ReceiveTrump(string trump)
        {
            _promise.TrySetResult(trump);
        }

        public void ReceiveBuddyAce(string buddyAce)
        {
            _promise.TrySetResult(buddyAce);
        }

        public async Task AnnounceWinner(string winner, string winningBid)
        {
            await _hubContext.Clients.All.AnnounceWinner(winner, winningBid);
        }
    }
}