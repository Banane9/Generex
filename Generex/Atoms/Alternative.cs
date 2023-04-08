using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class Alternative<T> : Atom<T>, IEnumerable<Atom<T>>
    {
        private readonly Atom<T>[] atoms;

        public IEnumerable<Atom<T>> Atoms
        {
            get
            {
                foreach (var atom in atoms)
                    yield return atom;
            }
        }

        public Alternative(params Atom<T>[] atoms) : base(atoms.First().EqualityComparer)
        {
            if (atoms.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(atoms), "Alternative must have at least one option.");

            this.atoms = atoms;
        }

        public Alternative(IEnumerable<Atom<T>> atoms) : this(atoms.ToArray())
        { }

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        public override string ToString()
        {
            if (atoms.Length == 1)
                return atoms[0].ToString();

            return $"({string.Join("|", atoms.Select(atom => atom.ToString()))})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
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

            for (var i = 0; i < atoms.Length; ++i)
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