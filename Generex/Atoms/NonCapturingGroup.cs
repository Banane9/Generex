using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern who's sub-pattern's matched sequence won't be part of the resulting <see cref="Match{T}"/>.
    /// </summary>
    /// <inheritdoc/>
    public class NonCapturingGroup<T> : UnaryModifier<T>
    {
        public NonCapturingGroup(Generex<T> atom) : base(atom)
        { }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"(?:{Atom})";
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                foreach (var match in nextMatch.GetParentSequence()
                                        .TakeUntil(match => match != currentMatch)
                                        .Skip(1))
                    match.Capturing = false;

                yield return nextMatch;
            }
        }
    }
}