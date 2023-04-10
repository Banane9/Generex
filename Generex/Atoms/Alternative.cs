using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class Alternative<T> : Generex<T>, IEnumerable<Generex<T>>
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

        public Alternative(Generex<T> atom, params Generex<T>[] furtherAtoms) : this(atom.Yield().Concat(furtherAtoms))
        { }

        public Alternative(IEnumerable<Generex<T>> atoms)
        {
            this.atoms = atoms.ToArray();
        }

        public IEnumerator<Generex<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        public override string ToString()
        {
            if (Length == 1)
                return atoms[0].ToString();

            return $"({string.Join("|", atoms.Select(atom => atom.ToString()))})";
        }

        protected override bool MatchEndInternal(MatchElement currentMatch)
        {
            // Check if option matches if one was picked
            if (currentMatch.TryGetLatestState(this, out int option) && option >= 0)
                return MatchEnd(atoms[option], currentMatch);

            // Any option matching is enough otherwise
            return atoms.Any(atom => MatchEnd(atom, currentMatch));
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            // Check if option matches if one was picked
            if (currentMatch.TryGetLatestState(this, out int option) && option >= 0)
            {
                foreach (var nextMatch in MatchNext(atoms[option], currentMatch, value))
                {
                    // Reset state when done so this can be used again
                    if (nextMatch.IsDone)
                        nextMatch.SetState(this, -1);

                    yield return nextMatch;
                }

                yield break;
            }

            // Attempt all options otherwise
            for (var i = 0; i < Length; ++i)
            {
                foreach (var nextMatch in MatchNext(atoms[i], currentMatch, value))
                {
                    // Only set state when not done so this can be used again
                    if (!nextMatch.IsDone)
                        nextMatch.SetState(this, i);

                    yield return nextMatch;
                }
            }
        }
    }
}