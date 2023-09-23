using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for an atomic group, which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionAtomicGroup<T>
    {
        /// <summary>
        /// Finishes constructing a plain atomic group, which prevents backtracking into it.
        /// </summary>
        IAdditionAtom<T> Group { get; }

        /// <summary>
        /// Finishes constructing a non-capturing atomic group, which excludes its contents from the final match.
        /// </summary>
        IAdditionAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named atomic capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named atomic capture group.</returns>
        IAdditionUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs an atomic capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured atomic capture group.</returns>
        IAdditionCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a group, which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionGroup<T>
    {
        /// <summary>
        /// Constructs an atomic group, which prevents backtracking into it.
        /// </summary>
        IAdditionAtomicGroup<T> Atomic { get; }

        /// <summary>
        /// Constructs a non-capturing group, which excludes its contents from the final match.
        /// </summary>
        IAdditionAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named capture group.</returns>
        IAdditionUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs a capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured capture group.</returns>
        IAdditionCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for an atomic group, which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeAtomicGroup<T>
    {
        /// <summary>
        /// Finishes constructing a plain atomic group, which prevents backtracking into it.
        /// </summary>
        IAlternativeAtom<T> Group { get; }

        /// <summary>
        /// Finishes constructing a non-capturing atomic group, which excludes its contents from the final match.
        /// </summary>
        IAlternativeAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named atomic capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named atomic capture group.</returns>
        IAlternativeUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs an atomic capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured atomic capture group.</returns>
        IAlternativeCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a group, which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeGroup<T>
    {
        /// <summary>
        /// Constructs an atomic group, which prevents backtracking into it.
        /// </summary>
        IAlternativeAtomicGroup<T> Atomic { get; }

        /// <summary>
        /// Constructs a non-capturing group, which excludes its contents from the final match.
        /// </summary>
        IAlternativeAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named capture group.</returns>
        IAlternativeUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs a capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured capture group.</returns>
        IAlternativeCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for an atomic group,
    /// which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAtomicGroup<T>
    {
        /// <summary>
        /// Finishes constructing a plain atomic group, which prevents backtracking into it.
        /// </summary>
        IAtom<T> Group { get; }

        /// <summary>
        /// Finishes constructing a non-capturing atomic group, which excludes its contents from the final match.
        /// </summary>
        IAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named atomic capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named atomic capture group.</returns>
        IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs an atomic capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured atomic capture group.</returns>
        ICapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a group, which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IGroup<T>
    {
        /// <summary>
        /// Constructs an atomic group, which prevents backtracking into it.
        /// </summary>
        IAtomicGroup<T> Atomic { get; }

        /// <summary>
        /// Constructs a non-capturing group, which excludes its contents from the final match.
        /// </summary>
        IAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named capture group.</returns>
        IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs an capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured capture group.</returns>
        ICapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for an atomic group, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceAtomicGroup<T>
    {
        /// <summary>
        /// Finishes constructing a plain atomic group, which prevents backtracking into it.
        /// </summary>
        ISequenceAtom<T> Group { get; }

        /// <summary>
        /// Finishes constructing a non-capturing atomic group, which excludes its contents from the final match.
        /// </summary>
        ISequenceAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named atomic capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named atomic capture group.</returns>
        ISequenceUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs an atomic capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured atomic capture group.</returns>
        ISequenceCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    /// <summary>
    /// The options for a group, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceGroup<T>
    {
        /// <summary>
        /// Constructs an atomic group, which prevents backtracking into it.
        /// </summary>
        ISequenceAtomicGroup<T> Atomic { get; }

        /// <summary>
        /// Constructs a non-capturing group, which excludes its contents from the final match.
        /// </summary>
        ISequenceAtom<T> NonCapturingGroup { get; }

        /// <summary>
        /// Constructs a not yet explicitly named capture group, creating a new <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">Outputs the capture group's reference.</param>
        /// <returns>The not yet explicitly named capture group.</returns>
        ISequenceUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);

        /// <summary>
        /// Constructs a capture group with an existing <see cref="CaptureReference{T}"/>.
        /// </summary>
        /// <param name="captureReference">The reference to use.</param>
        /// <returns>The fully configured capture group.</returns>
        ISequenceCapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference);
    }

    internal class Grouping<T> : Atom<T>, IParentAtom<T>, IAtomicGroup<T>, IGroup<T>, IUnnamedCapturedAtom<T>,
        IAlternativeAtomicGroup<T>, IAlternativeGroup<T>, IAlternativeUnnamedCapturedAtom<T>,
        IAdditionAtomicGroup<T>, IAdditionGroup<T>, IAdditionUnnamedCapturedAtom<T>,
        ISequenceAtomicGroup<T>, ISequenceGroup<T>, ISequenceUnnamedCapturedAtom<T>
    {
        private IFinishableAtom<T> atom;
        private bool atomic;
        private CaptureReference<T>? captureReference;
        private bool nonCapturing;

        public IAtomicGroup<T> Atomic => atomicGroup;
        IAdditionAtomicGroup<T> IAdditionGroup<T>.Atomic => atomicGroup;
        IAlternativeAtomicGroup<T> IAlternativeGroup<T>.Atomic => atomicGroup;
        ISequenceAtomicGroup<T> ISequenceGroup<T>.Atomic => atomicGroup;

        public IAtom<T> Group => this;
        IAdditionAtom<T> IAdditionAtomicGroup<T>.Group => this;
        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.Group => this;
        ISequenceAtom<T> ISequenceAtomicGroup<T>.Group => this;

        public IAtom<T> NonCapturingGroup => nonCapturingGroup;
        IAdditionAtom<T> IAdditionAtomicGroup<T>.NonCapturingGroup => nonCapturingGroup;
        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.NonCapturingGroup => nonCapturingGroup;
        ISequenceAtom<T> ISequenceAtomicGroup<T>.NonCapturingGroup => nonCapturingGroup;
        IAdditionAtom<T> IAdditionGroup<T>.NonCapturingGroup => nonCapturingGroup;
        ISequenceAtom<T> ISequenceGroup<T>.NonCapturingGroup => nonCapturingGroup;
        IAlternativeAtom<T> IAlternativeGroup<T>.NonCapturingGroup => nonCapturingGroup;

        private Grouping<T> atomicGroup
        {
            get
            {
                atomic = true;
                return this;
            }
        }

        private Grouping<T> nonCapturingGroup
        {
            get
            {
                nonCapturing = true;
                return this;
            }
        }

        public Grouping(IParentAtom<T>? parent, IFinishableAtom<T> atom) : base(parent)
        {
            this.atom = atom;
        }

        public ICapturedAtom<T> Called(string name) => called(name);

        IAdditionCapturedAtom<T> IAdditionUnnamedCapturedAtom<T>.Called(string name)
            => called(name);

        IAlternativeCapturedAtom<T> IAlternativeUnnamedCapturedAtom<T>.Called(string name)
            => called(name);

        ISequenceCapturedAtom<T> ISequenceUnnamedCapturedAtom<T>.Called(string name)
            => called(name);

        public IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        IAdditionUnnamedCapturedAtom<T> IAdditionAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        IAdditionUnnamedCapturedAtom<T> IAdditionGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        IAlternativeUnnamedCapturedAtom<T> IAlternativeAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        IAlternativeUnnamedCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => capturingGroup(out captureReference);

        public ICapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        IAdditionCapturedAtom<T> IAdditionAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        IAdditionCapturedAtom<T> IAdditionGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        IAlternativeCapturedAtom<T> IAlternativeAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        IAlternativeCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        ISequenceCapturedAtom<T> ISequenceAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        ISequenceCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => capturingGroup(captureReference);

        public IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            atom = repeat;

            return repeat;
        }

        public IGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            atom = group;

            return group;
        }

        public IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            atom = repeat;

            return repeat;
        }

        protected override Generex<T> finishInternal()
        {
            var nested = finishInternal(atom);

            if (atomic)
                nested = new Atoms.AtomicGroup<T>(nested);

            if (nonCapturing)
                return new Atoms.NonCapturingGroup<T>(nested);

            if (captureReference != null)
                return new Atoms.CapturingGroup<T>(nested, captureReference);

            return nested;
        }

        private Grouping<T> called(string name)
        {
            captureReference!.Name = name;
            return this;
        }

        private Grouping<T> capturingGroup(out CaptureReference<T> captureReference)
        {
            captureReference = new CaptureReference<T>();
            this.captureReference = captureReference;

            return this;
        }

        private Grouping<T> capturingGroup(CaptureReference<T> captureReference)
        {
            this.captureReference = captureReference;
            return this;
        }
    }
}