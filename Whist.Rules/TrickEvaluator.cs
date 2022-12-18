using System.Collections.Generic;

namespace Whist.Rules
{
    public abstract class TrickEvaluator
    {
        public int EvaluateTrick(IList<Card> cards)
        {
            if (cards[0].IsJoker)
                return 0;
            var result = 0;
            for (var i = 1; i < 4; i++)
                if (IsCandidateBetterThanCurrentBest(cards[i], cards[result]))
                    result = i;
            return result;
        }

        protected abstract bool IsCandidateBetterThanCurrentBest(Card candidate, Card currentBest);
    }
}