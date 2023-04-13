using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex.Atoms
{
    public class Sequence<T> : Chain<T>
    {
        public Sequence(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : base(atom, furtherAtoms)
        { }

        public Sequence(IEnumerable<Generex<T>> atoms) : base(atoms)
        { }

        public override string ToString() => ToString(false);

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
            => MatchSequence(currentMatch);

        private IEnumerable<MatchState<T>> MatchSequence(MatchState<T> currentMatch, int progress = 0)
        {
            var newProgress = progress + 1;

            if (newProgress >= Length)
            {
                foreach (var nextMatch in ContinueMatch(Atoms.Skip(progress).First(), currentMatch))
                    yield return nextMatch;

                yield break;
            }

            foreach (var nextMatch in ContinueMatch(Atoms.Skip(progress).First(), currentMatch))
            {
                nextMatch.IsMatchEnd = false;

                foreach (var futureMatch in MatchSequence(nextMatch, newProgress))
                    yield return futureMatch;
            }
        }
    }
}