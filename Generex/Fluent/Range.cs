using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeOpenRange<T>
    {
        IAlternativeRange<T> To(T maximum);
    }

    public interface IAlternativeRange<T> : IAlternativeAtom<T>
    {
        IAlternativeRangeAddition<T> And { get; }
    }

    public interface IAlternativeRangeAddition<T>
    {
        IAlternativeOpenRange<T> From(T minimum);

        IAlternativeRange<T> With(T exactly);
    }

    public interface IAlternativeRangeStart<T>
    {
        IAlternativeOpenRange<T> From(T minium);

        IAlternativeRange<T> Of(T literal);

        IAlternativeRangeAddition<T> Using(IComparer<T> comparer);
    }

    public interface IOpenRange<T>
    {
        IRange<T> To(T maximum);
    }

    public interface IRange<T> : IAtom<T>
    {
        IRangeAddition<T> And { get; }
    }

    public interface IRangeAddition<T>
    {
        IOpenRange<T> From(T minimum);

        IRange<T> With(T exactly);
    }

    public interface IRangeStart<T>
    {
        IOpenRange<T> From(T minium);

        IRange<T> Of(T literal);

        IRangeAddition<T> Using(IComparer<T> comparer);
    }

    public interface ISequenceOpenRange<T>
    {
        ISequenceRange<T> To(T maximum);
    }

    public interface ISequenceRange<T> : ISequenceAtom<T>
    {
        ISequenceRangeAddition<T> And { get; }
    }

    public interface ISequenceRangeAddition<T>
    {
        ISequenceOpenRange<T> From(T minimum);

        ISequenceRange<T> With(T exactly);
    }

    public interface ISequenceRangeStart<T>
    {
        ISequenceOpenRange<T> From(T minium);

        ISequenceRange<T> Of(T literal);

        ISequenceRangeAddition<T> Using(IComparer<T> comparer);
    }

    internal class Range<T> : Atom<T>, IRangeStart<T>, IRange<T>, IOpenRange<T>, IRangeAddition<T>,
        IAlternativeRangeStart<T>, IAlternativeRange<T>, IAlternativeOpenRange<T>, IAlternativeRangeAddition<T>,
        ISequenceRangeStart<T>, ISequenceRange<T>, ISequenceOpenRange<T>, ISequenceRangeAddition<T>
    {
        private readonly List<T> literals = new();
        private readonly List<LiteralRange<T>> ranges = new();
        private T? capturedMinium;
        private IComparer<T> comparer = Comparer<T>.Default;

        public IRangeAddition<T> And => this;

        IAlternativeRangeAddition<T> IAlternativeRange<T>.And => this;
        ISequenceRangeAddition<T> ISequenceRange<T>.And => this;

        public Range(IParentAtom<T>? parentSequence = null) : base(parentSequence)
        { }

        public override Generex<T> Finish()
            => new Atoms.Range<T>(ranges);

        public IOpenRange<T> From(T minimum)
        {
            capturedMinium = minimum;
            return this;
        }

        IAlternativeOpenRange<T> IAlternativeRangeStart<T>.From(T minium)
            => (IAlternativeOpenRange<T>)From(minium);

        IAlternativeOpenRange<T> IAlternativeRangeAddition<T>.From(T minimum)
            => (IAlternativeOpenRange<T>)From(minimum);

        ISequenceOpenRange<T> ISequenceRangeStart<T>.From(T minium)
            => (ISequenceOpenRange<T>)From(minium);

        ISequenceOpenRange<T> ISequenceRangeAddition<T>.From(T minimum)
            => (ISequenceOpenRange<T>)From(minimum);

        public IRange<T> Of(T literal) => With(literal);

        IAlternativeRange<T> IAlternativeRangeStart<T>.Of(T literal)
            => (IAlternativeRange<T>)Of(literal);

        ISequenceRange<T> ISequenceRangeStart<T>.Of(T literal)
            => (ISequenceRange<T>)Of(literal);

        public IRange<T> To(T maximum)
        {
            ranges.Add(new LiteralRange<T>(capturedMinium!, maximum, comparer));
            capturedMinium = default;

            return this;
        }

        IAlternativeRange<T> IAlternativeOpenRange<T>.To(T maximum)
            => (IAlternativeRange<T>)To(maximum);

        ISequenceRange<T> ISequenceOpenRange<T>.To(T maximum)
            => (ISequenceRange<T>)To(maximum);

        public IRangeAddition<T> Using(IComparer<T> comparer)
        {
            this.comparer = comparer;
            return this;
        }

        IAlternativeRangeAddition<T> IAlternativeRangeStart<T>.Using(IComparer<T> comparer)
            => (IAlternativeRangeAddition<T>)Using(comparer);

        ISequenceRangeAddition<T> ISequenceRangeStart<T>.Using(IComparer<T> comparer)
            => (ISequenceRangeAddition<T>)Using(comparer);

        public IRange<T> With(T exactly)
        {
            literals.Add(exactly);
            return this;
        }

        IAlternativeRange<T> IAlternativeRangeAddition<T>.With(T exactly)
            => (IAlternativeRange<T>)With(exactly);

        ISequenceRange<T> ISequenceRangeAddition<T>.With(T exactly)
            => (ISequenceRange<T>)With(exactly);
    }
}