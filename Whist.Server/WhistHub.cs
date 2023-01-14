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

        public Task SendChoice(string choice)
        {
            return _conductorService.ReceiveChoice(Context.ConnectionId, choice);
        }
    }
}