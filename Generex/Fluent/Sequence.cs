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
        ISequenceRepeatStart<T> GreedilyRepeat { get; }
        ISequenceRepeatStart<T> LazilyRepeat { get; }
    }

    public interface ISequenceCapturedAtom<T> : IFinishableAtom<T>
    {
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface ISequenceNext<T>
    {
        ISequenceCapturedGroupStart<T> CapturedGroup { get; }
        ISequenceLiteralStart<T> Literal { get; }
        ISequenceRangeStart<T> Range { get; }
        ISequenceAtom<T> Wildcard { get; }
    }

    public interface ISequenceRepeatedAtom<T> : IFinishableAtom<T>
    {
        ISequenceGroup<T> As { get; }
        ISequenceNext<T> FollowedBy { get; }
    }

    public interface ISequenceUnnamedCapturedAtom<T> : ISequenceCapturedAtom<T>
    {
        ISequenceCapturedAtom<T> Called(string name);
    }

    internal interface ISequenceParentAtom<T> : ISequenceNext<T>, IFinishableAtom<T>
    {
        ISequenceRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        ISequenceGroup<T> WrapInGroup(IFinishableAtom<T> child);

        ISequenceRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }

    internal class Sequence<T> : Atom<T>, IParentAtom<T>, ISequenceParentAtom<T>
    {
        private readonly List<IFinishableAtom<T>> atoms = new();

        public ISequenceCapturedGroupStart<T> CapturedGroup
        {
            get
            {
                var capturedGroup = new CapturedGroup<T>(this);
                atoms.Add(capturedGroup);
                return capturedGroup;
            }
        }

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

        public ISequenceAtom<T> Wildcard
        {
            get
            {
                var wildcard = new Wildcard<T>(this);
                atoms.Add(wildcard);
                return wildcard;
            }
        }

        public Sequence(IFinishableAtom<T> atom) : base(null)
        {
            atoms.Add(atom);
        }

        public void Add(IFinishableAtom<T> atom) => atoms.Add(atom);

        public void AddRange(IEnumerable<IFinishableAtom<T>> atoms)
            => this.atoms.AddRange(atoms);

        public ISequenceRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child, false);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInGreedyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInGreedyRepeat(child);

        public ISequenceGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var group = new Grouping<T>(this, child);
            atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(IFinishableAtom<T> child)
            => (IGroup<T>)WrapInGroup(child);

        public ISequenceRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var index = atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child, true);
            atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInLazyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInLazyRepeat(child);

        protected override Generex<T> FinishInternal()
        {
            if (atoms.Count == 1)
                return FinishInternal(atoms[0]);

            return new Atoms.Sequence<T>(atoms.Select(FinishInternal));
        }
    }
}