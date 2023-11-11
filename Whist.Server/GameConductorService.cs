using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whist.Rules;

namespace Whist.Server
{
    public sealed class GameConductorService : IMovePrompter, IAsyncDisposable, IConductorService
    {
        private readonly IHubContext<WhistHub, IWhistClient> _hubContext;
        private TaskCompletionSource<string>? _promise;
        private readonly object _connectionIdsSyncLock = new();
        private readonly List<string> _connectionIdsAtTable = new();
        private readonly GameTaskManager _gameTaskManager;

        public GameConductorService(IHubContext<WhistHub, IWhistClient> hubContext)
        {
            _hubContext = hubContext;
            _gameTaskManager = new GameTaskManager(this);
        }

        public ValueTask DisposeAsync()
        {
            return _gameTaskManager.DisposeAsync();
        }

        private IWhistClient GetClient(int playerIndex)
        {
            lock (_connectionIdsSyncLock)
            {
                var connectionId = _connectionIdsAtTable[playerIndex];
                return _hubContext.Clients.Client(connectionId);
            }
        }

        private async Task<string> Prompt(Func<Task> prompter)
        {
            _promise = new TaskCompletionSource<string>();
            await prompter();
            // TODO(jrgfogh): Validate the result:
            return await _promise.Task;
        }

        public Task<string> PromptForBid(int playerToBid)
        {
            return Prompt(GetClient(playerToBid).PromptForBid);
        }

        public Task<string> PromptForTrump(int winner)
        {
            return Prompt(GetClient(winner).PromptForTrump);
        }

        public Task<string> PromptForBuddyAce(int winner)
        {
            return Prompt(GetClient(winner).PromptForBuddyAce);
        }

        public Task<string> PromptForCard(int playerToPlay)
        {
            return Prompt(GetClient(playerToPlay).PromptForCard);
        }

        public async Task DealCards(int playerIndex, List<Card> cards)
        {
            await GetClient(playerIndex).ReceiveDealtCards(cards.Select(c => c.ToString()));
        }

        public async Task ReceiveChoice(string connectionId, string choice)
        {
            await _hubContext.Clients.All.ReceiveChoice(UserName(connectionId), choice);
            // TODO(jrgfogh): Validate the result:
            _promise?.TrySetResult(choice);
        }

        public async Task AnnounceWinner(string winner)
        {
            await _hubContext.Clients.All.AnnounceWinner(winner);
        }

        public async Task AnnounceBiddingWinner(string winner, string winningBid)
        {
            await _hubContext.Clients.All.AnnounceBiddingWinner(winner, winningBid);
        }

        public async Task StartPlaying()
        {
            await _hubContext.Clients.All.StartPlaying();
        }

        public void JoinTable(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                _connectionIdsAtTable.Add(connectionId);
                if (_connectionIdsAtTable.Count == 4) _gameTaskManager.StartGame();
            }
        }

        public async Task LeaveTable(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                _connectionIdsAtTable.Remove(connectionId);
            }
            await _gameTaskManager.StopGame();
        }

        private string UserName(string connectionId)
        {
            lock (_connectionIdsSyncLock)
            {
                var index = _connectionIdsAtTable.IndexOf(connectionId);
                if (index == -1)
                    return "";
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