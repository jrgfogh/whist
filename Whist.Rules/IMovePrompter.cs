using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whist.Rules
{
    public interface IMovePrompter
    {
        Task DealCards(int playerIndex, List<Card> cards);
        Task<string> PromptForBid(int playerToBid);
        Task<string> PromptForTrump(int winner);
        Task<string> PromptForBuddyAce(int winner);
        Task<string> PromptForCard(int playerToPlay);
        Task AnnounceWinner(string winner);
        Task AnnounceBiddingWinner(string winner, string winningBid);
        Task StartPlaying();
    }
}