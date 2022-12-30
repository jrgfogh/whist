using System.Collections.Generic;
using System.Linq;

namespace Whist.Rules
{
    public abstract class TrickEvaluator
    {
        public int EvaluateTrick(IList<Card> cards)
        {
            if (cards[0].IsJoker)
                return 0;
            var result = 0;
            foreach (var (card, index) in cards.Select((card, index) => (card, index)))
                if (IsCandidateBetterThanCurrentBest(card, cards[result]))
                    result = index;
            return result;
        }

        protected abstract bool IsCandidateBetterThanCurrentBest(Card candidate, Card currentBest);
    }
}