using System;
using System.Collections.Generic;
using System.Linq;

namespace Whist.Rules
{
    public sealed class Deck
    {
        private readonly List<Card> _cards = CreateCards();

        public Deck()
        {
            Shuffle();
        }

        private static List<Card> CreateCards()
        {
            var result = new List<Card>();
            foreach (var suit in new[] { "S", "H", "C", "D" })
            {
                result.AddRange(Enumerable.Range(1, 10).Select(number =>
                    new Card(suit + number)));
                result.AddRange(new[] { "J", "Q", "K" }.Select(rank =>
                    new Card(suit + rank)));
            }
            result.AddRange(new[] { "Joker", "Joker", "Joker" }.Select(cardName =>
                new Card(cardName)));

            return result;
        }

        private void Shuffle()
        {
            var generator = new Random();
            foreach (var index in Enumerable.Range(0, _cards.Count))
            {
                var swapIndex = generator.Next(index, _cards.Count);
                var tmp = _cards[index];
                _cards[index] = _cards[swapIndex];
                _cards[swapIndex] = tmp;
            }
        }

        public List<Card> DealCards(int count)
        {
            var result = _cards.GetRange(0, count);
            _cards.RemoveRange(0, count);
            return result;
        }
    }
}
