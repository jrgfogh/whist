﻿namespace Whist.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // TODO(jorgen.fogh): Test this!
    public sealed class GameConductor
    {
        private readonly IMovePrompter _movePrompter;

        private static List<Card> _cat;

        public GameConductor(IMovePrompter movePrompter)
        {
            this._movePrompter = movePrompter;
        }

        public async Task ConductGame()
        {
            await this.DealCards();
            var (winner, winningBid) = await this.ConductBiddingRound();
            // TODO(jorgen.fogh): Announce the winner?
            var trump = await this.PromptForTrump(winner, winningBid);
            // TODO(jorgen.fogh): I haven't yet written the code, which will use these variables:
            var ace = await this.PromptForBuddyAce(winner); // lgtm [cs/useless-assignment-to-local]
            // TODO(jorgen.fogh): Exchange cards.
            var round = new PlayingRound(CreateTrickEvaluator(winningBid, trump[0])); // lgtm [cs/useless-assignment-to-local]
        }

        private async Task<(int Winner, string WinningBid)> ConductBiddingRound()
        {
            // TODO(jorgen.fogh): Player 0 should not always start bidding.
            var round = new BiddingRound();
            while (!round.IsBiddingDone) round.Bid(await this._movePrompter.PromptForBid(round.PlayerToBid));
            return (round.Winner, round.WinningBid);
        }

        private async Task DealCards()
        {
            var deck = new Deck();
            foreach (var playerIndex in Enumerable.Range(0, 4))
                await this._movePrompter.DealCards(playerIndex, deck.DealCards(13));
            _cat = deck.DealCards(3);
        }

        private async Task<string> PromptForTrump(int winner, string winningBid)
        {
            if (winningBid.EndsWith("common"))
                return await this._movePrompter.PromptForTrump(winner);
            return "C";
        }

        private async Task<string> PromptForBuddyAce(int winner)
        {
            return await this._movePrompter.PromptForBuddyAce(winner);
        }

        // TODO(jorgen.fogh): Move this factory method.
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