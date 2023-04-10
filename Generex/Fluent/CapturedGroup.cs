using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeCapturedGroupEnd<T> : IAlternativeAtom<T>
    {
        IAlternativeAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    public interface IAlternativeCapturedGroupStart<T>
    {
        IAlternativeCapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    public interface ICapturedGroupEnd<T> : IAtom<T>
    {
        IAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    public interface ICapturedGroupStart<T>
    {
        ICapturedGroupEnd<T> ReferringBackTo(CaptureReference<T> captureReference);
    }

    public interface ISequenceCapturedGroupEnd<T> : ISequenceAtom<T>
    {
        ISequenceAtom<T> Using(IEqualityComparer<T> equalityComparer);
    }

    public interface ISequenceCapturedGroupStart<T>
    {
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