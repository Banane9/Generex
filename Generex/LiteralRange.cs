﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class LiteralRange<T>
    {
        public static LiteralRange<T> Wildcard { get; } = new(default!, default!, new WildcardComparer());
        public IComparer<T> Comparer { get; }
        public T Maximum { get; }
        public T Minimum { get; }

        public LiteralRange(T minimum, T maximum, IComparer<T>? comparer = null)
        {
            Minimum = minimum;
            Maximum = maximum;
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public bool Contains(T value)
            => Comparer.Compare(Minimum, value) <= 0 && Comparer.Compare(Maximum, value) >= 0;

        public override string ToString()
        {
            if (Comparer.Compare(Minimum, Maximum) == 0)
                return (Minimum ?? Maximum)?.ToString() ?? "null";

            return $"{Minimum}-{Maximum}";
        }

        private sealed class WildcardComparer : IComparer<T>
        {
            public int Compare(T x, T y) => 0;
        }
    }
}