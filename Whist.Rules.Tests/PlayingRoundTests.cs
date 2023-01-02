using System;
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
                  "pass\n" +
                  "pass\n" +
                  "pass", 3, 0)]
        [TestCase("pass\n" +
                  "pass\n" +
                  "pass\n" +
                  "H3", 3, 3)]
        [TestCase("H3\n" +
            "H4", 2, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "pass", 0, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H2", 0, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H7", 0, null)]
        [TestCase("H3\n" +
                  "H4\n" +
                  "H5\n" +
                  "H7\n" +
                  "C10", 1, null)]
        public void Play(string cardNames, int playerToPlay, int? trickTaker)
        {
            var round = PlayRound(cardNames, out var lastTrickTaker);
            Assert.That(lastTrickTaker, Is.EqualTo(trickTaker));
            Assert.That(round.PlayerToPlay, Is.EqualTo(playerToPlay));
        }

        private static PlayingRound PlayRound(string cardNames, out int? lastTrickTaker)
        {
            var evaluator = new SansTrickEvaluator();
            var round = new PlayingRound(evaluator);
            lastTrickTaker = null;
            foreach (var card in cardNames.Split('\n').Select(Card.CreateInstance))
                lastTrickTaker = round.Play(card);
            return round;
        }
        
        [Test]
        [TestCase("pass\n" +
                  "pass\n" +
                  "pass\n" +
                  "pass")]
        [TestCase("H3\n" +
                  "pass\n" +
                  "pass\n" +
                  "pass\n" +
                  "pass")]
        public void PlayingMustStop(string cardNames)
        {
            var evaluator = new SansTrickEvaluator();
            var round = new PlayingRound(evaluator);
            var cards = cardNames.Split('\n').Select(Card.CreateInstance).ToArray();
            foreach (var card in cards.Take(cards.Length - 1))
                round.Play(card);
            Assert.Throws(typeof(InvalidOperationException), () => round.Play(cards.Last()));
        }
    }
}
