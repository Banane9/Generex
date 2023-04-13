using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex.Atoms
{
    public class Sequence<T> : Generex<T>, IEnumerable<Generex<T>>
    {
        private readonly Generex<T>[] atoms;

        public IEnumerable<Generex<T>> Atoms
        {
            get
            {
                foreach (var atom in atoms)
                    yield return atom;
            }
        }

        public int Length => atoms.Length;

        public Sequence(Generex<T> atom, params Generex<T>[] furtherAtoms) : this(atom.Yield().Concat(furtherAtoms))
        { }

        public Sequence(IEnumerable<Generex<T>> atoms)
        {
            this.atoms = atoms.ToArray();
        }

        public IEnumerator<Generex<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        public override string ToString(bool grouped)
        {
            if (Length == 1)
                return atoms[0].ToString(grouped);

            var sequence = string.Join(SequenceSeparator, atoms.Select(atom => atom.ToString(true)));

            if (grouped)
                return $"({sequence})";
            else
                return sequence;
        }

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
            => MatchSequence(currentMatch);

        private IEnumerable<MatchElement<T>> MatchSequence(MatchElement<T> currentMatch, int progress = 0)
        {
            var newProgress = progress + 1;

            if (newProgress >= Length)
            {
                foreach (var nextMatch in MatchNext(atoms[progress], currentMatch))
                    yield return nextMatch;

                yield break;
            }

            foreach (var nextMatch in MatchNext(atoms[progress], currentMatch))
            {
                nextMatch.IsDone = false;

                foreach (var futureMatch in MatchSequence(nextMatch, newProgress))
                    yield return futureMatch;
            }
        }
    }
}