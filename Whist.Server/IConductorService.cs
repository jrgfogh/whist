using System.Threading.Tasks;

namespace Whist.Server;

public interface IConductorService
{
    void ReceiveChoice(string choice);
    void JoinTable(string connectionId);
    Task LeaveTable(string connectionId);
    string UserName(string connectionId);
}