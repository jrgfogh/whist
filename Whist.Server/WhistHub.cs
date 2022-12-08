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

        // TODO(jrgfogh): Make the table name dynamic.
        private const string TableName = "Table";
        // TODO(jrgfogh): This only works, when there is only a single single server process:
        private static readonly Dictionary<string, List<string>> ConnectionIdsAtTable = new()
        {
            { TableName, new List<string>() }
        };
        private static readonly List<KeyAndText> TableNames = new()
        {
            new() { Key = TableName + "Key", Text = TableName }
        };

        // TODO(jrgfogh): Implement RemoveTable as well.
        public async Task CreateTable(string name)
        {
            // TODO(jrgfogh): Should this be one data structure?
            TableNames.Add(new KeyAndText() { Key = name + "Key", Text = name });
            ConnectionIdsAtTable.Add(name, new List<string>());
        }

        public WhistHub(GameConductorService gameConductorService)
        {
            this._gameConductorService = gameConductorService;
        }

        public override async Task OnConnectedAsync()
        {
            await this.Clients.All.UpdateListOfTables(TableNames);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, TableName);
            // TODO(jrgfogh): Only remove the player if the player is at the table:
            List<string> connectionIds;
            lock (syncLock)
            {
                ConnectionIdsAtTable[TableName].Remove(this.Context.ConnectionId);
                connectionIds = new (ConnectionIdsAtTable[TableName]);
            }
            await this.Clients.Group(TableName).UpdatePlayersAtTable(connectionIds);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SelectTable(string table)
        {
            // TODO(jrgfogh): Validate the table name!
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, table);
            List<string> connectionIds;
            lock (syncLock)
            {
                ConnectionIdsAtTable[table].Add(this.Context.ConnectionId);
                connectionIds = new (ConnectionIdsAtTable[table]);
            }
            if (connectionIds.Count == 4)
                this._gameConductorService.StartGame(connectionIds);
            // TODO(jrgfogh): Don't send a list, just send an event saying who joined.
            await this.Clients.Group(table).UpdatePlayersAtTable(connectionIds);
        }

        public async Task SaveTableName(string key, string text)
        {
            TableNames[0].Text = text;
            await this.Clients.All.UpdateListOfTables(TableNames);
        }

        public async Task SavePlayerName(string key, string text)
        {
        }

        public async Task SendBid(string user, string bid)
        {
            this._gameConductorService.ReceiveBid(bid);
            await this.Clients.Group(TableName).ReceiveBid(user, bid);
        }

        public async Task SendTrump(string trump)
        {
            this._gameConductorService.ReceiveTrump(trump);
            await this.Clients.Group(TableName).ReceiveTrump(trump);
        }

        public async Task SendBuddyAce(string buddyAce)
        {
            this._gameConductorService.ReceiveBuddyAce(buddyAce);
            await this.Clients.Group(TableName).ReceiveBuddyAce(buddyAce);
        }
    }
}