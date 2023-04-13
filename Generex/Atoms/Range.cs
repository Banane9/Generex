﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class Range<T> : Generex<T>
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

        public override string ToString()
            => $"[{string.Join(SequenceSeparator, ranges.Select(range => range.ToString()))}]";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (!currentMatch.IsInputEnd && ranges.Any(range => range.Contains(currentMatch.NextValue)))
                yield return currentMatch.Next();
        }
    }
}