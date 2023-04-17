using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
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
    /// which is not yet part of a sequence or list of alternatives.
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
    /// which is not yet part of a sequence or list of alternatives.
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
        ISequenceCapturedGroupStart<T>, ISequenceCapturedGroupEnd<T>
    {
        private CaptureReference<T>? captureReference;
        private IEqualityComparer<T>? equalityComparer;

        public CapturedGroup(IParentAtom<T>? parent = null) : base(parent)
        { }

        public ICapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference)
        {
            this.captureReference = captureReference;
            return this;
        }

        IAlternativeCapturedGroupEnd<T> IAlternativeCapturedGroupStart<T>.ReferringBackTo(CaptureReference<T> captureReference)
            => (IAlternativeCapturedGroupEnd<T>)ReferringBackTo(captureReference);

        ISequenceCapturedGroupEnd<T> ISequenceCapturedGroupStart<T>.ReferringBackTo(CaptureReference<T> captureReference)
            => (ISequenceCapturedGroupEnd<T>)ReferringBackTo(captureReference);

        public IAtom<T> Using(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            return this;
        }

        IAlternativeAtom<T> IAlternativeCapturedGroupEnd<T>.Using(IEqualityComparer<T> equalityComparer)
            => (IAlternativeAtom<T>)Using(equalityComparer);

        ISequenceAtom<T> ISequenceCapturedGroupEnd<T>.Using(IEqualityComparer<T> equalityComparer)
            => (ISequenceAtom<T>)Using(equalityComparer);

        protected override Generex<T> FinishInternal()
            => new Atoms.CapturedGroup<T>(captureReference!, equalityComparer);
    }
}