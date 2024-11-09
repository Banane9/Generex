using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Generex
{
    public class TransformingCaptureReference<TIn, TOut> : CaptureReference<TIn>
    {
        /// <summary>
        /// Gets the <see cref="TryConverter{TInput, TOutput}"/> used to transform
        /// a <see cref="MatchedSequence{T}"/> into a <typeparamref name="TOut"/>.
        /// </summary>
        public TryConverter<IEnumerable<TIn>, TOut> TryTransform { get; }

        /// <summary>
        /// Creates a new transforming capture reference with the given <paramref name="tryTransform"/>
        /// and optionally a given <paramref name="name"/> or a <see cref="Guid"/> by default.
        /// </summary>
        /// <inheritdoc cref="CaptureReference{T}.CaptureReference(string?)"/>
        public TransformingCaptureReference(TryConverter<IEnumerable<TIn>, TOut> tryTransform, string? name = null)
        {
            TryTransform = tryTransform;
            Name = name ?? Guid.NewGuid().ToString();
        }

        public override bool TryGetCapture(IEnumerable<MatchState<TIn>> capturedMatch, [NotNullWhen(true)] out MatchedSequence<TIn>? matchedSequence)
        {
            var sequence = new MatchedSequence<TIn>(capturedMatch);

            if (!TryTransform(sequence, out var transformed))
            {
                matchedSequence = default;
                return false;
            }

            matchedSequence = new TransformedMatchedSequence<TIn, TOut>(capturedMatch, transformed);
            return true;
        }
    }
}