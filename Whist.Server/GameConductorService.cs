using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Whist.Rules;

namespace Whist.Server
{
    public sealed class GameConductorService : BackgroundService, IMovePrompter
    {
        private readonly IHubContext<WhistHub, IWhistClient> _hubContext;
        private TaskCompletionSource<string> _promise;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly object _connectionIdsSyncLock = new();
        private readonly List<string> _connectionIdsAtTable = new();

        public GameConductorService(IHubContext<WhistHub, IWhistClient> hubContext)
        {
            _hubContext = hubContext;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public override void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public void StartGame()
        {
            Task.Run(async () =>
            {
                var gameConductor = new GameConductor(this);
                await gameConductor.ConductGame().WaitAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
            });
        }

        private IWhistClient GetClient(int playerIndex)
        {
            lock (_connectionIdsSyncLock)
            {
                var connectionId = _connectionIdsAtTable[playerIndex];
                return _hubContext.Clients.Client(connectionId);
            }
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
            var receivedBuddyAce = await _promise.Task;
            // TODO(jrgfogh): Test this!
            if (!receivedBuddyAce.StartsWith("Buddy ace is ", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Received unexpected buddy ace message: " + receivedBuddyAce);
            return receivedBuddyAce;
        }

        public async Task<string> PromptForCard(int playerToPlay)
        {
            _promise = new TaskCompletionSource<string>();
            await GetClient(playerToPlay).PromptForCard();
            return await _promise.Task;
        }

        public async Task DealCards(int playerIndex, List<Card> cards)
        {
            await GetClient(playerIndex).ReceiveDealtCards(cards.Select(c => c.ToString()));
        }

        public void ReceiveChoice(string choice)
        {
            _promise.TrySetResult(choice);
        }

        public async Task AnnounceWinner(string winner)
        {
            await _hubContext.Clients.All.AnnounceWinner(winner);
        }

        public async Task AnnounceBiddingWinner(string winner, string winningBid)
        {
            await _hubContext.Clients.All.AnnounceBiddingWinner(winner, winningBid);
        }

        public void LeaveTable(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                _connectionIdsAtTable.Remove(connectionId);
            }
        }

        public void JoinTable(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                _connectionIdsAtTable.Add(connectionId);
                if (_connectionIdsAtTable.Count == 4)
                    StartGame();
            }
        }

        public string UserName(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                var index = _connectionIdsAtTable.IndexOf(connectionId);
                var playerNames = new[]
                {
                    "Player A",
                    "Player B",
                    "Player C",
                    "Player D",
                };
                return playerNames[index];
            }
        }
    }
}