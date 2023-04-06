using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex
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

        public Sequence(params Atom<T>[] atoms) : base(atoms.First().Comparer)
        {
            this.atoms = atoms;
        }

        public Sequence(IEnumerable<Atom<T>> atoms) : this(atoms.ToArray())
        { }

        public static implicit operator Sequence<T>(Atom<T>[] atoms) => new(atoms);

        public static implicit operator Sequence<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();
    }
}