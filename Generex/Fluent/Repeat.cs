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
        IAdditionAtom<T> And(int maximum);
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
        public IAdditionAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IAdditionAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IAdditionAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionAtom<T> AtMost(int maximum);

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
        public IAdditionAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAdditionAtom<T> MaybeAtMost(int maximum);
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
        IAlternativeAtom<T> And(int maximum);
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
        public IAlternativeAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IAlternativeAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IAlternativeAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeAtom<T> AtMost(int maximum);

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
        public IAlternativeAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAlternativeAtom<T> MaybeAtMost(int maximum);
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
        IAtom<T> And(int maximum);
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
        public IAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public IAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public IAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAtom<T> AtMost(int maximum);

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
        public IAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public IAtom<T> MaybeAtMost(int maximum);
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
        ISequenceAtom<T> And(int maximum);
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
        public ISequenceAtom<T> AnyNumber { get; }

        /// <summary>
        /// Configures the quantifier to allow one to infinite matches.
        /// </summary>
        public ISequenceAtom<T> AtLeastOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow zero to one match(es).
        /// </summary>
        public ISequenceAtom<T> AtMostOnce { get; }

        /// <summary>
        /// Configures the quantifier to allow <paramref name="minimum"/> to infinite matches.
        /// </summary>
        /// <param name="minimum">The minimum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceAtom<T> AtLeast(int minimum);

        /// <summary>
        /// Configures the quantifier to allow one to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceAtom<T> AtMost(int maximum);

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
        public ISequenceAtom<T> Exactly(int exactly);

        /// <summary>
        /// Configures the quantifier to allow zero to <paramref name="maximum"/> matches.
        /// </summary>
        /// <param name="maximum">The maximum number of matches to allow.</param>
        /// <returns>The fully configured quantifier.</returns>
        public ISequenceAtom<T> MaybeAtMost(int maximum);
    }

    internal class Repeat<T> : Atom<T>, IParentAtom<T>,
        IRepeatStart<T>, IRepeatEnd<T>,
        IAlternativeRepeatStart<T>, IAlternativeRepeatEnd<T>,
        IAdditionRepeatStart<T>, IAdditionRepeatEnd<T>,
        ISequenceRepeatStart<T>, ISequenceRepeatEnd<T>
    {
        private readonly bool _lazy;
        private IFinishableAtom<T> _atom;
        private int _maximum = -1;
        private int _minimum = -1;

        ISequenceAtom<T> ISequenceRepeatStart<T>.AnyNumber => (ISequenceAtom<T>)AnyNumber;
        IAlternativeAtom<T> IAlternativeRepeatStart<T>.AnyNumber => (IAlternativeAtom<T>)AnyNumber;
        IAdditionAtom<T> IAdditionRepeatStart<T>.AnyNumber => (IAdditionAtom<T>)AnyNumber;

        public IAtom<T> AnyNumber
        {
            get
            {
                _minimum = 0;
                _maximum = int.MaxValue;
                return this;
            }
        }

        public IAtom<T> AtLeastOnce
        {
            get
            {
                _minimum = 1;
                _maximum = int.MaxValue;
                return this;
            }
        }

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.AtLeastOnce => (IAlternativeAtom<T>)AtLeastOnce;
        IAdditionAtom<T> IAdditionRepeatStart<T>.AtLeastOnce => (IAdditionAtom<T>)AtLeastOnce;
        ISequenceAtom<T> ISequenceRepeatStart<T>.AtLeastOnce => (ISequenceAtom<T>)AtLeastOnce;

        public IAtom<T> AtMostOnce
        {
            get
            {
                _minimum = 0;
                _maximum = 1;
                return this;
            }
        }

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.AtMostOnce => (IAlternativeAtom<T>)AtMostOnce;
        IAdditionAtom<T> IAdditionRepeatStart<T>.AtMostOnce => (IAdditionAtom<T>)AtMostOnce;
        ISequenceAtom<T> ISequenceRepeatStart<T>.AtMostOnce => (ISequenceAtom<T>)AtMostOnce;

        public Repeat(IParentAtom<T>? parent, IFinishableAtom<T> atom, bool lazy) : base(parent)
        {
            _atom = atom;
            _lazy = lazy;
        }

        public IAtom<T> And(int maximum)
        {
            _maximum = maximum;
            return this;
        }

        IAdditionAtom<T> IAdditionRepeatEnd<T>.And(int maximum)
            => (IAdditionAtom<T>)And(maximum);

        IAlternativeAtom<T> IAlternativeRepeatEnd<T>.And(int maximum)
            => (IAlternativeAtom<T>)And(maximum);

        ISequenceAtom<T> ISequenceRepeatEnd<T>.And(int maximum)
            => (ISequenceAtom<T>)And(maximum);

        public IAtom<T> AtLeast(int minimum)
        {
            _minimum = minimum;
            _maximum = int.MaxValue;
            return this;
        }

        IAdditionAtom<T> IAdditionRepeatStart<T>.AtLeast(int minimum)
            => (IAdditionAtom<T>)AtLeast(minimum);

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.AtLeast(int minimum)
            => (IAlternativeAtom<T>)AtLeast(minimum);

        ISequenceAtom<T> ISequenceRepeatStart<T>.AtLeast(int minimum)
            => (ISequenceAtom<T>)AtLeast(minimum);

        public IAtom<T> AtMost(int maximum)
        {
            _minimum = 1;
            _maximum = maximum;
            return this;
        }

        IAdditionAtom<T> IAdditionRepeatStart<T>.AtMost(int maximum)
            => (IAdditionAtom<T>)AtMost(maximum);

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.AtMost(int maximum)
            => (IAlternativeAtom<T>)AtMost(maximum);

        ISequenceAtom<T> ISequenceRepeatStart<T>.AtMost(int maximum)
            => (ISequenceAtom<T>)AtMost(maximum);

        public IRepeatEnd<T> Between(int minimum)
        {
            _minimum = minimum;
            return this;
        }

        IAdditionRepeatEnd<T> IAdditionRepeatStart<T>.Between(int minimum)
            => (IAdditionRepeatEnd<T>)Between(minimum);

        IAlternativeRepeatEnd<T> IAlternativeRepeatStart<T>.Between(int minimum)
            => (IAlternativeRepeatEnd<T>)Between(minimum);

        ISequenceRepeatEnd<T> ISequenceRepeatStart<T>.Between(int minimum)
            => (ISequenceRepeatEnd<T>)Between(minimum);

        public IAtom<T> Exactly(int times)
        {
            _minimum = times;
            _maximum = times;
            return this;
        }

        IAdditionAtom<T> IAdditionRepeatStart<T>.Exactly(int times)
            => (IAdditionAtom<T>)Exactly(times);

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.Exactly(int times)
            => (IAlternativeAtom<T>)Exactly(times);

        ISequenceAtom<T> ISequenceRepeatStart<T>.Exactly(int times)
            => (ISequenceAtom<T>)Exactly(times);

        public IAtom<T> MaybeAtMost(int maximum)
        {
            _minimum = 0;
            _maximum = maximum;
            return this;
        }

        IAdditionAtom<T> IAdditionRepeatStart<T>.MaybeAtMost(int maximum)
            => (IAdditionAtom<T>)MaybeAtMost(maximum);

        IAlternativeAtom<T> IAlternativeRepeatStart<T>.MaybeAtMost(int maximum)
            => (IAlternativeAtom<T>)MaybeAtMost(maximum);

        ISequenceAtom<T> ISequenceRepeatStart<T>.MaybeAtMost(int maximum)
            => (ISequenceAtom<T>)MaybeAtMost(maximum);

        public IRepeatStart<T> WrapInGreedyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, false);
            _atom = repeat;

            return repeat;
        }

        public IGroup<T> WrapInGroup(IFinishableAtom<T> child)
        {
            var group = new Grouping<T>(this, child);
            _atom = group;

            return group;
        }

        public IRepeatStart<T> WrapInLazyRepeat(IFinishableAtom<T> child)
        {
            var repeat = new Repeat<T>(this, child, true);
            _atom = repeat;

            return repeat;
        }

        protected override Generex<T> FinishInternal()
            => _lazy ? new LazyQuantifier<T>(FinishInternal(_atom), _minimum, _maximum)
                : new GreedyQuantifier<T>(FinishInternal(_atom), _minimum, _maximum);
    }
}