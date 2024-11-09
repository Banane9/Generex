using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a reference to a capture group that
    /// transforms its captured match into another object.
    /// </summary>
    /// <remarks>
    /// If the transformation fails, the match will not be returned.
    /// </remarks>
    /// <inheritdoc/>
    public class TransformingCaptureReference<TIn, TOut>
        : CaptureReferenceBase<TIn, TransformedMatchedSequence<TIn, TOut>, TransformingCaptureReference<TIn, TOut>, TransformedCaptureGroup<TIn, TOut>>
    {
        /// <summary>
        /// Gets the <see cref="TryConverter{TInput, TOutput}"/> used to transform
        /// a <see cref="MatchedSequence{T}"/> into a <typeparamref name="TOut"/>.
        /// </summary>
        /// <remarks>
        /// If the transformation fails, the match will not be returned.
        /// </remarks>
        public TryConverter<IEnumerable<TIn>, TOut> TryTransform { get; }

        /// <summary>
        /// Creates a new transforming capture reference with the given <paramref name="tryTransform"/>
        /// and optionally a given <paramref name="name"/> or a <see cref="Guid"/> by default.
        /// </summary>
        /// <param name="tryTransform">
        /// The method converting a captured match into another object.<br/>
        /// If the transformation fails, the match will not be returned.
        /// </param>
        /// <inheritdoc cref="CaptureReference{T}.CaptureReference(string?)"/>
        public TransformingCaptureReference(TryConverter<IEnumerable<TIn>, TOut> tryTransform, string? name = null)
        {
            TryTransform = tryTransform;
            Name = name ?? Guid.NewGuid().ToString();
        }

        /// <inheritdoc/>
        public override TransformedCaptureGroup<TIn, TOut> GetCaptureGroup(IEnumerable<TransformedMatchedSequence<TIn, TOut>> captures)
            => new(this, captures);

        /// <inheritdoc/>
        public override bool TryGetCapture(IEnumerable<MatchState<TIn>> capturedMatch, [NotNullWhen(true)] out TransformedMatchedSequence<TIn, TOut>? capture)
        {
            var sequence = new MatchedSequence<TIn>(capturedMatch);

            if (!TryTransform(sequence, out var transformed))
            {
                capture = null;
                return false;
            }

            capture = new TransformedMatchedSequence<TIn, TOut>(capturedMatch, transformed);
            return true;
        }
    }
}