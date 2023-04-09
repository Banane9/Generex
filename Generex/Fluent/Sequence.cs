using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    public interface ISequenceAtom<T> : IFinishableAtom<T>
    {
        ISequenceGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
        ISequenceRepeatStart<T> Repeat { get; }
    }

    public interface ISequenceCapturedAtom<T> : IFinishableAtom<T>
    {
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface ISequenceNext<T>
    {
        public ISequenceLiteralStart<T> Literal { get; }
        public ISequenceRangeStart<T> Range { get; }
    }

    public interface ISequenceRepeatedAtom<T> : IFinishableAtom<T>
    {
        ISequenceGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
    }

    internal interface ISequenceParentAtom<T> : ISequenceNext<T>, IFinishableAtom<T>
    {
        ISequenceGroup<T> WrapInGroup(IFinishableAtom<T> child);

        ISequenceRepeatStart<T> WrapInRepeat(IFinishableAtom<T> child);
    }

    internal class Sequence<T> : Atom<T>, IParentAtom<T>, ISequenceParentAtom<T>
    {
        private readonly List<IFinishableAtom<T>> atoms = new();

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

        public Sequence(IFinishableAtom<T> atom) : base(null)
        {
            atoms.Add(atom);
        }

        public void Add(IFinishableAtom<T> atom) => atoms.Add(atom);

        public void AddRange(IEnumerable<IFinishableAtom<T>> atoms) => this.atoms.AddRange(atoms);

        public ISequenceGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var group = new Group<T>(this, child);
            atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(IFinishableAtom<T> child) => (IGroup<T>)WrapInGroup(child);

        public ISequenceRepeatStart<T> WrapInRepeat(IFinishableAtom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInRepeat(IFinishableAtom<T> child) => (IRepeatStart<T>)WrapInRepeat(child);

        protected override Generex<T> FinishInternal()
        {
            if (atoms.Count == 1)
                return FinishInternal(atoms[0]);

            return new Atoms.Sequence<T>(atoms.Select(FinishInternal));
        }
    }
}