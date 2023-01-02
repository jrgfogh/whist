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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _gameConductorService.LeaveTable(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendChoice(string choice)
        {
            await Clients.All.ReceiveChoice(UserNameOfCaller(), choice);
            _gameConductorService.ReceiveChoice(choice);
        }

        private string UserNameOfCaller()
        {
            return _gameConductorService.UserName(Context.ConnectionId);
        }
    }
}