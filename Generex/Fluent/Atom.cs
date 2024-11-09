using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a general atom,
    /// which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Start a list of requirements with this atom and add another to it.
        /// </summary>
        IAdditionNext<T> Additionally { get; }

        /// <summary>
        /// Start a list of alternatives with this atom and add another to it.
        /// </summary>
        IAlternativeNext<T> Alternatively { get; }

        /// <summary>
        /// Construct a group wrapper for the current atom.
        /// </summary>
        IGroup<T> As { get; }

        /// <summary>
        /// Start a sequence with this atom and add the next one to it.
        /// </summary>
        ISequenceNext<T> FollowedBy { get; }

        /// <summary>
        /// Construct a greedy quantifier for the current atom.
        /// </summary>
        IRepeatStart<T> GreedilyRepeat { get; }

        /// <summary>
        /// Construct a lazy quantifier for the current atom.
        /// </summary>
        IRepeatStart<T> LazilyRepeat { get; }
    }

    /// <summary>
    /// The options for a named capture group,
    /// which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface ICapturedAtom<T> : IFinishableAtom<T>
    {
        /// <summary>
        /// Start a list of requirements with this atom and add another to it.
        /// </summary>
        IAdditionNext<T> Additionally { get; }

        /// <summary>
        /// Start a list of alternatives with this atom and add another to it.
        /// </summary>
        IAlternativeNext<T> Alternatively { get; }

        /// <summary>
        /// Start a sequence with this atom and add the next one to it.
        /// </summary>
        ISequenceNext<T> FollowedBy { get; }
    }

    /// <summary>
    /// The options for a finishable fluent construction.
    /// </summary>
    /// <remarks>
    /// This is a state that the fluent construction can be finished in.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IFinishableAtom<T>
    {
        /// <summary>
        /// Generates a <see cref="Generex{T}"/> pattern representing the current fluent construction.
        /// </summary>
        /// <returns>The pattern representing the fluent construction.</returns>
        Generex<T> Finish();
    }

    /// <summary>
    /// The options for a not yet explicitly named capture group,
    /// which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IUnnamedCapturedAtom<T> : ICapturedAtom<T>
    {
        /// <summary>
        /// Adds a name to the capture group.
        /// </summary>
        /// <param name="name">The name to associate with the group.</param>
        /// <returns>The named capture group.</returns>
        ICapturedAtom<T> Called(string name);
    }

    internal abstract class Atom<T> : IAtom<T>, ICapturedAtom<T>,
        IAlternativeAtom<T>, IAlternativeCapturedAtom<T>,
        IAdditionAtom<T>, IAdditionCapturedAtom<T>,
        ISequenceAtom<T>, ISequenceCapturedAtom<T>
    {
        private IParentAtom<T>? _parent;

        public IAdditionNext<T> Additionally => AdditionParent;
        public IAlternativeNext<T> Alternatively => AlternativeParent;

        public IGroup<T> As
        {
            get
            {
                var group = new Grouping<T>(null, this);
                _parent = group;

                return group;
            }
        }

        IAlternativeGroup<T> IAlternativeAtom<T>.As => AlternativeParent.WrapInGroup(this);
        ISequenceGroup<T> ISequenceAtom<T>.As => SequenceParent.WrapInGroup(this);
        IAdditionGroup<T> IAdditionAtom<T>.As => AdditionParent.WrapInGroup(this);
        public ISequenceNext<T> FollowedBy => SequenceParent;

        public IRepeatStart<T> GreedilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, false);
                _parent = repeat;

                return repeat;
            }
        }

        ISequenceRepeatStart<T> ISequenceAtom<T>.GreedilyRepeat
            => SequenceParent.WrapInGreedyRepeat(this);

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.GreedilyRepeat
            => AlternativeParent.WrapInGreedyRepeat(this);

        IAdditionRepeatStart<T> IAdditionAtom<T>.GreedilyRepeat
            => AdditionParent.WrapInGreedyRepeat(this);

        public IRepeatStart<T> LazilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, true);
                _parent = repeat;

                return repeat;
            }
        }

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.LazilyRepeat
            => AlternativeParent.WrapInLazyRepeat(this);

        ISequenceRepeatStart<T> ISequenceAtom<T>.LazilyRepeat
            => SequenceParent.WrapInLazyRepeat(this);

        IAdditionRepeatStart<T> IAdditionAtom<T>.LazilyRepeat => AdditionParent.WrapInLazyRepeat(this);

        private IAdditionParentAtom<T> AdditionParent
        {
            get
            {
                _parent ??= new Addition<T>(this);
                return (IAdditionParentAtom<T>)_parent;
            }
        }

        private IAlternativeParentAtom<T> AlternativeParent
        {
            get
            {
                _parent ??= new Alternative<T>(this);
                return (IAlternativeParentAtom<T>)_parent;
            }
        }

        private ISequenceParentAtom<T> SequenceParent
        {
            get
            {
                _parent ??= new Sequence<T>(this);
                return (ISequenceParentAtom<T>)_parent;
            }
        }

        protected Atom(IParentAtom<T>? parent)
        {
            _parent = parent;
        }

        public Generex<T> Finish()
        {
            var current = this;
            while (current._parent != null)
                current = (Atom<T>)current._parent;

            return current.FinishInternal();
        }

        protected static Generex<T> FinishInternal(IFinishableAtom<T> atom)
            => ((Atom<T>)atom).FinishInternal();

        protected static void SetParent(IFinishableAtom<T> atom, IParentAtom<T> parent)
            => ((Atom<T>)atom)._parent = parent;

        protected abstract Generex<T> FinishInternal();
    }

    internal interface IParentAtom<T> : IFinishableAtom<T>
    {
        IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        IGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }
}