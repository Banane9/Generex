using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for a range of literals, which is missing its maximum and is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionOpenRange<T>
    {
        /// <summary>
        /// Configures the maximum for an entry in the range of literals.
        /// </summary>
        /// <param name="maximum">The maximum literal for the entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAdditionRange<T> To(T maximum);
    }

    /// <summary>
    /// The options for a ready-to-use range of literals, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionRange<T> : IAdditionAtom<T>
    {
        /// <summary>
        /// Continue the range of literals.
        /// </summary>
        IAdditionRangeAddition<T> And { get; }
    }

    /// <summary>
    /// The options for continuing a range of literals, which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionRangeAddition<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IAdditionOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAdditionRange<T> With(T exactly);
    }

    /// <summary>
    /// The options for the start of a range of literals, which is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionRangeStart<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IAdditionOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAdditionRange<T> Of(T literal);

        /// <summary>
        /// Sets an explicit <see cref="IComparer{T}"/> for the range of literals.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started range of literals with the set <paramref name="comparer"/>.</returns>
        IAdditionRangeAddition<T> Using(IComparer<T> comparer);
    }

    /// <summary>
    /// The options for a range of literals, which is missing its maximum and is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeOpenRange<T>
    {
        /// <summary>
        /// Configures the maximum for an entry in the range of literals.
        /// </summary>
        /// <param name="maximum">The maximum literal for the entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAlternativeRange<T> To(T maximum);
    }

    /// <summary>
    /// The options for a ready-to-use range of literals, which is part of a list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IAlternativeRange<T> : IAlternativeAtom<T>
    {
        /// <summary>
        /// Continue the range of literals.
        /// </summary>
        IAlternativeRangeAddition<T> And { get; }
    }

    /// <summary>
    /// The options for continuing a range of literals, which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeRangeAddition<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IAlternativeOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAlternativeRange<T> With(T exactly);
    }

    /// <summary>
    /// The options for the start of a range of literals, which is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeRangeStart<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IAlternativeOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IAlternativeRange<T> Of(T literal);

        /// <summary>
        /// Sets an explicit <see cref="IComparer{T}"/> for the range of literals.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started range of literals with the set <paramref name="comparer"/>.</returns>
        IAlternativeRangeAddition<T> Using(IComparer<T> comparer);
    }

    /// <summary>
    /// The options for a range of literals, which is missing its maximum and is not yet part of a sequence or list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IOpenRange<T>
    {
        /// <summary>
        /// Configures the maximum for an entry in the range of literals.
        /// </summary>
        /// <param name="maximum">The maximum literal for the entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IRange<T> To(T maximum);
    }

    /// <summary>
    /// The options for a ready-to-use range of literals, which is not yet part of a sequence or list.
    /// </summary>
    /// <inheritdoc/>
    public interface IRange<T> : IAtom<T>
    {
        /// <summary>
        /// Continue the range of literals.
        /// </summary>
        IRangeAddition<T> And { get; }
    }

    /// <summary>
    /// The options for continuing a range of literals, which is not yet part of a sequence or list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IRangeAddition<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IRange<T> With(T exactly);
    }

    /// <summary>
    /// The options for the start of a range of literals, which is not yet part of a sequence or list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IRangeStart<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        IOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        IRange<T> Of(T literal);

        /// <summary>
        /// Sets an explicit <see cref="IComparer{T}"/> for the range of literals.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started range of literals with the set <paramref name="comparer"/>.</returns>
        IRangeAddition<T> Using(IComparer<T> comparer);
    }

    /// <summary>
    /// The options for a range of literals, which is missing its maximum and is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceOpenRange<T>
    {
        /// <summary>
        /// Configures the maximum for an entry in the range of literals.
        /// </summary>
        /// <param name="maximum">The maximum literal for the entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        ISequenceRange<T> To(T maximum);
    }

    /// <summary>
    /// The options for a ready-to-use range of literals, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceRange<T> : ISequenceAtom<T>
    {
        /// <summary>
        /// Continue the range of literals.
        /// </summary>
        ISequenceRangeAddition<T> And { get; }
    }

    /// <summary>
    /// The options for continuing a range of literals, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceRangeAddition<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        ISequenceOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        ISequenceRange<T> With(T exactly);
    }

    /// <summary>
    /// The options for the start of a range of literals, which is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceRangeStart<T>
    {
        /// <summary>
        /// Configures the minimum for a new entry in the range of literals.
        /// </summary>
        /// <param name="minimum">The minimum literal for the new entry.</param>
        /// <returns>The range of literals with a new entry missing its maximum.</returns>
        ISequenceOpenRange<T> From(T minimum);

        /// <summary>
        /// Adds a single literal as a new entry in the range of literals.
        /// </summary>
        /// <param name="exactly">The literal for the new entry.</param>
        /// <returns>The ready-to-use range of literals.</returns>
        ISequenceRange<T> Of(T literal);

        /// <summary>
        /// Sets an explicit <see cref="IComparer{T}"/> for the range of literals.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started range of literals with the set <paramref name="comparer"/>.</returns>
        ISequenceRangeAddition<T> Using(IComparer<T> comparer);
    }

    internal class Range<T> : Atom<T>, IRangeStart<T>, IRange<T>, IOpenRange<T>, IRangeAddition<T>,
        IAlternativeRangeStart<T>, IAlternativeRange<T>, IAlternativeOpenRange<T>, IAlternativeRangeAddition<T>,
        IAdditionRangeStart<T>, IAdditionRange<T>, IAdditionOpenRange<T>, IAdditionRangeAddition<T>,
        ISequenceRangeStart<T>, ISequenceRange<T>, ISequenceOpenRange<T>, ISequenceRangeAddition<T>
    {
        private readonly List<T> literals = new();
        private readonly List<LiteralRange<T>> ranges = new();
        private T? capturedMinimum;
        private IComparer<T> comparer = Comparer<T>.Default;

        public IRangeAddition<T> And => this;

        IAlternativeRangeAddition<T> IAlternativeRange<T>.And => this;
        IAdditionRangeAddition<T> IAdditionRange<T>.And => this;
        ISequenceRangeAddition<T> ISequenceRange<T>.And => this;

        public Range(IParentAtom<T>? parentSequence = null) : base(parentSequence)
        { }

        public IOpenRange<T> From(T minimum)
        {
            capturedMinimum = minimum;
            return this;
        }

        IAdditionOpenRange<T> IAdditionRangeStart<T>.From(T minimum)
            => (IAdditionOpenRange<T>)From(minimum);

        IAdditionOpenRange<T> IAdditionRangeAddition<T>.From(T minimum)
            => (IAdditionOpenRange<T>)From(minimum);

        IAlternativeOpenRange<T> IAlternativeRangeStart<T>.From(T minimum)
            => (IAlternativeOpenRange<T>)From(minimum);

        IAlternativeOpenRange<T> IAlternativeRangeAddition<T>.From(T minimum)
            => (IAlternativeOpenRange<T>)From(minimum);

        ISequenceOpenRange<T> ISequenceRangeStart<T>.From(T minimum)
            => (ISequenceOpenRange<T>)From(minimum);

        ISequenceOpenRange<T> ISequenceRangeAddition<T>.From(T minimum)
            => (ISequenceOpenRange<T>)From(minimum);

        public IRange<T> Of(T literal) => With(literal);

        IAdditionRange<T> IAdditionRangeStart<T>.Of(T literal)
            => (IAdditionRange<T>)Of(literal);

        IAlternativeRange<T> IAlternativeRangeStart<T>.Of(T literal)
            => (IAlternativeRange<T>)Of(literal);

        ISequenceRange<T> ISequenceRangeStart<T>.Of(T literal)
            => (ISequenceRange<T>)Of(literal);

        public IRange<T> To(T maximum)
        {
            ranges.Add(new LiteralRange<T>(capturedMinimum!, maximum, comparer));
            capturedMinimum = default;

            return this;
        }

        IAdditionRange<T> IAdditionOpenRange<T>.To(T maximum)
            => (IAdditionRange<T>)To(maximum);

        IAlternativeRange<T> IAlternativeOpenRange<T>.To(T maximum)
            => (IAlternativeRange<T>)To(maximum);

        ISequenceRange<T> ISequenceOpenRange<T>.To(T maximum)
            => (ISequenceRange<T>)To(maximum);

        public IRangeAddition<T> Using(IComparer<T> comparer)
        {
            this.comparer = comparer;
            return this;
        }

        IAdditionRangeAddition<T> IAdditionRangeStart<T>.Using(IComparer<T> comparer)
            => (IAdditionRangeAddition<T>)Using(comparer);

        IAlternativeRangeAddition<T> IAlternativeRangeStart<T>.Using(IComparer<T> comparer)
            => (IAlternativeRangeAddition<T>)Using(comparer);

        ISequenceRangeAddition<T> ISequenceRangeStart<T>.Using(IComparer<T> comparer)
            => (ISequenceRangeAddition<T>)Using(comparer);

        public IRange<T> With(T exactly)
        {
            literals.Add(exactly);
            return this;
        }

        IAdditionRange<T> IAdditionRangeAddition<T>.With(T exactly)
            => (IAdditionRange<T>)With(exactly);

        IAlternativeRange<T> IAlternativeRangeAddition<T>.With(T exactly)
            => (IAlternativeRange<T>)With(exactly);

        ISequenceRange<T> ISequenceRangeAddition<T>.With(T exactly)
            => (ISequenceRange<T>)With(exactly);

        protected override Generex<T> finishInternal()
            => new Atoms.Range<T>(ranges);
    }
}