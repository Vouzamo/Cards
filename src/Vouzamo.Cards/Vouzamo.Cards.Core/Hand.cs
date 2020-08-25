using System.Collections.Generic;

namespace Vouzamo.Cards.Core
{
    public class Hand
    {
        public int Score => CalculateScore();

        public List<Card> Cards { get; set; }
        public HandRankings Ranking { get; set; }

        public Hand(List<Card> cards, HandRankings ranking)
        {
            Cards = cards;
            Ranking = ranking;
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

            score += (int)Ranking << 20;

            score += (int)Cards[0].Rank << 16;
            score += (int)Cards[1].Rank << 12;
            score += (int)Cards[2].Rank << 8;
            score += (int)Cards[3].Rank << 4;
            score += (int)Cards[4].Rank;

            return score;
        }
    }
}
