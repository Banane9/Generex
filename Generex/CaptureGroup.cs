using EnumerableToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex
{
    public sealed class CaptureGroup<T> : CaptureGroupBase<T, MatchedSequence<T>, CaptureReference<T>, CaptureGroup<T>>
    {
        /// <inheritdoc/>
        public CaptureGroup(CaptureReference<T> captureReference, IEnumerable<MatchedSequence<T>> captures)
            : base(captureReference, captures) { }
    }

    public abstract class CaptureGroupBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
        : ICaptureGroup<TIn>, IEnumerable<TCapture>
        where TCapture : MatchedSequence<TIn>
        where TCaptureReference : CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
        where TCaptureGroup : CaptureGroupBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
    {
        private readonly TCapture[] _captures;

        /// <inheritdoc cref="ICaptureGroup{T}.CaptureReference"/>
        public TCaptureReference CaptureReference { get; }

        ICaptureReference<TIn> ICaptureGroup<TIn>.CaptureReference => CaptureReference;

        IEnumerable<MatchedSequence<TIn>> ICaptureGroup<TIn>.Captures
            => _captures.AsSafeEnumerable();

        /// <inheritdoc cref="ICaptureGroup{T}.First"/>
        public TCapture First => _captures[0];

        MatchedSequence<TIn> ICaptureGroup<TIn>.First => First;

        /// <inheritdoc cref="ICaptureGroup{T}.Last"/>
        public TCapture Last => _captures[^1];

        MatchedSequence<TIn> ICaptureGroup<TIn>.Last => Last;

        public CaptureGroupBase(TCaptureReference captureReference, IEnumerable<TCapture> captures)
        {
            CaptureReference = captureReference;
            _captures = captures.ToArray();
        }

        public IEnumerator<TCapture> GetEnumerator()
            => ((IEnumerable<TCapture>)_captures).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _captures.GetEnumerator();
    }

    public interface ICaptureGroup<T>
    {
        public ICaptureReference<T> CaptureReference { get; }
        public IEnumerable<MatchedSequence<T>> Captures { get; }
        public MatchedSequence<T> First { get; }
        public MatchedSequence<T> Last { get; }
    }

    public class TransformedCaptureGroup<TIn, TOut>
        : CaptureGroupBase<TIn, TransformedMatchedSequence<TIn, TOut>, TransformingCaptureReference<TIn, TOut>, TransformedCaptureGroup<TIn, TOut>>,
            IEnumerable<TransformedMatchedSequence<TIn, TOut>>
    {
        internal TransformedCaptureGroup(TransformingCaptureReference<TIn, TOut> captureReference, IEnumerable<TransformedMatchedSequence<TIn, TOut>> captures)
            : base(captureReference, captures)
        { }
    }
}