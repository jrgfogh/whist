using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Whist.Server
{
    public sealed class WhistHub(IConductorService conductorService) : Hub<IWhistClient>
    {
        public Task SendChoice(string choice)
        {
            return conductorService.ReceiveChoice(Context.ConnectionId, choice);
        }
    }
}