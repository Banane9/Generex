using EnumerableToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex
{
    /// <summary>
    /// Represents the sequence of <typeparamref name="TCapture"/>s
    /// that was made for a certain <typeparamref name="TCaptureGroup"/>
    /// to achieve a <see cref="Match{T}"/>.
    /// </summary>
    public sealed class CaptureGroup<T> : CaptureGroupBase<T, MatchedSequence<T>, CaptureReference<T>, CaptureGroup<T>>
    {
        /// <inheritdoc/>
        public CaptureGroup(CaptureReference<T> captureReference, IEnumerable<MatchedSequence<T>> captures)
            : base(captureReference, captures) { }
    }

    /// <summary>
    /// Represents the base class for sequences of <typeparamref name="TCapture"/>s
    /// that were made for a certain <typeparamref name="TCaptureGroup"/>
    /// to achieve a <see cref="Match{T}"/>.
    /// </summary>
    /// <typeparam name="TIn">The type of elements in the input sequence.</typeparam>
    /// <typeparam name="TCapture">The type of the captures in this group.</typeparam>
    /// <typeparam name="TCaptureReference">The type of the reference used to refer to this group's captures.</typeparam>
    /// <typeparam name="TCaptureGroup">The type of this capture group.</typeparam>
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

        /// <summary>
        /// Creates a new capture group for the given <paramref name="captureReference"/>
        /// with the <paramref name="captures"/> that were made to achieve a <see cref="Match{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference used to refer to this group's captures.</param>
        /// <param name="captures">
        /// The sequence of captures that were made for this group's
        /// <see cref="CaptureReference">CaptureReference</see> to achieve a <see cref="Match{T}"/>.
        /// </param>
        public CaptureGroupBase(TCaptureReference captureReference, IEnumerable<TCapture> captures)
        {
            CaptureReference = captureReference;
            _captures = captures.ToArray();
        }

        /// <inheritdoc/>
        public IEnumerator<TCapture> GetEnumerator()
            => ((IEnumerable<TCapture>)_captures).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _captures.GetEnumerator();
    }

    /// <summary>
    /// Defines the interface for capture groups.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ICaptureGroup<T>
    {
        /// <summary>
        /// Gets the reference used to refer to this group's captures.
        /// </summary>
        public ICaptureReference<T> CaptureReference { get; }

        /// <summary>
        /// Gets the sequence of captures that were made for this group's
        /// <see cref="CaptureReference">CaptureReference</see> to achieve a <see cref="Match{T}"/>.
        /// </summary>
        public IEnumerable<MatchedSequence<T>> Captures { get; }

        /// <summary>
        /// Gets the first capture of this group.
        /// </summary>
        public MatchedSequence<T> First { get; }

        /// <summary>
        /// Gets the last capture of this group.
        /// </summary>
        public MatchedSequence<T> Last { get; }
    }

    /// <inheritdoc/>
    public class TransformedCaptureGroup<TIn, TOut>
        : CaptureGroupBase<TIn, TransformedMatchedSequence<TIn, TOut>, TransformingCaptureReference<TIn, TOut>, TransformedCaptureGroup<TIn, TOut>>,
            IEnumerable<TransformedMatchedSequence<TIn, TOut>>
    {
        /// <inheritdoc/>
        public TransformedCaptureGroup(TransformingCaptureReference<TIn, TOut> captureReference, IEnumerable<TransformedMatchedSequence<TIn, TOut>> captures)
            : base(captureReference, captures)
        { }
    }
}