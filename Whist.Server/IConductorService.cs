using System.Threading.Tasks;

namespace Whist.Server;

public interface IConductorService
{
    Task ReceiveChoice(string connectionId, string choice);
    void JoinTable(string connectionId);
    Task LeaveTable(string connectionId);
}