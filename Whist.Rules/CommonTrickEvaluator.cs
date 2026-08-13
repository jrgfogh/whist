namespace Whist.Rules
{
    public sealed class CommonTrickEvaluator(char trump) : TrickEvaluator
    {
        protected override bool IsCandidateBetterThanCurrentBest(Card candidate, Card currentBest)
        {
            if (candidate.Suit == trump && currentBest.Suit != trump)
                return true;
            return !candidate.IsJoker &&
                   candidate.FaceValue > currentBest.FaceValue &&
                   candidate.Suit == currentBest.Suit;
        }
    }
}