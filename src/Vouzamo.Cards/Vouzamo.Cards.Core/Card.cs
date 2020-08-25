namespace Vouzamo.Cards.Core
{
    public class Card
    {
        public int Value { get; }
        public Ranks Rank => (Ranks)(((Value - 1) % 13) + 1);
        public Suits Suit => (Suits)((Value - 1) / 13);

        public Card(int value)
        {
            Value = value;
        }

        public Card(Ranks rank, Suits suit) : this((13 * (int)suit) + (int)rank)
        {

        }

        public override string ToString()
        {
            var suit = string.Empty;

            switch(Suit)
            {
                case Suits.Spades:
                    suit = "♠";
                    break;
                case Suits.Hearts:
                    suit = "♥";
                    break;
                case Suits.Clubs:
                    suit = "♣";
                    break;
                case Suits.Diamonds:
                    suit = "♦";
                    break;
            }

            switch(Rank)
            {
                case Ranks.Ace:
                    return $"{suit}A";
                case Ranks.Jack:
                    return $"{suit}J";
                case Ranks.Queen:
                    return $"{suit}Q";
                case Ranks.King:
                    return $"{suit}K";
                case Ranks.Ten:
                    return $"{suit}T";
                default:
                    return $"{suit}{(int)Rank}";
            }
        }
    }
}
