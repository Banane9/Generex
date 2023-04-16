using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeAtomicGroup<T>
    {
        IAlternativeAtom<T> Group { get; }
        IAlternativeAtom<T> NonCapturingGroup { get; }

        IAlternativeUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface IAlternativeGroup<T>
    {
        IAlternativeAtomicGroup<T> Atomic { get; }
        IAlternativeAtom<T> NonCapturingGroup { get; }

        IAlternativeUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface IAtomicGroup<T>
    {
        IAtom<T> Group { get; }
        IAtom<T> NonCapturingGroup { get; }

        IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface IGroup<T>
    {
        IAtomicGroup<T> Atomic { get; }
        IAtom<T> NonCapturingGroup { get; }

        IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface ISequenceAtomicGroup<T>
    {
        ISequenceAtom<T> Group { get; }
        ISequenceAtom<T> NonCapturingGroup { get; }

        ISequenceUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface ISequenceGroup<T>
    {
        ISequenceAtomicGroup<T> Atomic { get; }
        ISequenceAtom<T> NonCapturingGroup { get; }

        ISequenceUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    internal class Grouping<T> : Atom<T>, IParentAtom<T>, IAtomicGroup<T>, IGroup<T>, IUnnamedCapturedAtom<T>,
        IAlternativeAtomicGroup<T>, IAlternativeGroup<T>, IAlternativeUnnamedCapturedAtom<T>,
        ISequenceAtomicGroup<T>, ISequenceGroup<T>, ISequenceUnnamedCapturedAtom<T>
    {
        private IFinishableAtom<T> atom;
        private bool atomic;
        private CaptureReference<T>? captureReference;
        private bool nonCapturing;

        public IAtomicGroup<T> Atomic
        {
            get
            {
                atomic = true;
                return this;
            }
        }

        IAlternativeAtomicGroup<T> IAlternativeGroup<T>.Atomic => (IAlternativeAtomicGroup<T>)Atomic;
        ISequenceAtomicGroup<T> ISequenceGroup<T>.Atomic => (ISequenceAtomicGroup<T>)Atomic;
        public IAtom<T> Group => this;
        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.Group => this;

        ISequenceAtom<T> ISequenceAtomicGroup<T>.Group => this;

        public IAtom<T> NonCapturingGroup
        {
            get
            {
                nonCapturing = true;
                return this;
            }
        }

        IAlternativeAtom<T> IAlternativeAtomicGroup<T>.NonCapturingGroup => (IAlternativeAtom<T>)NonCapturingGroup;
        ISequenceAtom<T> ISequenceAtomicGroup<T>.NonCapturingGroup => (ISequenceAtom<T>)NonCapturingGroup;

        IAlternativeAtom<T> IAlternativeGroup<T>.NonCapturingGroup => (IAlternativeAtom<T>)NonCapturingGroup;
        ISequenceAtom<T> ISequenceGroup<T>.NonCapturingGroup => (ISequenceAtom<T>)NonCapturingGroup;

        public Grouping(IParentAtom<T>? parent, IFinishableAtom<T> atom) : base(parent)
        {
            this.atom = atom;
        }

        public ICapturedAtom<T> Called(string name)
        {
            captureReference!.Name = name;
            return this;
        }

        IAlternativeCapturedAtom<T> IAlternativeUnnamedCapturedAtom<T>.Called(string name)
            => (IAlternativeCapturedAtom<T>)Called(name);

        ISequenceCapturedAtom<T> ISequenceUnnamedCapturedAtom<T>.Called(string name)
            => (ISequenceCapturedAtom<T>)Called(name);

        public IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference)
        {
            captureReference = new CaptureReference<T>();
            this.captureReference = captureReference;

            return this;
        }

        IAlternativeUnnamedCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => (IAlternativeUnnamedCapturedAtom<T>)CapturingGroup(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => (ISequenceUnnamedCapturedAtom<T>)CapturingGroup(out captureReference);

        IAlternativeUnnamedCapturedAtom<T> IAlternativeAtomicGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => (IAlternativeUnnamedCapturedAtom<T>)CapturingGroup(out captureReference);

        ISequenceUnnamedCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(out CaptureReference<T> captureReference)
            => (ISequenceUnnamedCapturedAtom<T>)CapturingGroup(out captureReference);

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

        protected override Generex<T> FinishInternal()
        {
            var nested = FinishInternal(atom);

            if (atomic)
                nested = new Atoms.AtomicGroup<T>(nested);

            if (nonCapturing)
                return new Atoms.NonCapturingGroup<T>(nested);

            if (captureReference != null)
                return new Atoms.CapturingGroup<T>(nested, captureReference);

            return nested;
        }
    }
}