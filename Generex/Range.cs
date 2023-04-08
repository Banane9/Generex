using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class Range<T> : Atom<T>
    {
        public IComparer<T> Comparer { get; }
        public T Maximum { get; }
        public T Minimum { get; }

        public Range(T minimum, T maximum, IComparer<T> comparer, IEqualityComparer<T>? equalityComparer = null)
            : base(equalityComparer ?? new ComparerEquality(comparer))
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = comparer;
        }

        // Version for IComparable<T>?

        public override string ToString()
        {
            return $"[{Minimum}-{Maximum}]";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            if (Comparer.Compare(Minimum, value) <= 0 && Comparer.Compare(Maximum, value) >= 0)
                yield return currentMatch.DoneWithNext(value);
        }

        private sealed class ComparerEquality : IEqualityComparer<T>
        {
            private readonly IComparer<T> comparer;

            public ComparerEquality(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public bool Equals(T x, T y) => comparer.Compare(x, y) == 0;

            public int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
        }
    }
}