using System.Threading.Tasks;

namespace Whist.Server
{
    public interface ILobbyClient
    {
        Task AnnouncePlayerName(int index, string name);
    }
}