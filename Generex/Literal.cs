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

        public Literal(T value, IEqualityComparer<T>? comparer = null) : base(comparer)
        {
            Value = value;
        }

        public static implicit operator Literal<T>(T value) => new(value);

        protected override IEnumerable<Match<T>> MatchNextInternal(Match<T> match, T value)
        {
            if (!Comparer.Equals(Value, value))
                yield break;

            var foundMatch = new Match<T>(match);
            foundMatch.Add(value);

            yield return foundMatch;
        }
    }
}