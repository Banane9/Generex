using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public abstract class Atom<T>
    {
        public IEqualityComparer<T> Comparer { get; }

        public Atom(IEqualityComparer<T>? comparer = null)
        {
            Comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public static Alternative<T> operator |(Atom<T> leftAtom, Atom<T> rightAtom) => new(leftAtom, rightAtom);

        public static Sequence<T> operator +(Atom<T> leftAtom, Atom<T> rightAtom) => new(leftAtom, rightAtom);
    }
}