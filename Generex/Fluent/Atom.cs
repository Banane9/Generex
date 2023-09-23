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
    /// The options for a quantified atom,
    /// which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IRepeatedAtom<T> : IFinishableAtom<T>
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

    internal abstract class Atom<T> : IAtom<T>, ICapturedAtom<T>, IRepeatedAtom<T>,
        IAlternativeAtom<T>, IAlternativeCapturedAtom<T>, IAlternativeRepeatedAtom<T>,
        IAdditionAtom<T>, IAdditionCapturedAtom<T>, IAdditionRepeatedAtom<T>,
        ISequenceAtom<T>, ISequenceCapturedAtom<T>, ISequenceRepeatedAtom<T>
    {
        private IParentAtom<T>? parent;

        public IAdditionNext<T> Additionally => additionParent;
        public IAlternativeNext<T> Alternatively => alternativeParent;

        public IGroup<T> As
        {
            get
            {
                var group = new Grouping<T>(null, this);
                parent = group;

                return group;
            }
        }

        IAlternativeGroup<T> IAlternativeRepeatedAtom<T>.As => alternativeParent.WrapInGroup(this);

        ISequenceGroup<T> ISequenceRepeatedAtom<T>.As => sequenceParent.WrapInGroup(this);

        IAlternativeGroup<T> IAlternativeAtom<T>.As => alternativeParent.WrapInGroup(this);
        ISequenceGroup<T> ISequenceAtom<T>.As => sequenceParent.WrapInGroup(this);
        IAdditionGroup<T> IAdditionAtom<T>.As => additionParent.WrapInGroup(this);
        IAdditionGroup<T> IAdditionRepeatedAtom<T>.As => additionParent.WrapInGroup(this);
        public ISequenceNext<T> FollowedBy => sequenceParent;

        public IRepeatStart<T> GreedilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, false);
                parent = repeat;

                return repeat;
            }
        }

        ISequenceRepeatStart<T> ISequenceAtom<T>.GreedilyRepeat
            => sequenceParent.WrapInGreedyRepeat(this);

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.GreedilyRepeat
            => alternativeParent.WrapInGreedyRepeat(this);

        IAdditionRepeatStart<T> IAdditionAtom<T>.GreedilyRepeat
            => additionParent.WrapInGreedyRepeat(this);

        public IRepeatStart<T> LazilyRepeat
        {
            get
            {
                var repeat = new Repeat<T>(null, this, true);
                parent = repeat;

                return repeat;
            }
        }

        IAlternativeRepeatStart<T> IAlternativeAtom<T>.LazilyRepeat
            => alternativeParent.WrapInLazyRepeat(this);

        ISequenceRepeatStart<T> ISequenceAtom<T>.LazilyRepeat
            => sequenceParent.WrapInLazyRepeat(this);

        IAdditionRepeatStart<T> IAdditionAtom<T>.LazilyRepeat => additionParent.WrapInLazyRepeat(this);

        private IAdditionParentAtom<T> additionParent
        {
            get
            {
                parent ??= new Addition<T>(this);
                return (IAdditionParentAtom<T>)parent;
            }
        }

        private IAlternativeParentAtom<T> alternativeParent
        {
            get
            {
                parent ??= new Alternative<T>(this);
                return (IAlternativeParentAtom<T>)parent;
            }
        }

        private ISequenceParentAtom<T> sequenceParent
        {
            get
            {
                parent ??= new Sequence<T>(this);
                return (ISequenceParentAtom<T>)parent;
            }
        }

        protected Atom(IParentAtom<T>? parent)
        {
            this.parent = parent;
        }

        public Generex<T> Finish()
        {
            var current = this;
            while (current.parent != null)
                current = (Atom<T>)current.parent;

            return current.finishInternal();
        }

        protected static Generex<T> finishInternal(IFinishableAtom<T> atom)
            => ((Atom<T>)atom).finishInternal();

        protected static void setParent(IFinishableAtom<T> atom, IParentAtom<T> parent)
            => ((Atom<T>)atom).parent = parent;

        protected abstract Generex<T> finishInternal();
    }

    internal interface IParentAtom<T> : IFinishableAtom<T>
    {
        IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child);

        IGroup<T> WrapInGroup(IFinishableAtom<T> child);

        IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child);
    }
}