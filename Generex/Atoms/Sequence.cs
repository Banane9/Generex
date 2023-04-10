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

            return $"({string.Join("⋅", atoms.Select(atom => atom.ToString()))})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            if (progress >= Length)
                yield break;

            foreach (var nextMatch in MatchNext(atoms[progress], currentMatch, value))
            {
                // Advance progress when nested match is complete
                if (nextMatch.IsDone)
                {
                    var newProgress = progress + 1;

                    if (newProgress < Length)
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