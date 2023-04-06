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
    }
}