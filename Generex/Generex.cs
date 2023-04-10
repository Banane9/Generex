﻿using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public abstract partial class Generex<T>
    {
        public static implicit operator Generex<T>(T value) => new Literal<T>(value);

        public static implicit operator Generex<T>(Generex<T>[] atoms) => new Sequence<T>((IEnumerable<Generex<T>>)atoms);

        public static implicit operator Generex<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        public static Generex<T> operator |(Generex<T> leftAtom, Generex<T> rightAtom) => new Alternative<T>(leftAtom, rightAtom);

        public static Generex<T> operator +(Generex<T> leftAtom, Generex<T> rightAtom) => new Sequence<T>(leftAtom, rightAtom);

        protected static string SequenceSeparator { get; } = typeof(T) == typeof(char) ? "" : "⋅";

        public IEnumerable<Match<T>> MatchAll(IEnumerable<T> inputSequence, bool fromStartOnly = false)
        {
            var nextQueue = new Queue<MatchElement>();
            var inputEnumerator = inputSequence.GetEnumerator();

            var currentQueue = new Queue<MatchElement>();
            currentQueue.Enqueue(new MatchElement());

            if (!inputEnumerator.MoveNext())
                yield break;

            var index = 0;
            foreach (var value in inputSequence)
            {
                while (currentQueue.Count > 0)
                {
                    var currentMatch = currentQueue.Dequeue();

                    foreach (var nextMatch in MatchNextInternal(currentMatch, value))
                    {
                        if (nextMatch.IsDone)
                            yield return nextMatch.GetMatch();
                        else
                            nextQueue.Enqueue(nextMatch);
                    }
                }

                (nextQueue, currentQueue) = (currentQueue, nextQueue);

                if (currentQueue.Count == 0 && fromStartOnly)
                    yield break;

                currentQueue.Enqueue(new MatchElement(index));
                ++index;
            }

            while (currentQueue.Count > 0)
            {
                var endMatch = currentQueue.Dequeue();

                if (!MatchEndInternal(endMatch))
                    continue;

                endMatch.IsDone = true;
                yield return endMatch.GetMatch();
            }
        }

        public abstract override string ToString();

        protected static IEnumerable<MatchElement> MatchNext(Generex<T> instance, MatchElement currentMatch, T value)
            => instance.MatchNextInternal(currentMatch, value);

        protected abstract IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value);

        protected virtual bool MatchEndInternal(MatchElement currentMatch) => false;

        protected static bool MatchEnd(Generex<T> instance, MatchElement currentMatch)
            => instance.MatchEndInternal(currentMatch);
    }
}