using System.Collections.Generic;

namespace Vouzamo.Cards.Core
{
    public interface IHandEvaluator
    {
        IHand Evaluate(IEnumerable<Card> cards);
    }
}
