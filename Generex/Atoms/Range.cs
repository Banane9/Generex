using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents multiple literals or ranges of literals, at least one of which a value has to fall into to match.
    /// </summary>
    /// <inheritdoc/>
    public class Range<T> : Generex<T>
    {
        private readonly LiteralRange<T>[] ranges;

        /// <summary>
        /// Gets the number of ranges.
        /// </summary>
        public int Length => ranges.Length;

        /// <summary>
        /// Gets the ranges in the order of their appearance.
        /// </summary>
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

        /// <inheritdoc/>
        public override string ToString()
            => $"[{string.Join(sequenceSeparator, ranges.Select(range => range.ToString()))}]";

        protected override IEnumerable<MatchState<T>> continueMatchInternal(MatchState<T> currentMatch)
        {
            if (!currentMatch.IsInputEnd && ranges.Any(range => range.Contains(currentMatch.NextValue)))
                yield return currentMatch.Next();
        }
    }
}