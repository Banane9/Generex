using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a range of literals, defined by a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public sealed class LiteralRange<T>
    {
        /// <summary>
        /// Gets the comparer used to determine whether a value falls into the range.
        /// </summary>
        public IComparer<T> Comparer { get; }

        /// <summary>
        /// Gets the maximum value in the range.
        /// </summary>
        public T Maximum { get; }

        /// <summary>
        /// Gets the minimum value in the range.
        /// </summary>
        public T Minimum { get; }

        /// <summary>
        /// Creates a new literal range using the given minimum, maximum, and (default) comparer.
        /// </summary>
        /// <param name="minimum">The minimum value in the range.</param>
        /// <param name="maximum">The maximum value in the range.</param>
        /// <param name="comparer">The comparer used to determine whether a value falls into the range. <see cref="Comparer{T}.Default"/> by default.</param>
        /// <exception cref="ArgumentOutOfRangeException">When maximum is &lt; minium.</exception>
        public LiteralRange(T minimum, T maximum, IComparer<T>? comparer = null)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = comparer ?? Comparer<T>.Default;

            if (Comparer.Compare(Minimum, Maximum) > 0)
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must compare as >= to the minimum.");
        }

        /// <summary>
        /// Creates a new literal range using the exact value and (default) comparer.
        /// </summary>
        /// <param name="exactly">The exact value of the range.</param>
        /// <param name="comparer">The comparer used to determine whether a value falls into the range. <see cref="Comparer{T}.Default"/> by default.</param>
        public LiteralRange(T exactly, IComparer<T>? comparer = null) : this(exactly, exactly, comparer)
        { }

        /// <summary>
        /// Determines whether a given <paramref name="value"/> falls into the range.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if <see cref="Minimum">Minimum</see> &lt;= <paramref name="value"/> &lt;= <see cref="Maximum">Maximum</see>, otherwise <c>false</c>.</returns>
        public bool Contains(T value)
            => Comparer.Compare(Minimum, value) <= 0 && Comparer.Compare(Maximum, value) >= 0;

        /// <summary>
        /// Returns a string that represents the current range pattern.
        /// </summary>
        /// <returns>A string that represents the current range pattern.</returns>
        public override string ToString()
        {
            if (Comparer.Compare(Minimum, Maximum) == 0)
                return Generex<T>.EscapeLiteral((Minimum ?? Maximum)?.ToString()) ?? "null";

            return Generex<T>.EscapeLiteral($"{Minimum?.ToString() ?? "null"}-{Maximum?.ToString() ?? "null"}");
        }
    }
}