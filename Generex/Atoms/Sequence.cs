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

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            if (progress >= Length)
                throw new InvalidOperationException("Sequence can't be at progress >= Length!");

            foreach (var nextMatch in MatchNext(atoms[progress], currentMatch, value))
            {
                // Nothing to do when nested atom isn't done
                if (!nextMatch.IsDone)
                {
                    yield return nextMatch;
                    continue;
                }

                var newProgress = progress + 1;
                if (newProgress >= Length)
                {
                    // Reset state when done, so this atom can be used again
                    nextMatch.SetState(this, 0);
                    yield return nextMatch;
                    continue;
                }

                // Sequence not done - advance progress
                nextMatch.IsDone = false;
                nextMatch.SetState(this, newProgress);

                if (nextMatch.Index > currentMatch.Index)
                {
                    yield return nextMatch;
                    continue;
                }

                // Have to recurse to pass the value to the next atom in the sequence,
                // if nextMatch is zero-width and this sequence isn't done.
                // If this sequence is done, it's the parent's problem or the match is actually complete.
                foreach (var nextNextMatch in MatchNextInternal(nextMatch, value))
                    yield return nextNextMatch;
            }
        }
    }
}