using NUnit.Framework;

namespace Whist.Rules.Tests
{
    public sealed class CardTests
    {
        [Test]
        [TestCase("S5")]
        [TestCase("C2")]
        public void EqualityOperatorsIdenticalCards(string cardName)
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.That(Card.CreateInstance(cardName) == Card.CreateInstance(cardName));
            // ReSharper disable once EqualExpressionComparison
            Assert.That(Equals(Card.CreateInstance(cardName), Card.CreateInstance(cardName)));
            // ReSharper disable once EqualExpressionComparison
            Assert.That(Card.CreateInstance(cardName) != Card.CreateInstance(cardName), Is.False);
        }

        [Test]
        [TestCase("S5", "C2")]
        [TestCase("H7", "D3")]
        [TestCase("H7", "pass")]
        [TestCase("pass", "D3")]
        public void EqualityOperatorsDifferentCards(string left, string right)
        {
            Assert.That(Card.CreateInstance(left) == Card.CreateInstance(right), Is.False);
            Assert.That(Equals(Card.CreateInstance(left), Card.CreateInstance(right)), Is.False);
            Assert.That(Card.CreateInstance(left) != Card.CreateInstance(right));
        }
    }
}