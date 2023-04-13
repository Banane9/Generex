using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern which has multiple sub-patterns.
    /// </summary>
    /// <inheritdoc/>
    public abstract class Chain<T> : Generex<T>
    {
        private readonly Generex<T>[] atoms;

        /// <summary>
        /// Gets the sub-patterns in the order of their appearance.
        /// </summary>
        public IEnumerable<Generex<T>> Atoms
        {
            get
            {
                foreach (var atom in atoms)
                    yield return atom;
            }
        }

        /// <summary>
        /// Gets the number of sub-patterns.
        /// </summary>
        public int Length => atoms.Length;

        protected Chain(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : this(atom.Yield().Concat(furtherAtoms))
        { }

        protected Chain(IEnumerable<Generex<T>> atoms)
        {
            this.atoms = atoms.ToArray();
        }

        /// <inheritdoc/>
        public override string ToString() => ToString(false);
    }
}