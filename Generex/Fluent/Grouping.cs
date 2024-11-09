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
        private IFinishableAtom<T> _atom;
        private bool _atomic;
        private CaptureReference<T>? _captureReference;
        private bool _nonCapturing;

        public IAtomicGroup<T> Atomic => AtomicGrouping;
        IAdditionAtomicGroup<T> IAdditionGroup<T>.Atomic => AtomicGrouping;
        IAlternativeAtomicGroup<T> IAlternativeGroup<T>.Atomic => AtomicGrouping;
        ISequenceAtomicGroup<T> ISequenceGroup<T>.Atomic => AtomicGrouping;

        public IAtom<T> Group => this;
        IAdditionAtom<T> IAdditionAtomicGroup<T>.Group => this;
        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.Group => this;
        ISequenceAtom<T> ISequenceAtomicGroup<T>.Group => this;

        public IAtom<T> NonCapturingGroup => NonCapturingGrouping;
        IAdditionAtom<T> IAdditionAtomicGroup<T>.NonCapturingGroup => NonCapturingGrouping;
        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.NonCapturingGroup => NonCapturingGrouping;
        ISequenceAtom<T> ISequenceAtomicGroup<T>.NonCapturingGroup => NonCapturingGrouping;
        IAdditionAtom<T> IAdditionGroup<T>.NonCapturingGroup => NonCapturingGrouping;
        ISequenceAtom<T> ISequenceGroup<T>.NonCapturingGroup => NonCapturingGrouping;
        IAlternativeAtom<T> IAlternativeGroup<T>.NonCapturingGroup => NonCapturingGrouping;

        private Grouping<T> AtomicGrouping
        {
            get
            {
                _atomic = true;
                return this;
            }
        }

        private Grouping<T> NonCapturingGrouping
        {
            get
            {
                _nonCapturing = true;
                return this;
            }
        }

        public Grouping(IParentAtom<T>? parent, IFinishableAtom<T> atom) : base(parent)
        {
            _atom = atom;
        }

        IAdditionCapturedAtom<T> IAdditionUnnamedCapturedAtom<T>.Called(string name)
            => CalledInternal(name);

        IAlternativeCapturedAtom<T> IAlternativeUnnamedCapturedAtom<T>.Called(string name)
            => CalledInternal(name);

        ISequenceCapturedAtom<T> ISequenceUnnamedCapturedAtom<T>.Called(string name)
            => CalledInternal(name);

        public ICapturedAtom<T> Called(string name) => CalledInternal(name);

        public IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        IAdditionUnnamedCapturedAtom<T> IAdditionAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        IAdditionUnnamedCapturedAtom<T> IAdditionGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        IAlternativeUnnamedCapturedAtom<T> IAlternativeAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        IAlternativeUnnamedCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => CapturingGrouping(out captureReference);

        public ICapturedAtom<T> CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        IAdditionCapturedAtom<T> IAdditionAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        IAdditionCapturedAtom<T> IAdditionGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        IAlternativeCapturedAtom<T> IAlternativeAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        IAlternativeCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        ISequenceCapturedAtom<T> ISequenceAtomicGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        ISequenceCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(CaptureReference<T> captureReference)
            => CapturingGrouping(captureReference);

        public IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            _atom = repeat;

            return repeat;
        }

        public IGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            _atom = group;

            return group;
        }

        public IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            _atom = repeat;

            return repeat;
        }

        protected override Generex<T> FinishInternal()
        {
            var nested = FinishInternal(_atom);

            if (_atomic)
                nested = new Atoms.AtomicGroup<T>(nested);

            if (_nonCapturing)
                return new Atoms.NonCapturingGroup<T>(nested);

            if (_captureReference != null)
                return new Atoms.CapturingGroup<T>(nested, _captureReference);

            return nested;
        }

        private Grouping<T> CalledInternal(string name)
        {
            _captureReference!.Name = name;
            return this;
        }

        private Grouping<T> CapturingGrouping(CaptureReference<T> captureReference)
        {
            this._captureReference = captureReference;
            return this;
        }

        private Grouping<T> CapturingGrouping(out CaptureReference<T> captureReference)
        {
            captureReference = new CaptureReference<T>();
            this._captureReference = captureReference;

            return this;
        }
    }
}