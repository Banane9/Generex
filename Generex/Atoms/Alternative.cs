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

        public override string ToString(bool grouped)
        {
            if (Length == 0)
                return atoms[0].ToString(grouped);

            var alternatives = string.Join("|", atoms.Select(atom => atom.ToString(atom is Sequence<T>)));

            if (grouped)
                return $"({alternatives})";
            else
                return alternatives;
        }

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
            => Atoms.SelectMany(atom => MatchNext(atom, currentMatch.Clone()));
    }
}