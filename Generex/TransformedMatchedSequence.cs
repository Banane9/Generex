using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a matched sequence that got saved and which can be referenced,
    /// either through a <see cref="CapturedGroup{T}"/> using the same
    /// <see cref="ICaptureReference{T}"/> or in the resulting <see cref="Match{T}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of elements in the input sequence.</typeparam>
    /// <typeparam name="TOut">The type of the object that the matched sequence is transformed into.</typeparam>
    public class TransformedMatchedSequence<TIn, TOut> : MatchedSequence<TIn>
    {
        /// <summary>
        /// Gets the transformed object representing this matched sequence.
        /// </summary>
        public TOut Transformed { get; }

        /// <summary>
        /// Creates a matched sequence spanning the given sequence of items in the input sequence,
        /// including a transformed object representing the matched sequence.
        /// </summary>
        /// <param name="transformed">The transformed object representing the matched sequence.</param>
        /// <inheritdoc cref="MatchedSequence{T}.MatchedSequence(IEnumerable{MatchState{T}})"/>
        public TransformedMatchedSequence(IEnumerable<MatchState<TIn>> matchSequence, TOut transformed)
            : base(matchSequence)
        {
            Transformed = transformed;
        }

        /// <summary>
        /// Creates a zero-width match at the given zero-based <paramref name="index"/>,
        /// including a transformed object representing the match.
        /// </summary>
        /// <param name="transformed">The transformed object representing the match.</param>
        /// <inheritdoc cref="MatchedSequence{T}.MatchedSequence(int)"/>
        public TransformedMatchedSequence(int index, TOut transformed)
            : base(index)
        {
            Transformed = transformed;
        }
    }
}