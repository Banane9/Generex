using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a general atom, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Add another atom to the list of requirements.
        /// </summary>
        IAdditionNext<T> Additionally { get; }

        /// <summary>
        /// Construct a group wrapper for the current atom.
        /// </summary>
        IAdditionGroup<T> As { get; }

        /// <summary>
        /// Construct a greedy quantifier for the current atom.
        /// </summary>
        IAdditionRepeatStart<T> GreedilyRepeat { get; }

        /// <summary>
        /// Construct a lazy quantifier for the current atom.
        /// </summary>
        IAdditionRepeatStart<T> LazilyRepeat { get; }
    }

    /// <summary>
    /// The options for a named capture group, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionCapturedAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Add another atom to the list of requirements.
        /// </summary>
        IAdditionNext<T> Additionally { get; }
    }

    /// <summary>
    /// The options for the next atom in a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionNext<T>
    {
        /// <summary>
        /// Construct a back-reference to the latest capture of another capturing group,
        /// which values have to match one by one.
        /// </summary>
        public IAdditionCapturedGroupStart<T> CapturedGroup { get; }

        /// <summary>
        /// Construct a sequence of literals, which values have to match one by one.
        /// </summary>
        public IAdditionLiteralStart<T> Literal { get; }

        /// <summary>
        /// Construct a negated range of literals, of which a value has to match none.
        /// </summary>
        public IAdditionRangeStart<T> NegatedRange { get; }

        /// <summary>
        /// Construct a range of literals, of which a value has to match at least one.
        /// </summary>
        public IAdditionRangeStart<T> Range { get; }

        /// <summary>
        /// Add a wildcard to the list of requirements, which matches any value.
        /// </summary>
        public IAdditionAtom<T> Wildcard { get; }
    }

    /// <summary>
    /// The options for a not yet explicitly named capture group, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionUnnamedCapturedAtom<T> : IAdditionCapturedAtom<T>
    {
        /// <summary>
        /// Adds a name to the capture group.
        /// </summary>
        /// <param name="name">The name to associate with the group.</param>
        /// <returns>The named capture group.</returns>
        IAdditionCapturedAtom<T> Called(string name);
    }

    internal class Addition<T> : Atom<T>, IParentAtom<T>, IAdditionParentAtom<T>
    {
        private readonly List<IFinishableAtom<T>> _atoms = [];

        public IAdditionCapturedGroupStart<T> CapturedGroup
        {
            get
            {
                var capturedGroup = new CapturedGroup<T>(this);
                _atoms.Add(capturedGroup);
                return capturedGroup;
            }
        }

        public IAdditionLiteralStart<T> Literal
        {
            get
            {
                var literal = new Literal<T>(this);
                _atoms.Add(literal);
                return literal;
            }
        }

        public IAdditionRangeStart<T> NegatedRange
        {
            get
            {
                var range = new Range<T>(true, this);
                _atoms.Add(range);
                return range;
            }
        }

        public IAdditionRangeStart<T> Range
        {
            get
            {
                var range = new Range<T>(false, this);
                _atoms.Add(range);
                return range;
            }
        }

        public IAdditionAtom<T> Wildcard
        {
            get
            {
                var wildcard = new Wildcard<T>(this);
                _atoms.Add(wildcard);
                return wildcard;
            }
        }

        public Addition(IFinishableAtom<T> atom) : base(null)
        {
            _atoms.Add(atom);
        }

        public IAdditionRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            SetParent(child, repeat);

            var index = _atoms.LastIndexOf(child);
            _atoms[index] = repeat;

            return repeat;
        }

        IRepeatStart<T> IParentAtom<T>.WrapInGreedyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInGreedyRepeat(child);

        public IAdditionGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            SetParent(child, group);

            var index = _atoms.LastIndexOf(child);
            _atoms[index] = group;

            return group;
        }

        IGroup<T> IParentAtom<T>.WrapInGroup(IFinishableAtom<T> child)
            => (IGroup<T>)WrapInGroup(child);

        IRepeatStart<T> IParentAtom<T>.WrapInLazyRepeat(IFinishableAtom<T> child)
            => (IRepeatStart<T>)WrapInLazyRepeat(child);

        public IAdditionRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            SetParent(child, repeat);

            var index = _atoms.LastIndexOf(child);
            _atoms[index] = repeat;

            return repeat;
        }

        protected override Generex<T> FinishInternal()
        {
            if (_atoms.Count == 1)
                return FinishInternal(_atoms[0]);

            return new Conjunction<T>(_atoms.Select(FinishInternal));
        }
    }

    internal interface IAdditionParentAtom<T> : IAdditionNext<T>, IFinishableAtom<T>
    {
        IAdditionRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        IAdditionGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IAdditionRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }
}