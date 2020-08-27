using System.Collections.Generic;
using Vouzamo.Cards.Core;

namespace Vouzamo.Cards.Poker
{

    public class PokerHand : IHand
    {
        public int Score => CalculateScore();
        public string Ranking => HandRanking.ToString();

        public List<Card> Cards { get; set; }
        public HandRankings HandRanking { get; set; }

        public PokerHand(List<Card> cards, HandRankings handRanking)
        {
            Cards = cards;
            HandRanking = handRanking;
        }

        /// <summary>
        /// Score is an unsigned 24 bit number with each 4 bit sequence representing hand ranking, and then each card rank sorted ace descending respectively
        /// For example:
        /// Full House (Aces over Eights)
        /// 0110 0001 0001 0001 1000 1000 (6361480)
        /// Flush (8 High)
        /// 0101 1000 0110 0101 0100 0010 (5793090)
        /// </summary>
        /// <returns>Int32 representation of 24 bit unsigned score</returns>
        private int CalculateScore()
        {
            var score = 0;

            score += (int)HandRanking << 20;

            score += (int)Cards[0].Rank << 16;
            score += (int)Cards[1].Rank << 12;
            score += (int)Cards[2].Rank << 8;
            score += (int)Cards[3].Rank << 4;
            score += (int)Cards[4].Rank;

            return score;
        }
    }
}
