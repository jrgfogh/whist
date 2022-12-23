using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whist.Server
{
    public interface IWhistClient : ILobbyClient
    {
        Task PromptForBid();
        Task PromptForTrump();
        Task PromptForBuddyAce();
        Task ReceiveDealtCards(IEnumerable<string> cards);
        Task ReceiveChoice(string chooser, string choice);
        Task AnnounceWinner(string winner, string winningBid);
    }
}