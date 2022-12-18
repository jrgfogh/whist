using System.Collections.Generic;

namespace Whist.Rules
{
    public sealed class Player
    {
        public readonly List<Card> Hand;

        public Player(List<Card> hand)
        {
            Hand = hand;
        }
    }
}
