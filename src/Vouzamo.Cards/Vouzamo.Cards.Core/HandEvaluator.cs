using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.Cards.Core
{
    public static class HandEvaluator
    {
        public static Hand Evaluate(IEnumerable<Card> cards)
        {
            if(TryGetRoyalFlush(cards, out Hand royalFlush))
            {
                return royalFlush;
            }

            if (TryGetStraightFlush(cards, out Hand straightFlush))
            {
                return straightFlush;
            }

            if (TryGetFourOfAKind(cards, out Hand fourOfAKind))
            {
                ApplyKickers(fourOfAKind, cards);

                return fourOfAKind;
            }

            if (TryGetFullHouse(cards, out Hand fullHouse))
            {
                return fullHouse;
            }

            if (TryGetFlush(cards, out Hand flush))
            {
                return flush;
            }

            if (TryGetStraight(cards, out Hand straight))
            {
                return straight;
            }

            if (TryGetThreeOfAKind(cards, out Hand threeOfAKind))
            {
                ApplyKickers(threeOfAKind, cards);

                return threeOfAKind;
            }

            if (TryGetTwoPair(cards, out Hand twoPair))
            {
                ApplyKickers(twoPair, cards);

                return twoPair;
            }

            if (TryGetPair(cards, out Hand pair))
            {
                ApplyKickers(pair, cards);

                return pair;
            }

            var highCard = GetHighCard(cards);

            ApplyKickers(highCard, cards);

            return highCard;
        }

        private static void ApplyKickers(Hand hand, IEnumerable<Card> cards)
        {
            var remainingCards = cards.Where(c => !hand.Cards.Contains(c));

            var kickers = remainingCards.SortAcesHigh().Take(5 - hand.Cards.Count());

            // Modify hand.Score based on kickers

            hand.Cards.AddRange(kickers);
        }

        private static bool TryGetRoyalFlush(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach(var suit in suited)
            {
                hand = new Hand(suit.SortAcesHigh().Take(5).ToList(), HandRankings.RoyalFlush);

                if (hand.Cards.Count == 5 && hand.Cards.First().Rank == Ranks.Ace && hand.Cards.Last().Rank == Ranks.Ten)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetStraightFlush(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach (var suit in suited)
            {
                var sorted = suit.SortAcesLow();

                if (sorted.TryGetStraight(out var straight))
                {
                    hand = new Hand(straight, HandRankings.StraightFlush);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFourOfAKind(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach(var rank in ranked)
            {
                if(rank.Count() >= 4)
                {
                    hand = new Hand(rank.Take(4).ToList(), HandRankings.FourOfAKind);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFullHouse(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            if(TryGetThreeOfAKind(cards, out var threeOfAKind))
            {
                var remainingCards = cards.Where(c => c.Rank != threeOfAKind.Cards.First().Rank);

                if(TryGetPair(remainingCards, out var pair))
                {
                    var fullHouse = threeOfAKind.Cards;
                    fullHouse.AddRange(pair.Cards);

                    hand = new Hand(fullHouse, HandRankings.FullHouse);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFlush(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach (var suit in suited)
            {
                if(suit.Count() >= 5)
                {
                    hand = new Hand(suit.SortAcesHigh().Take(5).ToList(), HandRankings.Flush);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetStraight(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var sorted = cards.SortAcesHigh();

            if(sorted.TryGetStraight(out var straight))
            {
                hand = new Hand(straight, HandRankings.Straight);
            }
            else
            {
                sorted = cards.SortAcesLow();

                if (sorted.TryGetStraight(out straight))
                {
                    hand = new Hand(straight, HandRankings.Straight);
                }
            }

            return hand != default;
        }

        private static bool TryGetThreeOfAKind(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach (var rank in ranked)
            {
                if (rank.Count() >= 3)
                {
                    hand = new Hand(rank.Take(3).ToList(), HandRankings.ThreeOfAKind);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetTwoPair(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            if (TryGetPair(cards, out var firstPair))
            {
                var remainingCards = cards.Where(c => c.Rank != firstPair.Cards.First().Rank);

                if (TryGetPair(remainingCards, out var secondPair))
                {
                    var twoPair = firstPair.Cards;
                    twoPair.AddRange(secondPair.Cards);

                    hand = new Hand(twoPair, HandRankings.TwoPair);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetPair(IEnumerable<Card> cards, out Hand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach (var rank in ranked)
            {
                if (rank.Count() >= 2)
                {
                    hand = new Hand(rank.Take(2).ToList(), HandRankings.Pair);

                    return true;
                }
            }

            return false;
        }

        private static Hand GetHighCard(IEnumerable<Card> cards)
        {
            return new Hand(cards.SortAcesHigh().Take(5).ToList(), HandRankings.HighCard);
        }
    }
}
