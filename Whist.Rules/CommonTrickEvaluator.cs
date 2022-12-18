namespace Whist.Rules
{
    public sealed class CommonTrickEvaluator : TrickEvaluator
    {
        private readonly char _trump;

        public CommonTrickEvaluator(char trump) => _trump = trump;

        protected override bool IsCandidateBetterThanCurrentBest(Card candidate, Card currentBest)
        {
            if (candidate.Suit == _trump && currentBest.Suit != _trump)
                return true;
            return !candidate.IsJoker &&
                   candidate.FaceValue > currentBest.FaceValue &&
                   candidate.Suit == currentBest.Suit;
        }
    }
}