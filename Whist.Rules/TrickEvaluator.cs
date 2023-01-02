using System.Collections.Generic;
using System.Linq;

namespace Whist.Rules
{
    public abstract class TrickEvaluator
    {
        public int EvaluateTrick(IEnumerable<(Card, int)> cards)
        {
            if (cards.First().Item1.IsJoker)
                return 0;
            var (currentBest, result) = cards.First();
            foreach (var (card, index) in cards)
                if (IsCandidateBetterThanCurrentBest(card, currentBest))
                    (currentBest, result) = (card, index);
            return result;
        }

        protected abstract bool IsCandidateBetterThanCurrentBest(Card candidate, Card currentBest);
    }
}