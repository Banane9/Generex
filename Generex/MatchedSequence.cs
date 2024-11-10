using Generex.Atoms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a matched sequence that got saved and which can be referenced,
    /// either through a <see cref="CapturedGroup{T}"/> using the same
    /// <see cref="ICaptureReference{T}"/> or in the resulting <see cref="Match{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public class MatchedSequence<T> : IEnumerable<T>
    {
        private readonly T[] _sequence;

        /// <summary>
        /// Gets an invalid matched sequence.
        /// </summary>
        public static MatchedSequence<T> Invalid { get; } = new MatchedSequence<T>(-1);

        /// <summary>
        /// Gets the index of the last item in this matched sequence.
        /// </summary>
        public int EndIndex { get; }

        /// <summary>
        /// Gets the number of items in this matched sequence.
        /// </summary>
        /// <remarks>
        /// Zero-width matches are possible.
        /// </remarks>
        public int Length => _sequence.Length;

        /// <summary>
        /// Gets the index of the first item in this matched sequence.
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the item at the specified
        /// <paramref name="index"/> in this matched sequence.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        /// <returns>The item at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">When the <paramref name="index"/> is <c>&lt; 0</c> or <c>&gt;= <see cref="Length">Length</see></c>.</exception>
        public T this[int index] => _sequence[index];

        /// <summary>
        /// Creates a matched sequence spanning the given sequence of items in the input sequence.
        /// </summary>
        /// <param name="matchSequence">The sequence of items that the match spans.</param>
        /// <exception cref="ArgumentOutOfRangeException">When there's zero items in the <paramref name="matchSequence"/>.</exception>
        public MatchedSequence(IEnumerable<MatchState<T>> matchSequence)
        {
            var matches = matchSequence.ToArray();

            if (matches.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(matchSequence), "Match Sequence must have at least one element!");

            _sequence = matchSequence.Select(match => match.NextValue).ToArray();
            StartIndex = matches[0].Index;
            EndIndex = matches[^1].Index;
        }

        /// <summary>
        /// Creates a zero-width match at the given zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the zero-width match.</param>
        public MatchedSequence(int index)
        {
            _sequence = [];
            StartIndex = index;
            EndIndex = index;
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_sequence).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _sequence.GetEnumerator();
    }
}