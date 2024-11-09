using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern consisting of multiple sub-patterns.
    /// </summary>
    /// <inheritdoc/>
    public abstract class Junction<T> : Generex<T>
    {
        private readonly Generex<T>[] _atoms;

        /// <summary>
        /// Gets the sub-patterns in the order of their appearance.
        /// </summary>
        public IEnumerable<Generex<T>> Atoms => _atoms.AsSafeEnumerable();

        /// <summary>
        /// Gets the number of sub-patterns.
        /// </summary>
        public int Length => _atoms.Length;

        protected Junction(Generex<T> atom, params Generex<T>[] furtherAtoms)
        {
            _atoms = [atom, .. furtherAtoms];
        }

        protected Junction(IEnumerable<Generex<T>> atoms)
        {
            this._atoms = atoms.ToArray();
        }

        /// <inheritdoc/>
        public override string ToString() => ToString(false);
    }
}