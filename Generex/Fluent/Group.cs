using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeGroup<T>
    {
        IAlternativeAtom<T> NonCapturingGroup { get; }

        IAlternativeCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference, string? name);
    }

    public interface IGroup<T>
    {
        IAtom<T> NonCapturingGroup { get; }

        ICapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference, string? name);
    }

    public interface ISequenceGroup<T>
    {
        ISequenceAtom<T> NonCapturingGroup { get; }

        ISequenceCapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference, string? name);
    }

    internal class Group<T> : Atom<T>, IParentAtom<T>, IGroup<T>, IAlternativeGroup<T>, ISequenceGroup<T>
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

        public ICapturedAtom<T> CapturingGroup(out CaptureReference<T> captureReference, string? name)
        {
            captureReference = new CaptureReference<T>(name);
            this.captureReference = captureReference;

            return this;
        }

        IAlternativeCapturedAtom<T> IAlternativeGroup<T>.CapturingGroup(out CaptureReference<T> captureReference, string? name)
            => (IAlternativeCapturedAtom<T>)CapturingGroup(out captureReference, name);

        ISequenceCapturedAtom<T> ISequenceGroup<T>.CapturingGroup(out CaptureReference<T> captureReference, string? name)
            => (ISequenceCapturedAtom<T>)CapturingGroup(out captureReference, name);

        public IGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Group<T>(this, child);
            atom = group;

            return group;
        }

        public IRepeatStart<T> WrapInRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child);
            atom = repeat;

            return repeat;
        }

        protected override Generex<T> FinishInternal()
        {
            if (captureReference == null)
                return new Atoms.NonCapturingGroup<T>(FinishInternal(atom));

            return new Atoms.CapturingGroup<T>(captureReference, FinishInternal(atom));
        }
    }
}