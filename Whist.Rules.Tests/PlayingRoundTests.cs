using NUnit.Framework;
using System.Linq;

namespace Whist.Rules.Tests
{
    public sealed class PlayingRoundTests
    {
        [Test]
        public void Player0PlaysFirst()
        {
            var evaluator = new SansTrickEvaluator();
            var round = new PlayingRound(evaluator);
            Assert.That(round.PlayerToPlay, Is.EqualTo(0));
        }

        [Test]
        [TestCase("H3", 1, null)]
        [TestCase("H3\n" +
            "H4", 2, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5", 3, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H2", 2, 2)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H7", 3, 3)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H7\n" +
                  "C10", 0, null)]
        public void Play(string cardNames, int playerToPlay, int? trickTaker)
        {
            var evaluator = new SansTrickEvaluator();
            var round = new PlayingRound(evaluator);
            int? lastTrickTaker = null;
            foreach (var card in cardNames.Split('\n').Select(name => new Card(name)))
                lastTrickTaker = round.Play(card);
            Assert.That(lastTrickTaker, Is.EqualTo(trickTaker));
            Assert.That(round.PlayerToPlay, Is.EqualTo(playerToPlay));
        }
    }
}
