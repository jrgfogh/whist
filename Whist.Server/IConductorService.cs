namespace Whist.Server;

public interface IConductorService
{
    void ReceiveChoice(string choice);
    void JoinTable(string connectionId);
    void LeaveTable(string connectionId);
    string UserName(string connectionId);
}