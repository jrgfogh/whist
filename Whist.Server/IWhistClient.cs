using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whist.Server
{
    public interface IWhistClient : ILobbyClient
    {
        Task PromptForBid();
        Task PromptForTrump();
        Task PromptForBuddyAce();
        Task PromptForCard();
        Task ReceiveDealtCards(IEnumerable<string> cards);
        Task ReceiveChoice(string chooser, string choice);
        Task AnnounceBiddingWinner(string winner, string winningBid);
        Task AnnounceWinner(string winner);
    }
}