using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class LazyQuantifier<T> : Quantifier<T>
    {
        public LazyQuantifier(Generex<T> atom, int exactly) : base(atom, exactly)
        { }

        public LazyQuantifier(Generex<T> atom, int minimum, int maximum) : base(atom, minimum, maximum)
        { }

        public override string ToString() => Minimum == Maximum ? base.ToString() : $"{base.ToString()}?";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (Minimum == 0)
                yield return currentMatch;

            foreach (var quantityMatch in MatchQuantity(currentMatch))
                yield return quantityMatch;
        }

        private IEnumerable<MatchState<T>> MatchQuantity(MatchState<T> currentMatch, int progress = 1, bool tryWithoutNext = true)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                // Lazy order: shorter matches first
                if (progress >= Minimum)
                    yield return nextMatch;

                // Only recurse when it's not at the max number yet
                if (progress < Maximum && (tryWithoutNext || !nextMatch.IsInputEnd))
                    foreach (var futureMatch in MatchQuantity(nextMatch, progress + 1, !nextMatch.IsInputEnd))
                        yield return futureMatch;
            }
        }
    }
}