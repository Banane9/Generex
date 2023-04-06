using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class Range<T> : Atom<T>
    {
        public T Maximum { get; }
        public T Minimum { get; }

        public Range(T minimum, T maximum, IComparer<T> comparer, IEqualityComparer<T>? equalityComparer = null)
            : base(equalityComparer ?? new EqualityComparer(comparer))
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        private sealed class EqualityComparer : IEqualityComparer<T>
        {
            private readonly IComparer<T> comparer;

            public EqualityComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public bool Equals(T x, T y) => comparer.Compare(x, y) == 0;

            public int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
        }
    }
}