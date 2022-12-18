using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Whist.Server
{
    public sealed class GameConductorServiceWrapper : IHostedService
    {
        private readonly GameConductorService _gameConductorService;

        public GameConductorServiceWrapper(GameConductorService gameConductorService)
        {
            _gameConductorService = gameConductorService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _gameConductorService.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _gameConductorService.StopAsync(cancellationToken);
        }
    }
}