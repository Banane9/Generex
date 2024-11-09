using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    /// <summary>
    /// The options for the start of a sequence of literals, which just had an explicit
    /// <see cref="IEqualityComparer{T}"/> set and is part of a list of requirements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAdditionComparingLiteral<T>
    {
        /// <summary>
        /// Starts the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The starting sequence of literals.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAdditionLiteral<T> Of(IEnumerable<T> literals);

        /// <summary>
        /// Starts the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal for the sequence.</param>
        /// <param name="furtherLiterals">Any further literals for the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAdditionLiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    /// <summary>
    /// The options for a ready-to-use sequence of literals, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionLiteral<T> : IAdditionAtom<T>
    {
        /// <summary>
        /// Continues the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal to continue the sequence.</param>
        /// <param name="furtherLiterals">Any further literals to continue the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAdditionLiteral<T> And(T literal, params T[] furtherLiterals);

        /// <summary>
        /// Continues the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The sequence of literals to continue with.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAdditionLiteral<T> And(IEnumerable<T> literals);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which is part of a list of requirements.
    /// </summary>
    /// <inheritdoc/>
    public interface IAdditionLiteralStart<T> : IAdditionComparingLiteral<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the sequence of literals.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started sequence with the set <paramref name="equalityComparer"/>.</returns>
        IAdditionComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which just had an explicit
    /// <see cref="IEqualityComparer{T}"/> set and is part of a list of alternatives.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IAlternativeComparingLiteral<T>
    {
        /// <summary>
        /// Starts the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The starting sequence of literals.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAlternativeLiteral<T> Of(IEnumerable<T> literals);

        /// <summary>
        /// Starts the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal for the sequence.</param>
        /// <param name="furtherLiterals">Any further literals for the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAlternativeLiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    /// <summary>
    /// The options for a ready-to-use sequence of literals, which is part of a list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IAlternativeLiteral<T> : IAlternativeAtom<T>
    {
        /// <summary>
        /// Continues the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal to continue the sequence.</param>
        /// <param name="furtherLiterals">Any further literals to continue the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAlternativeLiteral<T> And(T literal, params T[] furtherLiterals);

        /// <summary>
        /// Continues the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The sequence of literals to continue with.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        IAlternativeLiteral<T> And(IEnumerable<T> literals);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which is part of a list of alternatives.
    /// </summary>
    /// <inheritdoc/>
    public interface IAlternativeLiteralStart<T> : IAlternativeComparingLiteral<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the sequence of literals.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started sequence with the set <paramref name="equalityComparer"/>.</returns>
        IAlternativeComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which just had an explicit
    /// <see cref="IEqualityComparer{T}"/> set and is not yet part of a sequence or list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface IComparingLiteral<T>
    {
        /// <summary>
        /// Starts the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The starting sequence of literals.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ILiteral<T> Of(IEnumerable<T> literals);

        /// <summary>
        /// Starts the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal for the sequence.</param>
        /// <param name="furtherLiterals">Any further literals for the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ILiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    /// <summary>
    /// The options for a ready-to-use sequence of literals, which is not yet part of a sequence or list.
    /// </summary>
    /// <inheritdoc/>
    public interface ILiteral<T> : IAtom<T>
    {
        /// <summary>
        /// Continues the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal to continue the sequence.</param>
        /// <param name="furtherLiterals">Any further literals to continue the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ILiteral<T> And(T literal, params T[] furtherLiterals);

        /// <summary>
        /// Continues the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The sequence of literals to continue with.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ILiteral<T> And(IEnumerable<T> literals);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which is not yet part of a sequence or list.
    /// </summary>
    /// <inheritdoc/>
    public interface ILiteralStart<T> : IComparingLiteral<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the sequence of literals.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started sequence with the set <paramref name="equalityComparer"/>.</returns>
        IComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which just had an explicit
    /// <see cref="IEqualityComparer{T}"/> set and is part of a sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ISequenceComparingLiteral<T>
    {
        /// <summary>
        /// Starts the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The starting sequence of literals.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ISequenceLiteral<T> Of(IEnumerable<T> literals);

        /// <summary>
        /// Starts the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal for the sequence.</param>
        /// <param name="furtherLiterals">Any further literals for the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ISequenceLiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    /// <summary>
    /// The options for a ready-to-use sequence of literals, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceLiteral<T> : ISequenceAtom<T>
    {
        /// <summary>
        /// Continues the sequence of literals with the given ones.
        /// </summary>
        /// <param name="literal">The first literal to continue the sequence.</param>
        /// <param name="furtherLiterals">Any further literals to continue the sequence.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ISequenceLiteral<T> And(T literal, params T[] furtherLiterals);

        /// <summary>
        /// Continues the sequence of literals with the given one.
        /// </summary>
        /// <param name="literals">The sequence of literals to continue with.</param>
        /// <returns>The ready-to-use sequence of literals.</returns>
        ISequenceLiteral<T> And(IEnumerable<T> literals);
    }

    /// <summary>
    /// The options for the start of a sequence of literals, which is part of a sequence.
    /// </summary>
    /// <inheritdoc/>
    public interface ISequenceLiteralStart<T> : ISequenceComparingLiteral<T>
    {
        /// <summary>
        /// Sets an explicit <see cref="IEqualityComparer{T}"/> for the sequence of literals.
        /// </summary>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}"/> to use.</param>
        /// <returns>The not-yet-started sequence with the set <paramref name="equalityComparer"/>.</returns>
        ISequenceComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    internal class Literal<T> : Atom<T>, ILiteral<T>, ILiteralStart<T>,
        IAlternativeLiteral<T>, IAlternativeLiteralStart<T>,
        IAdditionLiteral<T>, IAdditionLiteralStart<T>,
        ISequenceLiteral<T>, ISequenceLiteralStart<T>
    {
        private readonly List<T> literals = [];
        private IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

        public Literal(IParentAtom<T>? parent = null) : base(parent)
        { }

        ISequenceLiteral<T> ISequenceLiteral<T>.And(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        ISequenceLiteral<T> ISequenceLiteral<T>.And(IEnumerable<T> literals)
            => and(literals);

        IAdditionLiteral<T> IAdditionLiteral<T>.And(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        IAdditionLiteral<T> IAdditionLiteral<T>.And(IEnumerable<T> literals)
            => and(literals);

        IAlternativeLiteral<T> IAlternativeLiteral<T>.And(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        IAlternativeLiteral<T> IAlternativeLiteral<T>.And(IEnumerable<T> literals)
            => and(literals);

        public ILiteral<T> And(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        public ILiteral<T> And(IEnumerable<T> literals) => and(literals);

        ISequenceLiteral<T> ISequenceComparingLiteral<T>.Of(IEnumerable<T> literals)
            => and(literals);

        ISequenceLiteral<T> ISequenceComparingLiteral<T>.Of(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        public ILiteral<T> Of(IEnumerable<T> literals) => and(literals);

        public ILiteral<T> Of(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        IAdditionLiteral<T> IAdditionComparingLiteral<T>.Of(IEnumerable<T> literals)
            => and(literals);

        IAdditionLiteral<T> IAdditionComparingLiteral<T>.Of(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        IAlternativeLiteral<T> IAlternativeComparingLiteral<T>.Of(IEnumerable<T> literals)
            => and(literals);

        IAlternativeLiteral<T> IAlternativeComparingLiteral<T>.Of(T literal, params T[] furtherLiterals)
            => and(literal.Yield().Concat(furtherLiterals));

        public IComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer)
            => @using(equalityComparer);

        IAdditionComparingLiteral<T> IAdditionLiteralStart<T>.Using(IEqualityComparer<T> equalityComparer)
            => @using(equalityComparer);

        IAlternativeComparingLiteral<T> IAlternativeLiteralStart<T>.Using(IEqualityComparer<T> equalityComparer)
            => @using(equalityComparer);

        ISequenceComparingLiteral<T> ISequenceLiteralStart<T>.Using(IEqualityComparer<T> equalityComparer)
            => @using(equalityComparer);

        protected override Generex<T> finishInternal()
        {
            if (literals.Count == 1)
                return new Atoms.Literal<T>(literals[0], equalityComparer);

            return new Atoms.Sequence<T>(literals.Select(literal => new Atoms.Literal<T>(literal, equalityComparer)));
        }

        private Literal<T> @using(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            return this;
        }

        private Literal<T> and(IEnumerable<T> literals)
        {
            this.literals.AddRange(literals);
            return this;
        }
    }
}