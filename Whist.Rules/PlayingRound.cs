using System.Collections.Generic;

namespace Whist.Rules
{
    public sealed class PlayingRound(TrickEvaluator evaluator)
    {
        private readonly List<Card> _cardsInTrick = new();

        public int? Play(Card card)
        {
            _cardsInTrick.Add(card);
            if (_cardsInTrick.Count == 4)
                return WinnerTakesTrick();
            PlayerToPlay = (PlayerToPlay + 1) % 4;
            return null;
        }

        private int WinnerTakesTrick()
        {
            var winner = evaluator.EvaluateTrick(_cardsInTrick);
            _cardsInTrick.Clear();
            PlayerToPlay = winner;
            return winner;
        }

        public int PlayerToPlay { get; private set; }
    }
}
