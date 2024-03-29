﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a literal value which has to be matched.
    /// </summary>
    /// <inheritdoc/>
    public class Literal<T> : Generex<T>
    {
        public IEqualityComparer<T> EqualityComparer { get; }
        public T Value { get; }

        public Literal(T value, IEqualityComparer<T>? equalityComparer = null)
        {
            Value = value;
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Value is char character && char.IsWhiteSpace(character))
                return $"({character})";

            if (Value is string text && string.IsNullOrWhiteSpace(text))
                return $"({text})";

            return EscapeLiteral(Value?.ToString()) ?? "null";
        }

        protected override IEnumerable<MatchState<T>> continueMatchInternal(MatchState<T> currentMatch)
        {
            if (!currentMatch.IsInputEnd && EqualityComparer.Equals(Value, currentMatch.NextValue))
                yield return currentMatch.Next();
        }
    }
}