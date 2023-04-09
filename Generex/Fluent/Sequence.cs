using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    public interface ISequenceAtom<T> : ISequenceCapturedAtom<T>, ISequenceRepeatedAtom<T>
    {
        ISequenceRepeatStart<T> Repeat { get; }
    }

    public interface ISequenceCapturedAtom<T>
    {
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface ISequenceNext<T>
    {
        public ISequenceLiteralStart<T> Literal { get; }
        public ISequenceRangeStart<T> Range { get; }
    }

    public interface ISequenceRepeatedAtom<T>
    {
        ISequenceGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
    }

    internal interface ISequenceParentAtom<T> : ISequenceNext<T>
    {
        ISequenceGroup<T> WrapInGroup(Atom<T> child);

        ISequenceRepeatStart<T> WrapInRepeat(Atom<T> child);
    }

    internal class Sequence<T> : Atom<T>, IParentAtom<T>, ISequenceParentAtom<T>
    {
        private readonly List<IAtom<T>> atoms = new();

        public ISequenceLiteralStart<T> Literal
        {
            get
            {
                var literal = new Literal<T>(this);
                atoms.Add(literal);
                return literal;
            }
        }

        public ISequenceRangeStart<T> Range
        {
            get
            {
                var range = new Range<T>(this);
                atoms.Add(range);
                return range;
            }
        }

        public Sequence(IAtom<T> atom) : base(null)
        {
            atoms.Add(atom);
        }

        public void Add(IAtom<T> atom) => atoms.Add(atom);

        public void AddRange(IEnumerable<IAtom<T>> atoms) => this.atoms.AddRange(atoms);

        public ISequenceGroup<T> WrapInGroup(Atom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var group = new Group<T>(this, child);
            atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(Atom<T> child) => (IGroup<T>)WrapInGroup(child);

        public ISequenceRepeatStart<T> WrapInRepeat(Atom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInRepeat(Atom<T> child) => (IRepeatStart<T>)WrapInRepeat(child);
    }
}