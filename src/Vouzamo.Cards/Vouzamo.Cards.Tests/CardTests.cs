using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vouzamo.Cards.Core;

namespace Vouzamo.Cards.Tests
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Sorting()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Jack, Suits.Spades),
                new Card(Ranks.Seven, Suits.Diamonds),
                new Card(Ranks.Ace, Suits.Spades),
                new Card(Ranks.Eight, Suits.Clubs),
                new Card(Ranks.Ace, Suits.Hearts),
                new Card(Ranks.Seven, Suits.Clubs)
            };

            var acesHigh = cards.SortAcesHigh();

            Assert.AreEqual(Ranks.Ace, acesHigh[1].Rank);

            var acesLow = cards.SortAcesLow();

            Assert.AreEqual(Ranks.Ace, acesLow[4].Rank);
        }

        [TestMethod]
        public void RoyalFlush()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Ace, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Six, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.RoyalFlush, hand.Ranking);
        }

        [TestMethod]
        public void StraightFlush()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Nine, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Six, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.StraightFlush, hand.Ranking);
        }

        [TestMethod]
        public void FourOfAKind()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Jack, Suits.Spades),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.FourOfAKind, hand.Ranking);
        }

        [TestMethod]
        public void FullHouse()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Jack, Suits.Spades),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Ten, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.FullHouse, hand.Ranking);
        }

        [TestMethod]
        public void Flush()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Six, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Six, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.Flush, hand.Ranking);
        }

        [TestMethod]
        public void Straight()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Nine, Suits.Hearts),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Diamonds),
                new Card(Ranks.Jack, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Six, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.Straight, hand.Ranking);
        }

        [TestMethod]
        public void ThreeOfAKind()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Jack, Suits.Spades),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.Two, Suits.Hearts),
                new Card(Ranks.Queen, Suits.Hearts),
                new Card(Ranks.Seven, Suits.Clubs),
                new Card(Ranks.Jack, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.ThreeOfAKind, hand.Ranking);
            Assert.AreEqual(Ranks.Ten, hand.Cards.Last().Rank);
        }

        [TestMethod]
        public void TwoPair()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Jack, Suits.Spades),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Ten, Suits.Hearts),
                new Card(Ranks.King, Suits.Hearts),
                new Card(Ranks.Seven, Suits.Hearts),
                new Card(Ranks.Two, Suits.Clubs),
                new Card(Ranks.King, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.TwoPair, hand.Ranking);
            Assert.AreEqual(Ranks.Ten, hand.Cards.Last().Rank);
        }

        [TestMethod]
        public void Pair()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Two, Suits.Spades),
                new Card(Ranks.Five, Suits.Diamonds),
                new Card(Ranks.Ace, Suits.Hearts),
                new Card(Ranks.Eight, Suits.Hearts),
                new Card(Ranks.King, Suits.Hearts),
                new Card(Ranks.Eight, Suits.Clubs),
                new Card(Ranks.Six, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.Pair, hand.Ranking);
            Assert.AreEqual(Ranks.Six, hand.Cards.Last().Rank);
        }

        [TestMethod]
        public void HighCard()
        {
            var cards = new List<Card>
            {
                new Card(Ranks.Queen, Suits.Spades),
                new Card(Ranks.Jack, Suits.Diamonds),
                new Card(Ranks.Nine, Suits.Hearts),
                new Card(Ranks.Seven, Suits.Hearts),
                new Card(Ranks.Six, Suits.Hearts),
                new Card(Ranks.Three, Suits.Clubs),
                new Card(Ranks.Two, Suits.Hearts)
            };

            var hand = HandEvaluator.Evaluate(cards);

            Assert.AreEqual(HandRankings.HighCard, hand.Ranking);
            Assert.AreEqual(Ranks.Six, hand.Cards.Last().Rank);
        }

        [TestMethod]
        public void ValueTests()
        {
            var deck = new Deck();

            deck.Shuffle();

            if(deck.TryDeal(2, out var player1) && deck.TryDeal(2, out var player2))
            {
                if(deck.TryDeal(3, out var flop) && deck.TryDeal(1, out var turn) && deck.TryDeal(1, out var river))
                {
                    var player1hand = new List<Card>();
                    player1hand.AddRange(player1);
                    player1hand.AddRange(flop);
                    player1hand.AddRange(turn);
                    player1hand.AddRange(river);

                    var player2hand = new List<Card>();
                    player2hand.AddRange(player2);
                    player2hand.AddRange(flop);
                    player2hand.AddRange(turn);
                    player2hand.AddRange(river);

                    Assert.AreEqual(7, player1hand.Count);
                    Assert.AreEqual(7, player2hand.Count);
                }
            }

            while(deck.TryDeal(5, out var hand))
            {
                foreach(var card in hand)
                {
                    Debug.WriteLine(card);
                }
            }

            Assert.AreEqual(2, deck.Count);
        }
    }
}
