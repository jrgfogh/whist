using System;
using System.Collections.Generic;
using System.Linq;

namespace Whist.Rules
{
    public sealed class PlayingRound
    {
        private readonly TrickEvaluator _evaluator;
        private readonly List<(Card, int)> _cardsInTrick = new();
        private readonly bool[] _isStillIn = {true, true, true, true};

        public PlayingRound(TrickEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        public int? Play(Card? card)
        {
            if (!_isStillIn[PlayerToPlay])
                throw new InvalidOperationException("A card was played after the round ended.");
            if (card == null)
                _isStillIn[PlayerToPlay] = false;
            else
                _cardsInTrick.Add((card, PlayerToPlay));
            var playersRemaining = _isStillIn.Count(isIn => isIn);
            if (playersRemaining == 0)
                throw new InvalidOperationException("A card was played after the round ended.");
            if (playersRemaining == 1 && _cardsInTrick.Count > 0)
                return WinnerTakesTrick();
            do
            {
                PlayerToPlay = (PlayerToPlay + 1) % 4;
            } while (!_isStillIn[PlayerToPlay]);
            return null;
        }

        private int WinnerTakesTrick()
        {
            var winner = _evaluator.EvaluateTrick(_cardsInTrick);
            _cardsInTrick.Clear();
            return winner;
        }

        public int PlayerToPlay { get; private set; }
    }
}
