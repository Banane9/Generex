using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        IGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
        IRepeatStart<T> GreedilyRepeat { get; }
        IRepeatStart<T> LazilyRepeat { get; }
    }

    public interface ICapturedAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface IFinishableAtom<T>
    {
        Generex<T> Finish();
    }

    public interface IRepeatedAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        IGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface IUnnamedCapturedAtom<T> : ICapturedAtom<T>
    {
        ICapturedAtom<T> Called(string name);
    }

    internal abstract class Atom<T> : IAtom<T>, ICapturedAtom<T>, IRepeatedAtom<T>,
        IAlternativeAtom<T>, IAlternativeCapturedAtom<T>, IAlternativeRepeatedAtom<T>,
        ISequenceAtom<T>, ISequenceCapturedAtom<T>, ISequenceRepeatedAtom<T>
    {
        private IParentAtom<T>? parent;

        public IAlternativeNext<T> Alternatively => AlternativeParent;

        public IGroup<T> As
        {
            get
            {
                var group = new Group<T>(null, this);
                parent = group;

                return group;
            }
        }

        IAlternativeGroup<T> IAlternativeRepeatedAtom<T>.As => AlternativeParent.WrapInGroup(this);

        ISequenceGroup<T> ISequenceRepeatedAtom<T>.As => SequenceParent.WrapInGroup(this);

        IAlternativeGroup<T> IAlternativeAtom<T>.As => AlternativeParent.WrapInGroup(this);
        ISequenceGroup<T> ISequenceAtom<T>.As => SequenceParent.WrapInGroup(this);
        public ISequenceNext<T> FollowedBy => SequenceParent;

        public IRepeatStart<T> GreedilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, false);
                parent = repeat;

                return repeat;
            }
        }

        ISequenceRepeatStart<T> ISequenceAtom<T>.GreedilyRepeat
            => SequenceParent.WrapInGreedyRepeat(this);

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.GreedilyRepeat
            => AlternativeParent.WrapInGreedyRepeat(this);

        public IRepeatStart<T> LazilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, true);
                parent = repeat;

                return repeat;
            }
        }

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.LazilyRepeat
            => AlternativeParent.WrapInLazyRepeat(this);

        ISequenceRepeatStart<T> ISequenceAtom<T>.LazilyRepeat
            => SequenceParent.WrapInLazyRepeat(this);

        private IAlternativeParentAtom<T> AlternativeParent
        {
            get
            {
                parent ??= new Alternative<T>(this);
                return (IAlternativeParentAtom<T>)parent;
            }
        }

        private ISequenceParentAtom<T> SequenceParent
        {
            get
            {
                parent ??= new Sequence<T>(this);
                return (ISequenceParentAtom<T>)parent;
            }
        }

        protected Atom(IParentAtom<T>? parent)
        {
            this.parent = parent;
        }

        public Generex<T> Finish()
        {
            var current = this;
            while (current.parent != null)
                current = (Atom<T>)current.parent;

            return current.FinishInternal();
        }

        protected static Generex<T> FinishInternal(IFinishableAtom<T> atom)
            => ((Atom<T>)atom).FinishInternal();

        protected static void SetParent(IFinishableAtom<T> atom, IParentAtom<T> parent)
            => ((Atom<T>)atom).parent = parent;

        protected abstract Generex<T> FinishInternal();
    }

    internal interface IParentAtom<T> : IFinishableAtom<T>
    {
        IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        IGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }
}