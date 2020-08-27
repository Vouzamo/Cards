using System.Collections.Generic;
using System.Linq;
using Vouzamo.Cards.Core;

namespace Vouzamo.Cards.Poker
{
    public class PokerHandEvaluator : IHandEvaluator
    {
        public IHand Evaluate(IEnumerable<Card> cards)
        {
            if(TryGetRoyalFlush(cards, out IHand royalFlush))
            {
                return royalFlush;
            }

            if (TryGetStraightFlush(cards, out IHand straightFlush))
            {
                return straightFlush;
            }

            if (TryGetFourOfAKind(cards, out IHand fourOfAKind))
            {
                ApplyKickers(fourOfAKind, cards);

                return fourOfAKind;
            }

            if (TryGetFullHouse(cards, out IHand fullHouse))
            {
                return fullHouse;
            }

            if (TryGetFlush(cards, out IHand flush))
            {
                return flush;
            }

            if (TryGetStraight(cards, out IHand straight))
            {
                return straight;
            }

            if (TryGetThreeOfAKind(cards, out IHand threeOfAKind))
            {
                ApplyKickers(threeOfAKind, cards);

                return threeOfAKind;
            }

            if (TryGetTwoPair(cards, out IHand twoPair))
            {
                ApplyKickers(twoPair, cards);

                return twoPair;
            }

            if (TryGetPair(cards, out IHand pair))
            {
                ApplyKickers(pair, cards);

                return pair;
            }

            var highCard = GetHighCard(cards);

            ApplyKickers(highCard, cards);

            return highCard;
        }

        private static void ApplyKickers(IHand hand, IEnumerable<Card> cards)
        {
            var remainingCards = cards.Where(c => !hand.Cards.Contains(c));

            var kickers = remainingCards.SortAcesHigh().Take(5 - hand.Cards.Count());

            hand.Cards.AddRange(kickers);
        }

        private static bool TryGetRoyalFlush(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach(var suit in suited)
            {
                hand = new PokerHand(suit.SortAcesHigh().Take(5).ToList(), HandRankings.RoyalFlush);

                if (hand.Cards.Count == 5 && hand.Cards.First().Rank == Ranks.Ace && hand.Cards.Last().Rank == Ranks.Ten)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetStraightFlush(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach (var suit in suited)
            {
                var sorted = suit.SortAcesLow();

                if (sorted.TryGetStraight(out var straight))
                {
                    hand = new PokerHand(straight, HandRankings.StraightFlush);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFourOfAKind(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach(var rank in ranked)
            {
                if(rank.Count() >= 4)
                {
                    hand = new PokerHand(rank.Take(4).ToList(), HandRankings.FourOfAKind);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFullHouse(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            if(TryGetThreeOfAKind(cards, out var threeOfAKind))
            {
                var remainingCards = cards.Where(c => c.Rank != threeOfAKind.Cards.First().Rank);

                if(TryGetPair(remainingCards, out var pair))
                {
                    var fullHouse = threeOfAKind.Cards;
                    fullHouse.AddRange(pair.Cards);

                    hand = new PokerHand(fullHouse, HandRankings.FullHouse);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetFlush(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var suited = cards.GroupBy(c => c.Suit);

            foreach (var suit in suited)
            {
                if(suit.Count() >= 5)
                {
                    hand = new PokerHand(suit.SortAcesHigh().Take(5).ToList(), HandRankings.Flush);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetStraight(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var sorted = cards.SortAcesHigh();

            if(sorted.TryGetStraight(out var straight))
            {
                hand = new PokerHand(straight, HandRankings.Straight);
            }
            else
            {
                sorted = cards.SortAcesLow();

                if (sorted.TryGetStraight(out straight))
                {
                    hand = new PokerHand(straight, HandRankings.Straight);
                }
            }

            return hand != default;
        }

        private static bool TryGetThreeOfAKind(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach (var rank in ranked)
            {
                if (rank.Count() >= 3)
                {
                    hand = new PokerHand(rank.Take(3).ToList(), HandRankings.ThreeOfAKind);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetTwoPair(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            if (TryGetPair(cards, out var firstPair))
            {
                var remainingCards = cards.Where(c => c.Rank != firstPair.Cards.First().Rank);

                if (TryGetPair(remainingCards, out var secondPair))
                {
                    var twoPair = firstPair.Cards;
                    twoPair.AddRange(secondPair.Cards);

                    hand = new PokerHand(twoPair, HandRankings.TwoPair);

                    return true;
                }
            }

            return false;
        }

        private static bool TryGetPair(IEnumerable<Card> cards, out IHand hand)
        {
            hand = default;

            var ranked = cards.SortAcesHigh().GroupBy(c => c.Rank);

            foreach (var rank in ranked)
            {
                if (rank.Count() >= 2)
                {
                    hand = new PokerHand(rank.Take(2).ToList(), HandRankings.Pair);

                    return true;
                }
            }

            return false;
        }

        private static IHand GetHighCard(IEnumerable<Card> cards)
        {
            return new PokerHand(cards.SortAcesHigh().Take(5).ToList(), HandRankings.HighCard);
        }
    }
}
