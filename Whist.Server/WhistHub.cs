namespace Whist.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public sealed class WhistHub : Hub<IWhistClient>
    {
        private readonly GameConductorService _gameConductorService;

        // TODO(jrgfogh): Make the table name dynamic.
        private const string TableName = "Table";
        // TODO(jrgfogh): This only works, when there is only a single instance of WhistHub:
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
            ConnectionIdsAtTable[TableName].Remove(this.Context.ConnectionId);
            await this.Clients.Group(TableName).UpdatePlayersAtTable(ConnectionIdsAtTable[TableName]);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SelectTable(string table)
        {
            // TODO(jrgfogh): Validate the table name!
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, table);
            ConnectionIdsAtTable[table].Add(this.Context.ConnectionId);
            if (ConnectionIdsAtTable[table].Count == 4)
                this._gameConductorService.StartGame(ConnectionIdsAtTable[table]);
            await this.Clients.Group(table).UpdatePlayersAtTable(ConnectionIdsAtTable[table]);
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