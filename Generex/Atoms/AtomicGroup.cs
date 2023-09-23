using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern who's sub-pattern can only be matched all-or-nothing, preventing any backtracking into it.
    /// </summary>
    /// <inheritdoc/>
    public class AtomicGroup<T> : UnaryModifier<T>
    {
        public AtomicGroup(Generex<T> atom) : base(atom)
        { }

        /// <inheritdoc/>
        public override string ToString()
            // Could also represent it with a + behind quantifiers, but .NET doesn't support that,
            // so ToString it only in the atomic group notation which is supported.
            // Atom is Quantifier<T> ? $"{Atom}+" : $"(?>{Atom})"
            => $"(?>{Atom})";

        protected override IEnumerable<MatchState<T>> continueMatchInternal(MatchState<T> currentMatch)
        {
            if (continueMatch(Atom, currentMatch).FirstOrDefault() is MatchState<T> matchChoice)
                yield return matchChoice;

            yield break;
        }
    }
}