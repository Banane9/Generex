using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex.Atoms
{
    public class Sequence<T> : Atom<T>, IEnumerable<Atom<T>>
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

        public Sequence(params Atom<T>[] atoms) : base(atoms.First().EqualityComparer)
        {
            if (atoms.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(atoms), "Sequence must have at least one element!");

            this.atoms = atoms;
        }

        public Sequence(IEnumerable<Atom<T>> atoms) : this(atoms.ToArray())
        { }

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        public override string ToString()
        {
            if (atoms.Length == 1)
                return atoms[0].ToString();

            return $"({string.Join("⋅", atoms.Select(atom => atom.ToString()))})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            if (progress >= atoms.Length)
                yield break;

            foreach (var nextMatch in MatchNext(atoms[progress], currentMatch, value))
            {
                // Advance progress when nested match is complete
                if (nextMatch.IsDone)
                {
                    var newProgress = progress + 1;

                    if (newProgress < atoms.Length)
                    {
                        nextMatch.IsDone = false;
                        nextMatch.SetState(this, newProgress);
                    }
                    else
                        // Reset state so this can be used again
                        nextMatch.SetState(this, 0);
                }

                yield return nextMatch;
            }
        }
    }
}