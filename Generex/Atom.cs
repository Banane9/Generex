using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public abstract class Atom<T>
    {
        public IEqualityComparer<T> Comparer { get; }

        public Atom(IEqualityComparer<T>? comparer = null)
        {
            Comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public static Atom<T> operator |(Atom<T> leftAtom, Atom<T> rightAtom) => new Alternative<T>(leftAtom, rightAtom);

        public static Atom<T> operator +(Atom<T> leftAtom, Atom<T> rightAtom) => new Sequence<T>(leftAtom, rightAtom);

        public IEnumerable<Match<T>> Match(IEnumerable<T> inputSequence)
        {
            var currentQueue = new Queue<Match<T>>();
            currentQueue.Enqueue(new Match<T>(MatchNextInternal));

            var nextQueue = new Queue<Match<T>>();

            foreach (var value in inputSequence)
            {
                while (currentQueue.Count > 0)
                {
                    var currentMatch = currentQueue.Dequeue();

                    foreach (var match in currentMatch.MatchNext(value))
                    {
                        if (match.Finished)
                            yield return match;
                        else
                        {
                            nextQueue.Enqueue(match);
                        }
                    }
                }

                if (nextQueue.Count == 0)
                    nextQueue.Enqueue(new Match<T>(MatchNextInternal));

                (nextQueue, currentQueue) = (currentQueue, nextQueue);
            }
        }

        protected static IEnumerable<Match<T>> MatchNext(Atom<T> instance, Match<T> match, T value) => instance.MatchNextInternal(match, value);

        protected abstract IEnumerable<Match<T>> MatchNextInternal(Match<T> match, T value);
    }
}