using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <remarks>
    /// Greedily consumes values from the input sequence which match its sub-pattern.<br/>
    /// This means it will prioritize matching as long a sequence as possible,
    /// only returning values when no match could be found otherwise.
    /// </remarks>
    /// <inheritdoc/>
    public class GreedyQuantifier<T> : Quantifier<T>
    {
        public GreedyQuantifier(Generex<T> atom, int exactly) : base(atom, exactly)
        { }

        public GreedyQuantifier(Generex<T> atom, int minimum, int maximum) : base(atom, minimum, maximum)
        { }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            foreach (var quantityMatch in MatchQuantity(currentMatch))
                yield return quantityMatch;

            if (Minimum == 0)
                yield return currentMatch;
        }

        private IEnumerable<MatchState<T>> MatchQuantity(MatchState<T> currentMatch, int progress = 1, bool tryWithoutNext = true)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                // Only recurse when it's not at the max number yet
                if (progress < Maximum && (tryWithoutNext || !nextMatch.IsInputEnd))
                {
                    foreach (var futureMatch in MatchQuantity(nextMatch, progress + 1, !nextMatch.IsInputEnd))
                        yield return futureMatch;
                }

                // Greedy order: shorter matches last
                if (progress >= Minimum)
                    yield return nextMatch;
            }
        }
    }
}