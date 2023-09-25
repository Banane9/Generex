using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <remarks>
    /// Lazily consumes values from the input sequence which match its sub-pattern.<br/>
    /// This means it will prioritize matching as short a sequence as possible,
    /// only consuming more values when no match could be found otherwise.
    /// </remarks>
    /// <inheritdoc/>
    public class LazyQuantifier<T> : Quantifier<T>
    {
        public LazyQuantifier(Generex<T> atom, int exactly) : base(atom, exactly)
        { }

        public LazyQuantifier(Generex<T> atom, int minimum, int maximum) : base(atom, minimum, maximum)
        { }

        /// <inheritdoc/>
        public override string ToString() => Minimum == Maximum ? base.ToString() : $"{base.ToString()}?";

        protected override IEnumerable<MatchState<T>> continueMatchInternal(MatchState<T> currentMatch)
        {
            if (Minimum == 0)
                yield return currentMatch;

            foreach (var quantityMatch in matchQuantity(currentMatch))
                yield return quantityMatch;
        }

        private IEnumerable<MatchState<T>> matchQuantity(MatchState<T> currentMatch, int progress = 1, bool tryWithoutNext = true)
        {
            foreach (var nextMatch in continueMatch(Atom, currentMatch))
            {
                // Lazy order: shorter matches first
                if (progress >= Minimum)
                    yield return nextMatch;

                // Only recurse when it's not at the max number yet
                if (progress < Maximum && (tryWithoutNext || !nextMatch.IsInputEnd))
                {
                    foreach (var futureMatch in matchQuantity(nextMatch, progress + 1, !nextMatch.IsInputEnd))
                        yield return futureMatch;
                }
            }
        }
    }
}