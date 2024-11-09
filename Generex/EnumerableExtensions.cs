using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Takes items from <paramref name="source"/> until <paramref name="predicate"/> is <c>true</c>,
        /// including the last item that made it so.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <param name="predicate">The function determining when to stop taking elements.</param>
        /// <returns>The items of the <paramref name="source"/> until <paramref name="predicate"/> is <c>true</c>.</returns>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (var item in source)
            {
                yield return item;

                if (predicate(item))
                    yield break;
            }
        }
    }
}