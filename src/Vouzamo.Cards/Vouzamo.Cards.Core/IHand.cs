using System.Collections.Generic;

namespace Vouzamo.Cards.Core
{
    public interface IHand
    {
        int Score { get; }
        string Ranking { get; }
        List<Card> Cards { get; }
    }
}
