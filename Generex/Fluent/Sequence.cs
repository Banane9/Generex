﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a general atom, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Construct a group wrapper for the current atom.
        /// </summary>
        ISequenceGroup<T> As { get; }

        /// <summary>
        /// Add the next atom to the sequence.
        /// </summary>
        ISequenceNext<T> FollowedBy { get; }

        /// <summary>
        /// Construct a greedy quantifier for the current atom.
        /// </summary>

        ISequenceRepeatStart<T> GreedilyRepeat { get; }

        /// <summary>
        /// Construct a lazy quantifier for the current atom.
        /// </summary>
        ISequenceRepeatStart<T> LazilyRepeat { get; }
    }

    /// <summary>
    /// The options for a named capture group, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceCapturedAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Adds the next atom to the sequence.
        /// </summary>
        ISequenceNext<T> FollowedBy { get; }
    }

    /// <summary>
    /// The options for the next atom in a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceNext<T>
    {
        /// <summary>
        /// Constructs a back-reference to the latest capture of another capturing group,
        /// which values have to match one by one.
        /// </summary>
        ISequenceCapturedGroupStart<T> CapturedGroup { get; }

        /// <summary>
        /// Constructs a sequence of literals, which values have to match one by one.
        /// </summary>
        ISequenceLiteralStart<T> Literal { get; }

        /// <summary>
        /// Construct a negated range of literals, of which a value has to match none.
        /// </summary>
        public ISequenceRangeStart<T> NegatedRange { get; }

        /// <summary>
        /// Constructs a range of literals, of which a value has to match at least one.
        /// </summary>
        ISequenceRangeStart<T> Range { get; }

        /// <summary>
        /// Adds a wildcard to the sequence, which matches any value.
        /// </summary>
        ISequenceAtom<T> Wildcard { get; }
    }

    /// <summary>
    /// The options for a not yet explicitly named capture group, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceUnnamedCapturedAtom<T> : ISequenceCapturedAtom<T>
    {
        /// <summary>
        /// Addss a name to the capture group.
        /// </summary>
        /// <param name="name">The name to associate with the group.</param>
        /// <returns>The named capture group.</returns>
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
        private readonly List<IFinishableAtom<T>> _atoms = [];

        public ISequenceCapturedGroupStart<T> CapturedGroup
        {
            get
            {
                var capturedGroup = new CapturedGroup<T>(this);
                _atoms.Add(capturedGroup);
                return capturedGroup;
            }
        }

        public ISequenceLiteralStart<T> Literal
        {
            get
            {
                var literal = new Literal<T>(this);
                _atoms.Add(literal);
                return literal;
            }
        }

        public ISequenceRangeStart<T> NegatedRange
        {
            get
            {
                var range = new Range<T>(true, this);
                _atoms.Add(range);
                return range;
            }
        }

        public ISequenceRangeStart<T> Range
        {
            get
            {
                var range = new Range<T>(false, this);
                _atoms.Add(range);
                return range;
            }
        }

        public ISequenceAtom<T> Wildcard
        {
            get
            {
                var wildcard = new Wildcard<T>(this);
                _atoms.Add(wildcard);
                return wildcard;
            }
        }

        public Sequence(IFinishableAtom<T> atom) : base(null)
        {
            _atoms.Add(atom);
        }

        public void Add(IFinishableAtom<T> atom) => _atoms.Add(atom);

        public void AddRange(IEnumerable<IFinishableAtom<T>> atoms)
            => _atoms.AddRange(atoms);

        public ISequenceRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var index = _atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child, false);
            _atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInGreedyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInGreedyRepeat(child);

        public ISequenceGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var index = _atoms.LastIndexOf(child);
            var group = new Grouping<T>(this, child);
            _atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(IFinishableAtom<T> child)
            => (IGroup<T>)WrapInGroup(child);

        public ISequenceRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var index = _atoms.LastIndexOf(child);
            var repeat = new Repeat<T>(this, child, true);
            _atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInLazyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInLazyRepeat(child);

        protected override Generex<T> FinishInternal()
        {
            if (_atoms.Count == 1)
                return FinishInternal(_atoms[0]);

            return new Atoms.Sequence<T>(_atoms.Select(FinishInternal));
        }
    }
}