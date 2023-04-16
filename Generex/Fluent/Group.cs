using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeGroup<T>
    {
        IAlternativeAtom<T> NonCapturingGroup { get; }

        IAlternativeUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface IGroup<T>
    {
        IAtom<T> NonCapturingGroup { get; }

        IUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    public interface ISequenceGroup<T>
    {
        ISequenceAtom<T> NonCapturingGroup { get; }

        ISequenceUnnamedCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference);
    }

    internal class Group<T> : Atom<T>, IParentAtom<T>, IGroup<T>, IUnnamedCapturedAtom<T>,
        IAlternativeGroup<T>, IAlternativeUnnamedCapturedAtom<T>,
        ISequenceGroup<T>, ISequenceUnnamedCapturedAtom<T>
    {
        private IFinishableAtom<T> atom;
        private CaptureReference<T>? captureReference;
        public IAtom<T> NonCapturingGroup => this;

        IAlternativeAtom<T> IAlternativeGroup<T>.NonCapturingGroup => this;
        ISequenceAtom<T> ISequenceGroup<T>.NonCapturingGroup => this;

        public Group(IParentAtom<T>? parent, IFinishableAtom<T> atom) : base(parent)
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
            var group = new Group<T>(this, child);
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
            if (captureReference == null)
                return new Atoms.NonCapturingGroup<T>(FinishInternal(atom));

            return new Atoms.CapturingGroup<T>(FinishInternal(atom), captureReference);
        }
    }
}