using System;
using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.Cards.Core
{

    public class Deck
    {
        static readonly Random Random = new Random();

        public int Count => Cards.Count;
        private Stack<Card> Cards { get; set; } = new Stack<Card>();

        public Deck()
        {
            for (int i = 52; i > 0; i--)
            {
                Cards.Push(new Card(i));
            }
        }

        public bool TryDeal(int count, out List<Card> cards)
        {
            cards = new List<Card>();

            if(count <= Count)
            {
                while (count > 0)
                {
                    cards.Add(Cards.Pop());
                    count -= 1;
                }

                return true;
            }

            return false;
        }

        public void Shuffle()
        {
            var cards = Cards.ToList();

            for (int n = Count - 1; n > 0; --n)
            {
                var k = Random.Next(n + 1);
                var temp = cards[n];
                cards[n] = cards[k];
                cards[k] = temp;
            }

            Cards = new Stack<Card>(cards);
        }
    }
}
