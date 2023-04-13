using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Ensures that the pattern inside this can only be matched all-or-nothing,
    /// preventing any backtracking into it.
    /// </summary>
    public class AtomicGroup<T> : UnaryModifier<T>
    {
        public AtomicGroup(Generex<T> atom) : base(atom)
        { }

        public override string ToString()
            => $"(?>{Atom})";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (ContinueMatch(Atom, currentMatch).FirstOrDefault() is MatchState<T> matchChoice)
                yield return matchChoice;

            yield break;
        }
    }
}