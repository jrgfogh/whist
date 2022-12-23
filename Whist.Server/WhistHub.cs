using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Whist.Server
{
    public sealed class WhistHub : Hub<IWhistClient>
    {
        private readonly GameConductorService _gameConductorService;

        public WhistHub(GameConductorService gameConductorService)
        {
            _gameConductorService = gameConductorService;
        }

        public override async Task OnConnectedAsync()
        {
            _gameConductorService.JoinTable(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _gameConductorService.LeaveTable(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendBid(string bid)
        {
            var userNameOfCaller = _gameConductorService.UserName(Context.ConnectionId);
            await Clients.All.ReceiveBid(userNameOfCaller, bid);
            _gameConductorService.ReceiveChoice(bid);
        }

        public async Task SendChoice(string choice)
        {
            await Clients.All.ReceiveChoice(choice);
            _gameConductorService.ReceiveChoice(choice);
        }
    }
}