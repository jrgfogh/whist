using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System;

namespace Whist.Rules.Tests
{
    internal static class AbstractTrickEvaluatorTests
    {
        public static void TestEvaluateTrick(TrickEvaluator trickEvaluator, string cardsPlayed, int winnerIndex)
        {
            var cards = cardsPlayed.Split(' ')
                .Select((cardName, index) => (Card.CreateInstance(cardName)!, index)).ToArray();
            Assert.That(trickEvaluator.EvaluateTrick(cards), Is.EqualTo(winnerIndex));
        }
    }
}