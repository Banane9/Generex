using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        IAlternativeGroup<T> As { get; }
        IAlternativeRepeatStart<T> GreedilyRepeat { get; }
        IAlternativeRepeatStart<T> LazilyRepeat { get; }
    }

    public interface IAlternativeCapturedAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
    }

    public interface IAlternativeNext<T>
    {
        public IAlternativeCapturedGroupStart<T> CapturedGroup { get; }
        public IAlternativeLiteralStart<T> Literal { get; }
        public IAlternativeRangeStart<T> Range { get; }
        public IAlternativeAtom<T> Wildcard { get; }
    }

    public interface IAlternativeRepeatedAtom<T> : IFinishableAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        IAlternativeGroup<T> As { get; }
    }

    public interface IAlternativeUnnamedCapturedAtom<T> : IAlternativeCapturedAtom<T>
    {
        IAlternativeCapturedAtom<T> Called(string name);
    }

    internal class Alternative<T> : Atom<T>, IParentAtom<T>, IAlternativeParentAtom<T>
    {
        private readonly List<IFinishableAtom<T>> atoms = new();

        public IAlternativeCapturedGroupStart<T> CapturedGroup
        {
            get
            {
                var capturedGroup = new CapturedGroup<T>(this);
                atoms.Add(capturedGroup);
                return capturedGroup;
            }
        }

        public IAlternativeLiteralStart<T> Literal
        {
            get
            {
                var literal = new Literal<T>(this);
                atoms.Add(literal);
                return literal;
            }
        }

        public IAlternativeRangeStart<T> Range
        {
            get
            {
                var range = new Range<T>(this);
                atoms.Add(range);
                return range;
            }
        }

        public IAlternativeAtom<T> Wildcard
        {
            get
            {
                var wildcard = new Wildcard<T>(this);
                atoms.Add(wildcard);
                return wildcard;
            }
        }

        public Alternative(IFinishableAtom<T> atom) : base(null)
        {
            atoms.Add(atom);
        }

        public IAlternativeRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            SetParent(child, repeat);

            var index = atoms.LastIndexOf(child);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInGreedyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInGreedyRepeat(child);

        public IAlternativeGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            SetParent(child, group);

            var index = atoms.LastIndexOf(child);
            atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(IFinishableAtom<T> child)
            => (IGroup<T>)WrapInGroup(child);

        IRepeatStart<T> IParentAtom<T>.WrapInLazyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInLazyRepeat(child);

        public IAlternativeRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            SetParent(child, repeat);

            var index = atoms.LastIndexOf(child);
            atoms[index] = repeat;

            return repeat;
        }

        protected override Generex<T> FinishInternal()
        {
            if (atoms.Count == 1)
                return FinishInternal(atoms[0]);

            return new Atoms.Alternative<T>(atoms.Select(FinishInternal));
        }
    }

    internal interface IAlternativeParentAtom<T> : IAlternativeNext<T>, IFinishableAtom<T>
    {
        IAlternativeRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        IAlternativeGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IAlternativeRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }
}