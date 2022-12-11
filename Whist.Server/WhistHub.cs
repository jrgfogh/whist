namespace Whist.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public sealed class WhistHub : Hub<IWhistClient>
    {
        private readonly GameConductorService _gameConductorService;

        private readonly object syncLock = new object();

        // TODO(jrgfogh): This only works, when there is only a single single server process:
        private static readonly List<string> ConnectionIdsAtTable = new();

        public WhistHub(GameConductorService gameConductorService)
        {
            this._gameConductorService = gameConductorService;
        }

        public override async Task OnConnectedAsync()
        {
            List<string> connectionIds;
            lock (syncLock)
            {
                ConnectionIdsAtTable.Add(this.Context.ConnectionId);
                connectionIds = new (ConnectionIdsAtTable);
            }
            // TODO(jrgfogh): Don't send a list, just send an event saying who joined.
            await this.Clients.All.UpdatePlayersAtTable(connectionIds);

            if (connectionIds.Count == 4)
                this._gameConductorService.StartGame(connectionIds);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            List<string> connectionIds = null;
            lock (syncLock)
            {
                if (ConnectionIdsAtTable.Remove(this.Context.ConnectionId))
                    connectionIds = new (ConnectionIdsAtTable);
            }
            if (connectionIds != null)
                await this.Clients.All.UpdatePlayersAtTable(connectionIds);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendBid(string bid)
        {
            // TODO(jrgfogh): Get this from somewhere else:
            var user = "Player A";
            this._gameConductorService.ReceiveBid(bid);
            await this.Clients.All.ReceiveBid(user, bid);
        }

        public async Task SendTrump(string trump)
        {
            this._gameConductorService.ReceiveTrump(trump);
            await this.Clients.All.ReceiveTrump(trump);
        }

        public async Task SendBuddyAce(string buddyAce)
        {
            this._gameConductorService.ReceiveBuddyAce(buddyAce);
            await this.Clients.All.ReceiveBuddyAce(buddyAce);
        }
    }
}