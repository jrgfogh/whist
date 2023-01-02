using System;

namespace Whist.Rules
{
    public sealed class Card
    {
        private readonly string _name;

        private const string Joker = "Joker";

        public char Suit => _name[0];

        public bool IsJoker => _name == Joker;

        public int FaceValue =>
            _name[1] switch
            {
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                _ => int.Parse(_name[1..])
            };

        private Card(string name) => _name = name;

        public static Card? CreateInstance(string name)
        {
            return name == "pass" ? null : new Card(name);
        }

        private bool Equals(Card other)
        {
            return _name == other._name;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is Card other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(Card? left, Card? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Card? left, Card? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
