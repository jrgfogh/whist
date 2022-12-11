
namespace Whist.Server
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILobbyClient
    {
        Task UpdatePlayersAtTable(IEnumerable<string> players);
    }
}