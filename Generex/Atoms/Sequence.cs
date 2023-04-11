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

        public override string ToString()
        {
            if (Length == 1)
                return atoms[0].ToString();

            return $"({string.Join(SequenceSeparator, atoms.Select(atom => atom.ToString()))})";
        }

        protected override bool MatchEndInternal(MatchElement currentMatch)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            if (progress >= Length)
                throw new InvalidOperationException("Sequence can't be at progress >= Length!");

            // Check if all remaining elements in the sequence accept an end match
            while (MatchEnd(atoms[progress], currentMatch) && ++progress < Length) ;

            return progress >= Length;
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch)
            => MatchSequence(currentMatch);

        private IEnumerable<MatchElement> MatchSequence(MatchElement currentMatch, int progress = 0)
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