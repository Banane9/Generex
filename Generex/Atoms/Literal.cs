using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generex.Atoms
{
    public class Literal<T> : Generex<T>
    {
        public T Value { get; }

        public Literal(T value, IEqualityComparer<T>? equalityComparer = null) : base(equalityComparer)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            if (EqualityComparer.Equals(Value, value))
                yield return currentMatch.DoneWithNext(value);
        }
    }
}