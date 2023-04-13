using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern that contains a single other pattern nested inside.
    /// </summary>
    /// <inheritdoc/>
    public abstract class UnaryModifier<T> : Generex<T>
    {
        /// <summary>
        /// Gets the pattern nested inside this.
        /// </summary>
        public Generex<T> Atom { get; }

        protected UnaryModifier(Generex<T> atom)
        {
            Atom = atom;
        }
    }
}