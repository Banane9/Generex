using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generex.Atoms
{
    public class Literal<T> : Generex<T>
    {
        public IEqualityComparer<T> EqualityComparer { get; }
        public T Value { get; }

        public Literal(T value, IEqualityComparer<T>? equalityComparer = null)
        {
            Value = value;
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public override string ToString()
        {
            if (Value is char character && char.IsWhiteSpace(character))
                return $"({character})";

            if (Value is string text && string.IsNullOrWhiteSpace(text))
                return $"({text})";

            return EscapeLiteral(Value?.ToString()) ?? "null";
        }

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
        {
            if (currentMatch.HasNext && EqualityComparer.Equals(Value, currentMatch.NextValue))
                yield return currentMatch.DoneWithNext();
        }
    }
}