using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a quantifier, which is missing its maximum and is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionRepeatEnd<T>
    {
        /// <summary>
        /// Sets the maximum number of matches allowed by the quantifier.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        IAdditionRepeatedAtom<T> And(int maximum);
    }

    /// <summary>
    /// The options for an unconfigured quantifier, which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionRepeatStart<T>
    {
        /// <summary>
        /// Configures the quantifier to allow zero to infinite matches.
        /// </summary>
        public IAdditionRepeatedAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IAdditionRepeatedAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IAdditionRepeatedAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionRepeatedAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionRepeatedAtom<T> AtMost(int maximum);

        /// <summary>
        /// Configures the quantifier to allow at least <paramref name="minimum"/> matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The quantifier missing its maximum.</returns>
        public IAdditionRepeatEnd<T> Between(int minimum);

        /// <summary>
        /// Configures the quantifier to allow <paramref name="exactly"/> matches.
        /// </summary>
        /// <param name="exactly">The number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionRepeatedAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    /// <summary>
    /// The options for a quantifier, which is missing its maximum and is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeRepeatEnd<T>
    {
        /// <summary>
        /// Sets the maximum number of matches allowed by the quantifier.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        IAlternativeRepeatedAtom<T> And(int maximum);
    }

    /// <summary>
    /// The options for an unconfigured quantifier, which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeRepeatStart<T>
    {
        /// <summary>
        /// Configures the quantifier to allow zero to infinite matches.
        /// </summary>
        public IAlternativeRepeatedAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IAlternativeRepeatedAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IAlternativeRepeatedAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeRepeatedAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeRepeatedAtom<T> AtMost(int maximum);

        /// <summary>
        /// Configures the quantifier to allow at least <paramref name="minimum"/> matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The quantifier missing its maximum.</returns>
        public IAlternativeRepeatEnd<T> Between(int minimum);

        /// <summary>
        /// Configures the quantifier to allow <paramref name="exactly"/> matches.
        /// </summary>
        /// <param name="exactly">The number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeRepeatedAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    /// <summary>
    /// The options for a quantifier, which is missing its maximum and
    /// is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IRepeatEnd<T>
    {
        /// <summary>
        /// Sets the maximum number of matches allowed by the quantifier.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        IRepeatedAtom<T> And(int maximum);
    }

    /// <summary>
    /// The options for an unconfigured quantifier, which is not yet part of a sequence or list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IRepeatStart<T>
    {
        /// <summary>
        /// Configures the quantifier to allow zero to infinite matches.
        /// </summary>
        public IRepeatedAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IRepeatedAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IRepeatedAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IRepeatedAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IRepeatedAtom<T> AtMost(int maximum);

        /// <summary>
        /// Configures the quantifier to allow at least <paramref name="minimum"/> matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The quantifier missing its maximum.</returns>
        public IRepeatEnd<T> Between(int minimum);

        /// <summary>
        /// Configures the quantifier to allow <paramref name="exactly"/> matches.
        /// </summary>
        /// <param name="exactly">The number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IRepeatedAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    /// <summary>
    /// The options for a quantifier, which is missing its maximum and part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceRepeatEnd<T>
    {
        /// <summary>
        /// Sets the maximum number of matches allowed by the quantifier.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        ISequenceRepeatedAtom<T> And(int maximum);
    }

    /// <summary>
    /// The options for an unconfigured quantifier, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceRepeatStart<T>
    {
        /// <summary>
        /// Configures the quantifier to allow zero to infinite matches.
        /// </summary>
        public ISequenceRepeatedAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public ISequenceRepeatedAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public ISequenceRepeatedAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceRepeatedAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceRepeatedAtom<T> AtMost(int maximum);

        /// <summary>
        /// Configures the quantifier to allow at least <paramref name="minimum"/> matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The quantifier missing its maximum.</returns>
        public ISequenceRepeatEnd<T> Between(int minimum);

        /// <summary>
        /// Configures the quantifier to allow <paramref name="exactly"/> matches.
        /// </summary>
        /// <param name="exactly">The number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceRepeatedAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceRepeatedAtom<T> MaybeAtMost(int maximum);
    }

    internal class Repeat<T> : Atom<T>, IParentAtom<T>,
        IRepeatStart<T>, IRepeatEnd<T>,
        IAlternativeRepeatStart<T>, IAlternativeRepeatEnd<T>,
        IAdditionRepeatStart<T>, IAdditionRepeatEnd<T>,
        ISequenceRepeatStart<T>, ISequenceRepeatEnd<T>
    {
        private readonly bool lazy;
        private IFinishableAtom<T> atom;
        private int maximum = -1;
        private int minimum = -1;
        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AnyNumber => (ISequenceRepeatedAtom<T>)AnyNumber;
        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AnyNumber => (IAlternativeRepeatedAtom<T>)AnyNumber;
        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.AnyNumber => (IAdditionRepeatedAtom<T>)AnyNumber;

        public IRepeatedAtom<T> AnyNumber
        {
            get
            {
                minimum = 0;
                maximum = int.MaxValue;
                return this;
            }
        }

        public IRepeatedAtom<T> AtLeastOnce
        {
            get
            {
                minimum = 1;
                maximum = int.MaxValue;
                return this;
            }
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtLeastOnce => (IAlternativeRepeatedAtom<T>)AtLeastOnce;
        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.AtLeastOnce => (IAdditionRepeatedAtom<T>)AtLeastOnce;

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtLeastOnce => (ISequenceRepeatedAtom<T>)AtLeastOnce;

        public IRepeatedAtom<T> AtMostOnce
        {
            get
            {
                minimum = 0;
                maximum = 1;
                return this;
            }
        }

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtMostOnce => (IAlternativeRepeatedAtom<T>)AtMostOnce;
        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.AtMostOnce => (IAdditionRepeatedAtom<T>)AtMostOnce;
        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtMostOnce => (ISequenceRepeatedAtom<T>)AtMostOnce;

        public Repeat(IParentAtom<T>? parent, IFinishableAtom<T> atom, bool lazy) : base(parent)
        {
            this.atom = atom;
            this.lazy = lazy;
        }

        public IRepeatedAtom<T> And(int maximum)
        {
            this.maximum = maximum;
            return this;
        }

        IAdditionRepeatedAtom<T> IAdditionRepeatEnd<T>.And(int maximum)
            => (IAdditionRepeatedAtom<T>)And(maximum);

        IAlternativeRepeatedAtom<T> IAlternativeRepeatEnd<T>.And(int maximum)
            => (IAlternativeRepeatedAtom<T>)And(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatEnd<T>.And(int maximum)
            => (ISequenceRepeatedAtom<T>)And(maximum);

        public IRepeatedAtom<T> AtLeast(int minimum)
        {
            this.minimum = minimum;
            maximum = int.MaxValue;
            return this;
        }

        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.AtLeast(int minimum)
            => (IAdditionRepeatedAtom<T>)AtLeast(minimum);

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtLeast(int minimum)
            => (IAlternativeRepeatedAtom<T>)AtLeast(minimum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtLeast(int minimum)
            => (ISequenceRepeatedAtom<T>)AtLeast(minimum);

        public IRepeatedAtom<T> AtMost(int maximum)
        {
            minimum = 1;
            this.maximum = maximum;
            return this;
        }

        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.AtMost(int maximum)
            => (IAdditionRepeatedAtom<T>)AtMost(maximum);

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.AtMost(int maximum)
            => (IAlternativeRepeatedAtom<T>)AtMost(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.AtMost(int maximum)
            => (ISequenceRepeatedAtom<T>)AtMost(maximum);

        public IRepeatEnd<T> Between(int minimum)
        {
            this.minimum = minimum;
            return this;
        }

        IAdditionRepeatEnd<T> IAdditionRepeatStart<T>.Between(int minimum)
            => (IAdditionRepeatEnd<T>)Between(minimum);

        IAlternativeRepeatEnd<T> IAlternativeRepeatStart<T>.Between(int minimum)
            => (IAlternativeRepeatEnd<T>)Between(minimum);

        ISequenceRepeatEnd<T> ISequenceRepeatStart<T>.Between(int minimum)
            => (ISequenceRepeatEnd<T>)Between(minimum);

        public IRepeatedAtom<T> Exactly(int times)
        {
            minimum = times;
            maximum = times;
            return this;
        }

        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.Exactly(int times)
            => (IAdditionRepeatedAtom<T>)Exactly(times);

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.Exactly(int times)
            => (IAlternativeRepeatedAtom<T>)Exactly(times);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.Exactly(int times)
            => (ISequenceRepeatedAtom<T>)Exactly(times);

        public IRepeatedAtom<T> MaybeAtMost(int maximum)
        {
            minimum = 0;
            this.maximum = maximum;
            return this;
        }

        IAdditionRepeatedAtom<T> IAdditionRepeatStart<T>.MaybeAtMost(int maximum)
            => (IAdditionRepeatedAtom<T>)MaybeAtMost(maximum);

        IAlternativeRepeatedAtom<T> IAlternativeRepeatStart<T>.MaybeAtMost(int maximum)
            => (IAlternativeRepeatedAtom<T>)MaybeAtMost(maximum);

        ISequenceRepeatedAtom<T> ISequenceRepeatStart<T>.MaybeAtMost(int maximum)
            => (ISequenceRepeatedAtom<T>)MaybeAtMost(maximum);

        public IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            atom = repeat;

            return repeat;
        }

        public IGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            atom = group;

            return group;
        }

        public IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            atom = repeat;

            return repeat;
        }

        protected override Generex<T> finishInternal()
            => lazy ? new LazyQuantifier<T>(finishInternal(atom), minimum, maximum)
                : new GreedyQuantifier<T>(finishInternal(atom), minimum, maximum);
    }
}