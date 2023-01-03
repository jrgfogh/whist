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
            Assert.That(new Card(cardName) == new Card(cardName));
            // ReSharper disable once EqualExpressionComparison
            Assert.That(Equals(new Card(cardName), new Card(cardName)));
            // ReSharper disable once EqualExpressionComparison
            Assert.That(new Card(cardName) != new Card(cardName), Is.False);
        }

        [Test]
        [TestCase("S5", "C2")]
        [TestCase("H7", "D3")]
        public void EqualityOperatorsDifferentCards(string left, string right)
        {
            Assert.That(new Card(left) == new Card(right), Is.False);
            Assert.That(Equals(new Card(left), new Card(right)), Is.False);
            Assert.That(new Card(left) != new Card(right));
        }
    }
}