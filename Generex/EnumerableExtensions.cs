using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            foreach (var sourceItem in source)
                yield return sourceItem;

            yield return item;
        }

        public static IEnumerable<TIn> TakeUntil<TIn>(this IEnumerable<TIn> source, Predicate<TIn> predicate)
        {
            foreach (var item in source)
            {
                yield return item;

                if (!predicate(item))
                    yield break;
            }
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}