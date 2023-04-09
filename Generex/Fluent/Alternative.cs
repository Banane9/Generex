using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeAtom<T> : IAlternativeCapturedAtom<T>, IAlternativeRepeatedAtom<T>
    {
        IAlternativeRepeatStart<T> Repeat { get; }
    }

    public interface IAlternativeCapturedAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
    }

    public interface IAlternativeNext<T>
    {
        public IAlternativeLiteralStart<T> Literal { get; }
        public IAlternativeRangeStart<T> Range { get; }
    }

    public interface IAlternativeRepeatedAtom<T>
    {
        IAlternativeNext<T> Alternatively { get; }
        IAlternativeGroup<T> As { get; }
    }

    internal class Alternative<T> : Atom<T>, IParentAtom<T>, IAlternativeParentAtom<T>
    {
        private readonly List<Atom<T>> atoms = new();

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

        public Alternative(Atom<T> atom) : base(null)
        {
            atoms.Add(atom);
        }

        public IAlternativeGroup<T> WrapInGroup(Atom<T> child)
        {
            var group = new Group<T>(this, child);
            SetParent(child, group);

            var index = atoms.LastIndexOf(child);
            atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(Atom<T> child) => (IGroup<T>)WrapInGroup(child);

        public IAlternativeRepeatStart<T> WrapInRepeat(Atom<T> child)
        {
            var repeat = new Repeat<T>(this, child);
            SetParent(child, repeat);

            var index = atoms.LastIndexOf(child);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInRepeat(Atom<T> child) => (IRepeatStart<T>)WrapInRepeat(child);
    }

    internal interface IAlternativeParentAtom<T> : IAlternativeNext<T>
    {
        IAlternativeGroup<T> WrapInGroup(Atom<T> child);

        IAlternativeRepeatStart<T> WrapInRepeat(Atom<T> child);
    }
}