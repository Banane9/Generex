using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Generex
{
    public class Literal<T> : Atom<T>
    {
        public T Value { get; }

        public Literal(T value, IEqualityComparer<T>? equalityComparer = null) : base(equalityComparer)
        {
            Value = value;
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement match, T value)
        {
            if (EqualityComparer.Equals(Value, value))
                yield return match.Next(value);
        }
    }
}