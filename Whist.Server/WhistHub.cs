using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Whist.Server
{
    public sealed class WhistHub : Hub<IWhistClient>
    {
        private readonly IConductorService _conductorService;

        public WhistHub(IConductorService conductorService)
        {
            _conductorService = conductorService;
        }

        public override async Task OnConnectedAsync()
        {
            _conductorService.JoinTable(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _conductorService.LeaveTable(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendChoice(string choice)
        {
            await Clients.All.ReceiveChoice(UserNameOfCaller(), choice);
            _conductorService.ReceiveChoice(choice);
        }

        private string UserNameOfCaller()
        {
            return _conductorService.UserName(Context.ConnectionId);
        }
    }
}