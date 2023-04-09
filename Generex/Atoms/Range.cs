using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class Range<T> : Generex<T>, IEnumerable<LiteralRange<T>>
    {
        private readonly LiteralRange<T>[] ranges;

        public int Length => ranges.Length;

        public IEnumerable<LiteralRange<T>> Ranges
        {
            get
            {
                foreach (var range in ranges)
                    yield return range;
            }
        }

        public Range(IEnumerable<LiteralRange<T>> ranges)
            : base(new ComparerEquality(ranges.First().Comparer))
        {
            this.ranges = ranges.ToArray();
        }

        // Version for IComparable<T>?

        public IEnumerator<LiteralRange<T>> GetEnumerator()
        {
            return ((IEnumerable<LiteralRange<T>>)ranges).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ranges.GetEnumerator();
        }

        public override string ToString()
        {
            if (Length == 1)
                return ranges[0].ToString();

            return $"[{string.Join("|", ranges.Select(range => range.ToString()))}]";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            foreach (var range in ranges)
            {
                if (range.Contains(value))
                    yield return currentMatch.DoneWithNext(value);
            }
        }

        private sealed class ComparerEquality : IEqualityComparer<T>
        {
            private readonly IComparer<T> comparer;

            public ComparerEquality(IComparer<T>? comparer)
            {
                this.comparer = comparer ?? Comparer<T>.Default;
            }

            public bool Equals(T x, T y) => comparer.Compare(x, y) == 0;

            public int GetHashCode(T obj) => EqualityComparer<T>.Default.GetHashCode(obj);
        }
    }
}