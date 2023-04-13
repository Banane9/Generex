using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public abstract class Chain<T> : Generex<T>
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

        protected Chain(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : this(atom.Yield().Concat(furtherAtoms))
        { }

        protected Chain(IEnumerable<Generex<T>> atoms)
        {
            this.atoms = atoms.ToArray();
        }
    }
}