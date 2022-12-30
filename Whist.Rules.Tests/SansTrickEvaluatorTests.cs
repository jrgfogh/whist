using NUnit.Framework;

namespace Whist.Rules.Tests
{
    public sealed class SansTrickEvaluatorTests
    {
        [Test]
        [TestCase("S7", 0)]
        [TestCase("S2 S3 S4", 2)]
        [TestCase("S2 S3 S4 S5", 3)]
        [TestCase("S2 S3 S5 S4", 2)]
        [TestCase("S2 S5 S3 S4", 1)]
        [TestCase("S2 S5 S6 S4", 2)]
        [TestCase("S7 S5 S6 S4", 0)]
        [TestCase("S7 C9 S6 S4", 0)]
        [TestCase("Joker C9 S6 S4", 0)]
        [TestCase("S7 Joker S6 S4", 0)]
        [TestCase("S1 Joker S6 S4", 0)]
        public void EvaluateTrick(string cardsPlayed, int winnerIndex)
        {
            AbstractTrickEvaluatorTests.TestEvaluateTrick(new SansTrickEvaluator(), cardsPlayed, winnerIndex);
        }
    }
}