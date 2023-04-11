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
        {
            this.ranges = ranges.ToArray();
        }

        public Range(LiteralRange<T> range, params LiteralRange<T>[] furtherRanges)
            : this(range.Yield().Concat(furtherRanges))
        { }

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
                return $"[{ranges[0]}]";

            return $"[{string.Join(SequenceSeparator, ranges.Select(range => range.ToString()))}]";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch)
        {
            if (!currentMatch.HasNext)
                yield break;

            foreach (var range in ranges)
            {
                if (range.Contains(currentMatch.NextValue))
                {
                    yield return currentMatch.DoneWithNext();
                    yield break;
                }
            }
        }
    }
}