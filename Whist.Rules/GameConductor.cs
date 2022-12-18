using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Whist.Rules
{
    // TODO(jrgfogh): Test this!
    public sealed class GameConductor
    {
        private readonly IMovePrompter _movePrompter;

        private static List<Card> _cat = new();

        public GameConductor(IMovePrompter movePrompter)
        {
            _movePrompter = movePrompter;
        }

        public async Task ConductGame()
        {
            await DealCards();
            var (winner, winningBid) = await ConductBiddingRound();
            await AnnounceWinner(winner, winningBid);
            var trump = await PromptForTrump(winner, winningBid);
            // TODO(jrgfogh): I haven't yet written the code, which will use these variables:
            _ = await PromptForBuddyAce(winner);
            // TODO(jrgfogh): Exchange cards.
            _ = new PlayingRound(CreateTrickEvaluator(winningBid, trump[0]));
        }

        private async Task<(int Winner, string WinningBid)> ConductBiddingRound()
        {
            // TODO(jrgfogh): Player 0 should not always start bidding.
            var round = new BiddingRound();
            while (!round.IsBiddingDone) round.Bid(await _movePrompter.PromptForBid(round.PlayerToBid));
            return (round.Winner, round.WinningBid!);
        }

        private async Task AnnounceWinner(int winner, string winningBid)
        {
            var playerNames = new[] { "Player A", "Player B", "Player C", "Player D" };
            await _movePrompter.AnnounceWinner(playerNames[winner], winningBid);
        }

        private async Task DealCards()
        {
            var deck = new Deck();
            foreach (var playerIndex in Enumerable.Range(0, 4))
                await _movePrompter.DealCards(playerIndex, deck.DealCards(13));
            _cat = deck.DealCards(3);
        }

        private async Task<string> PromptForTrump(int winner, string winningBid)
        {
            if (winningBid.EndsWith("common"))
                return await _movePrompter.PromptForTrump(winner);
            return "C";
        }

        private async Task<string> PromptForBuddyAce(int winner)
        {
            return await _movePrompter.PromptForBuddyAce(winner);
        }

        // TODO(jrgfogh): Move this factory method.
        private static TrickEvaluator CreateTrickEvaluator(string winningBid, char trump)
        {
            var bidKind = winningBid.Split(' ')[1];
            return bidKind switch
            {
                // ReSharper disable once PossibleInvalidOperationException
                "common" => new CommonTrickEvaluator(trump),
                "good" => new CommonTrickEvaluator(trump),
                "sans" => new SansTrickEvaluator(),
                "solo" => new SoloTrickEvaluator(),
                _ => throw new Exception($"Invalid bid: {winningBid}.")
            };
        }
    }
}