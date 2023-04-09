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
        IRepeatStart<T> Repeat { get; }
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

        public IRepeatStart<T> Repeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this);
                parent = repeat;

                return repeat;
            }
        }

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.Repeat => AlternativeParent.WrapInRepeat(this);

        ISequenceRepeatStart<T> ISequenceAtom<T>.Repeat => SequenceParent.WrapInRepeat(this);

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

        public abstract Generex<T> Finish();

        protected static void SetParent(IFinishableAtom<T> atom, IParentAtom<T> parent) => ((Atom<T>)atom).parent = parent;
    }

    internal interface IParentAtom<T>
    {
        IGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IRepeatStart<T> WrapInRepeat(IFinishableAtom<T> child);
    }
}