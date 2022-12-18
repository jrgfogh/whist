using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whist.Server
{
    public sealed class WhistHub : Hub<IWhistClient>
    {
        private readonly GameConductorService _gameConductorService;

        // TODO(jrgfogh): Move this into the game conductor service instead?
        private readonly object _syncLock = new();

        public WhistHub(GameConductorService gameConductorService)
        {
            _gameConductorService = gameConductorService;
        }

        public override async Task OnConnectedAsync()
        {
            lock (_syncLock)
            {
                _gameConductorService.ConnectionIdsAtTable.Add(Context.ConnectionId);
                if (_gameConductorService.ConnectionIdsAtTable.Count == 4)
                    _gameConductorService.StartGame();
            }
            // TODO(jrgfogh): Don't send a list, just send an event saying who joined, and send it to everyone.
            //await this.Clients.Caller.UpdatePlayersAtTable(connectionIds);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            List<string> connectionIds = null;
            lock (_syncLock)
            {
                if (_gameConductorService.ConnectionIdsAtTable.Remove(Context.ConnectionId))
                    connectionIds = new(_gameConductorService.ConnectionIdsAtTable);
            }
            if (connectionIds != null)
                await Clients.All.UpdatePlayersAtTable(connectionIds);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendBid(string bid)
        {
            await Clients.All.ReceiveBid(UserNameOfCaller(), bid);
            _gameConductorService.ReceiveBid(bid);
        }

        private string UserNameOfCaller()
        {
            var index = _gameConductorService.ConnectionIdsAtTable.IndexOf(Context.ConnectionId);
            var playerNames = new[] {
                "Player A",
                "Player B",
                "Player C",
                "Player D",
                };
            return playerNames[index];
        }

        public async Task SendTrump(string trump)
        {
            _gameConductorService.ReceiveTrump(trump);
            await Clients.All.ReceiveTrump(trump);
        }

        public async Task SendBuddyAce(string buddyAce)
        {
            _gameConductorService.ReceiveBuddyAce(buddyAce);
            await Clients.All.ReceiveBuddyAce(buddyAce);
        }
    }
}