using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class Quantifier<T> : Atom<T>
    {
        public Atom<T> Atom { get; }

        public Quantifier(Atom<T> atom) : base(atom.Comparer)
        {
            Atom = atom;
        }
    }
}