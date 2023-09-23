using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a sequence of sub-patterns, all of which need to match one after the other.
    /// </summary>
    /// <inheritdoc/>
    public class Sequence<T> : Junction<T>
    {
        public Sequence(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : base(atom, furtherAtoms)
        { }

        public Sequence(IEnumerable<Generex<T>> atoms) : base(atoms)
        { }

        /// <inheritdoc/>
        public override string ToString(bool grouped)
        {
            if (Length == 1)
                return Atoms.First().ToString(grouped);

            var sequence = string.Join(SequenceSeparator, Atoms.Select(atom => atom.ToString(atom is not Sequence<T>)));

            if (grouped)
                return $"({sequence})";
            else
                return sequence;
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
            => Atoms.Aggregate(currentMatch.Yield(), (currentMatches, atom) => currentMatches.SelectMany(match => ContinueMatch(atom, match)));
    }
}