using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeRepeatEnd<T>
    {
        IAlternativeRepeatedAtom<T> And(int maximum);
    }

    public interface IAlternativeRepeatStart<T>
    {
        public IAlternativeRepeatedAtom<T> Any { get; }

        public IAlternativeRepeatedAtom<T> AtLeastOnce { get; }

        public IAlternativeRepeatedAtom<T> AtMostOnce { get; }

        public IAlternativeRepeatedAtom<T> AtLeast(int minimum);

        public IAlternativeRepeatedAtom<T> AtMost(int maximum);

        public IAlternativeRepeatEnd<T> Between(int minimum);

        public IAlternativeRepeatedAtom<T> Exactly(int times);

        public IAlternativeRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    public interface IRepeatEnd<T>
    {
        IRepeatedAtom<T> And(int maximum);
    }

    public interface IRepeatStart<T>
    {
        public IRepeatedAtom<T> Any { get; }

        public IRepeatedAtom<T> AtLeastOnce { get; }

        public IRepeatedAtom<T> AtMostOnce { get; }

        public IRepeatedAtom<T> AtLeast(int minimum);

        public IRepeatedAtom<T> AtMost(int maximum);

        public IRepeatEnd<T> Between(int minimum);

        public IRepeatedAtom<T> Exactly(int times);

        public IRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    public interface ISequenceRepeatEnd<T>
    {
        ISequenceRepeatedAtom<T> And(int maximum);
    }

    public interface ISequenceRepeatStart<T>
    {
        public ISequenceRepeatedAtom<T> Any { get; }

        public ISequenceRepeatedAtom<T> AtLeastOnce { get; }

        public ISequenceRepeatedAtom<T> AtMostOnce { get; }

        public ISequenceRepeatedAtom<T> AtLeast(int minimum);

        public ISequenceRepeatedAtom<T> AtMost(int maximum);

        public ISequenceRepeatEnd<T> Between(int minimum);

        public ISequenceRepeatedAtom<T> Exactly(int times);

        public ISequenceRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    internal class Repeat<T> : Atom<T>, IParentAtom<T>, IRepeatStart<T>, IRepeatEnd<T>, IAlternativeRepeatStart<T>, IAlternativeRepeatEnd<T>, ISequenceRepeatStart<T>, ISequenceRepeatEnd<T>
    {
        private IFinishableAtom<T> atom;
        private int maximum = -1;
        private int minimum = -1;

        public IRepeatedAtom<T> Any
        {
            get
            {
                minimum = 0;
                maximum = int.MaxValue;
                return this;
            }
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.Any => (IAlternativeRepeatedAtom<T>)Any;

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.Any => (ISequenceRepeatedAtom<T>)Any;

        public IRepeatedAtom<T> AtLeastOnce
        {
            get
            {
                minimum = 1;
                maximum = int.MaxValue;
                return this;
            }
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtLeastOnce => (IAlternativeRepeatedAtom<T>)AtLeastOnce;

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtLeastOnce => (ISequenceRepeatedAtom<T>)AtLeastOnce;

        public IRepeatedAtom<T> AtMostOnce
        {
            get
            {
                minimum = 0;
                maximum = 1;
                return this;
            }
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtMostOnce => (IAlternativeRepeatedAtom<T>)AtMostOnce;
        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtMostOnce => (ISequenceRepeatedAtom<T>)AtMostOnce;

        public Repeat(IParentAtom<T>? parent, IFinishableAtom<T> atom) : base(parent)
        {
            this.atom = atom;
        }

        public IRepeatedAtom<T> And(int maximum)
        {
            this.maximum = maximum;
            return this;
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatEnd<T>.And(int maximum)
            => (IAlternativeRepeatedAtom<T>)And(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatEnd<T>.And(int maximum)
            => (ISequenceRepeatedAtom<T>)And(maximum);

        public IRepeatedAtom<T> AtLeast(int minimum)
        {
            this.minimum = minimum;
            maximum = int.MaxValue;
            return this;
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtLeast(int minimum)
            => (IAlternativeRepeatedAtom<T>)AtLeast(minimum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtLeast(int minimum)
            => (ISequenceRepeatedAtom<T>)AtLeast(minimum);

        public IRepeatedAtom<T> AtMost(int maximum)
        {
            minimum = 1;
            this.maximum = maximum;
            return this;
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtMost(int maximum)
            => (IAlternativeRepeatedAtom<T>)AtMost(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtMost(int maximum)
            => (ISequenceRepeatedAtom<T>)AtMost(maximum);

        public IRepeatEnd<T> Between(int minimum)
        {
            this.minimum = minimum;
            return this;
        }

        IAlternativeRepeatEnd<T> IAlternativeRepeatStart<T>.Between(int minimum)
            => (IAlternativeRepeatEnd<T>)Between(minimum);

        ISequenceRepeatEnd<T> ISequenceRepeatStart<T>.Between(int minimum)
            => (ISequenceRepeatEnd<T>)Between(minimum);

        public IRepeatedAtom<T> Exactly(int times)
        {
            minimum = times;
            maximum = times;
            return this;
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.Exactly(int times)
            => (IAlternativeRepeatedAtom<T>)Exactly(times);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.Exactly(int times)
            => (ISequenceRepeatedAtom<T>)Exactly(times);

        public IRepeatedAtom<T> MaybeAtMost(int maximum)
        {
            minimum = 0;
            this.maximum = maximum;
            return this;
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.MaybeAtMost(int maximum)
            => (IAlternativeRepeatedAtom<T>)MaybeAtMost(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.MaybeAtMost(int maximum)
            => (ISequenceRepeatedAtom<T>)MaybeAtMost(maximum);

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
            => new Quantifier<T>(FinishInternal(atom), minimum, maximum);
    }
}