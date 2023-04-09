using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}