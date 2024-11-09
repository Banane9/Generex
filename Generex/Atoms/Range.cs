using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents multiple literals or ranges of literals, at least one of which a value has to fall into to match.<br/>
    /// Alternatively, it can be negated, so that a value has to fall into none of the ranges to match.
    /// </summary>
    /// <inheritdoc/>
    public class Range<T> : Generex<T>
    {
        private readonly LiteralRange<T>[] _ranges;

        /// <summary>
        /// Gets the number of ranges.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Gets whether the <see cref="LiteralRange{T}">LiteralRanges</see> that are part
        /// of this range are treated as a blacklist.
        /// </summary>
        public bool Negated { get; }

        /// <summary>
        /// Gets the ranges in the order of their appearance.
        /// </summary>
        public IEnumerable<LiteralRange<T>> Ranges => _ranges.AsSafeEnumerable();

        public Range(IEnumerable<LiteralRange<T>> ranges) : this(false, ranges)
        { }

        public Range(LiteralRange<T> range, params LiteralRange<T>[] furtherRanges)
            : this(false, range.Yield().Concat(furtherRanges))
        { }

        public Range(bool negated, LiteralRange<T> range, params LiteralRange<T>[] furtherRanges)
            : this(negated, range.Yield().Concat(furtherRanges))
        { }

        public Range(bool negated, IEnumerable<LiteralRange<T>> ranges)
        {
            this._ranges = ranges.ToArray();
            Count = this._ranges.Length;
            Negated = negated;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"[{(Negated ? "^" : "")}{string.Join(SequenceSeparator, _ranges.Select(range => range.ToString()))}]";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (!currentMatch.IsInputEnd && (Negated ^ _ranges.Any(range => range.Contains(currentMatch.NextValue))))
                yield return currentMatch.Next();
        }
    }
}