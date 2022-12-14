namespace Whist.Rules
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMovePrompter
    {
        Task DealCards(int playerIndex, List<Card> cards);
        Task<string> PromptForBid(int playerToBid);
        Task<string> PromptForTrump(int winner);
        Task<string> PromptForBuddyAce(int winner);
        Task AnnounceWinner(string winner, string winningBid);
    }
}