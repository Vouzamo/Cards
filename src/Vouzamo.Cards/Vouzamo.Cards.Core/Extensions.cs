using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.Cards.Core
{
    public static class Extensions
    {

        public static List<Card> SortAcesHigh(this IEnumerable<Card> cards)
        {
            return cards.OrderByDescending(c => c.Rank == Ranks.Ace).ThenByDescending(c => c.Rank).ToList();
        }

        public static List<Card> SortAcesLow(this IEnumerable<Card> cards)
        {
            return cards.OrderBy(c => c.Rank == Ranks.Ace).ThenByDescending(c => c.Rank).ToList();
        }

        public static bool TryGetStraight(this IEnumerable<Card> cards, out List<Card> straight, int count = 5)
        {
            straight = new List<Card>();

            foreach (var card in cards)
            {
                if (!straight.Any())
                {
                    straight.Add(card);
                }
                else if (straight.Last().Rank == card.Rank)
                {
                    // Skip this card
                }
                else if (straight.Last().Rank == card.Rank + 1)
                {
                    straight.Add(card);

                    if (straight.Count() == count)
                    {
                        break;
                    }
                }
                else
                {
                    straight.Clear();
                    straight.Add(card);
                }
            }

            return straight.Count() == count;
        }
    }
}
