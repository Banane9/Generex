using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
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
            this.atoms = atoms;
        }

        public Alternative(IEnumerable<Atom<T>> atoms) : this(atoms.ToArray())
        { }

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement match, T value)
        {
            return atoms.SelectMany(atom => MatchNext(atom, match, value));
        }
    }
}