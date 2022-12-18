
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whist.Server
{
    public interface ILobbyClient
    {
        Task UpdatePlayersAtTable(IEnumerable<string> players);
    }
}