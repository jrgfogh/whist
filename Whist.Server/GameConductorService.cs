namespace Whist.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Hosting;
    using Rules;

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
            this._hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public void StartGame()
        {
            this._gameConductorThread = new Thread(async () =>
            {
                var gameConductor = new GameConductor(this);
                await gameConductor.ConductGame();
            });
            this._gameConductorThread.Start();
        }

        private IWhistClient GetClient(int playerIndex)
        {
            var connectionId = this.ConnectionIdsAtTable[playerIndex];
            return this._hubContext.Clients.Client(connectionId);
        }

        public async Task<string> PromptForBid(int playerToBid)
        {
            this._promise = new TaskCompletionSource<string>();
            await this.GetClient(playerToBid).PromptForBid();
            return await this._promise.Task;
        }

        public async Task<string> PromptForTrump(int winner)
        {
            this._promise = new TaskCompletionSource<string>();
            await this.GetClient(winner).PromptForTrump();
            return await this._promise.Task;
        }

        public async Task<string> PromptForBuddyAce(int winner)
        {
            this._promise = new TaskCompletionSource<string>();
            await this.GetClient(winner).PromptForBuddyAce();
            return await this._promise.Task;
        }

        public async Task DealCards(int playerIndex, List<Card> cards)
        {
            await this.GetClient(playerIndex).ReceiveDealtCards(cards.Select(c => c.ToString()));
        }

        // TODO(jrgfogh): Unduplicate!
        // TODO(jrgfogh): Check that we are expecting the message we received.
        public void ReceiveBid(string bid)
        {
            this._promise.TrySetResult(bid);
        }

        public void ReceiveTrump(string trump)
        {
            this._promise.TrySetResult(trump);
        }

        public void ReceiveBuddyAce(string buddyAce)
        {
            this._promise.TrySetResult(buddyAce);
        }
    }
}