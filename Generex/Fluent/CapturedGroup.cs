﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a ready-to-use back-reference to another capturing group,
    /// which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionCapturedGroupEnd<T> : IAdditionAtom<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the back-reference.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The fully configured back-reference.</returns>
        IAdditionAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for an unfinished back-reference to another capturing group,
    /// which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionCapturedGroupStart<T>
    {
        /// <summary>
        /// Sets which capturing group this refers back to.
        /// </summary>
        /// <param name="captureReference">The capturing group's <see cref="CaptureReference{T}"/>.</param>
        /// <returns>The ready-to-use back-reference to another capturing group.</returns>
        IAdditionCapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a ready-to-use back-reference to another capturing group,
    /// which is part of a list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IAlternativeCapturedGroupEnd<T> : IAlternativeAtom<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the back-reference.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The fully configured back-reference.</returns>
        IAlternativeAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for an unfinished back-reference to another capturing group,
    /// which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeCapturedGroupStart<T>
    {
        /// <summary>
        /// Sets which capturing group this refers back to.
        /// </summary>
        /// <param name="captureReference">The capturing group's <see cref="CaptureReference{T}"/>.</param>
        /// <returns>The ready-to-use back-reference to another capturing group.</returns>
        IAlternativeCapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a ready-to-use back-reference to another capturing group,
    /// which is not yet part of a sequence or list.
    /// </summary>
    /// <inheritdoc/>
    public interface ICapturedGroupEnd<T> : IAtom<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the back-reference.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The fully configured back-reference.</returns>
        IAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for an unfinished back-reference to another capturing group,
    /// which is not yet part of a sequence or list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ICapturedGroupStart<T>
    {
        /// <summary>
        /// Sets which capturing group this refers back to.
        /// </summary>
        /// <param name="captureReference">The capturing group's <see cref="CaptureReference{T}"/>.</param>
        /// <returns>The ready-to-use back-reference to another capturing group.</returns>
        ICapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a ready-to-use back-reference to another capturing group, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceCapturedGroupEnd<T> : ISequenceAtom<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the back-reference.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The fully configured back-reference.</returns>
        ISequenceAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for an unfinished back-reference to another capturing group, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceCapturedGroupStart<T>
    {
        /// <summary>
        /// Sets which capturing group this refers back to.
        /// </summary>
        /// <param name="captureReference">The capturing group's <see cref="CaptureReference{T}"/>.</param>
        /// <returns>The ready-to-use back-reference to another capturing group.</returns>
        ISequenceCapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    internal class CapturedGroup<T> : Atom<T>, ICapturedGroupStart<T>, ICapturedGroupEnd<T>,
        IAlternativeCapturedGroupStart<T>, IAlternativeCapturedGroupEnd<T>,
        IAdditionCapturedGroupStart<T>, IAdditionCapturedGroupEnd<T>,
        ISequenceCapturedGroupStart<T>, ISequenceCapturedGroupEnd<T>
    {
        private CaptureReference<T>? _captureReference;
        private IEqualityComparer<T>? _equalityComparer;

        public CapturedGroup(IParentAtom<T>? parent = null) : base(parent)
        { }

        public ICapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference)
            => ReferringBackToInternal(captureReference);

        ISequenceCapturedGroupEnd<T> ISequenceCapturedGroupStart<T>.ReferringBackTo(CaptureReference<T> captureReference)
            => ReferringBackToInternal(captureReference);

        IAdditionCapturedGroupEnd<T> IAdditionCapturedGroupStart<T>.ReferringBackTo(CaptureReference<T> captureReference)
            => ReferringBackToInternal(captureReference);

        IAlternativeCapturedGroupEnd<T> IAlternativeCapturedGroupStart<T>.ReferringBackTo(CaptureReference<T> captureReference)
            => ReferringBackToInternal(captureReference);

        public IAtom<T> Using(IEqualityComparer<T> equalityComparer) => UsingInternal(equalityComparer);

        IAdditionAtom<T> IAdditionCapturedGroupEnd<T>.Using(IEqualityComparer<T> equalityComparer)
            => UsingInternal(equalityComparer);

        IAlternativeAtom<T> IAlternativeCapturedGroupEnd<T>.Using(IEqualityComparer<T> equalityComparer)
            => UsingInternal(equalityComparer);

        ISequenceAtom<T> ISequenceCapturedGroupEnd<T>.Using(IEqualityComparer<T> equalityComparer)
            => UsingInternal(equalityComparer);

        protected override Generex<T> FinishInternal()
            => new Atoms.CapturedGroup<T>(_captureReference!, _equalityComparer);

        private CapturedGroup<T> ReferringBackToInternal(CaptureReference<T> captureReference)
        {
            _captureReference = captureReference;
            return this;
        }

        private CapturedGroup<T> UsingInternal(IEqualityComparer<T> equalityComparer)
        {
            _equalityComparer = equalityComparer;
            return this;
        }
    }
}