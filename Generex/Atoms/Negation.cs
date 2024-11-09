using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents the negation of another pattern.<br/>
    /// This will match anything <i>not</i> matched by the nested pattern.
    /// </summary>
    /// <inheritdoc/>
    public class Negation<T> : UnaryModifier<T>
    {
        public Negation(Generex<T> atom) : base(atom)
        { }

        public override string ToString() => $"(?!:{Atom})";

        protected override IEnumerable<MatchState<T>> continueMatchInternal(MatchState<T> currentMatch)
        {
            var acceptableMatches = new HashSet<MatchState<T>>(continueMatch(Atom, currentMatch));

            return currentMatch.AllNext().Where(match => !acceptableMatches.Contains(match));
        }
    }
}