using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public abstract partial class Atom<T>
    {
        public IEqualityComparer<T> EqualityComparer { get; }

        public Atom(IEqualityComparer<T>? equalityComparer = null)
        {
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public static implicit operator Atom<T>(T value) => new Literal<T>(value);

        public static Atom<T> operator |(Atom<T> leftAtom, Atom<T> rightAtom) => new Alternative<T>(leftAtom, rightAtom);

        public static Atom<T> operator +(Atom<T> leftAtom, Atom<T> rightAtom) => new Sequence<T>(leftAtom, rightAtom);

        public IEnumerable<Match<T>> MatchAll(IEnumerable<T> inputSequence)
        {
            var currentQueue = new Queue<MatchElement>();
            currentQueue.Enqueue(new MatchElement(MatchNextInternal));

            var nextQueue = new Queue<MatchElement>();

            foreach (var value in inputSequence)
            {
                while (currentQueue.Count > 0)
                {
                    var currentMatch = currentQueue.Dequeue();

                    foreach (var match in currentMatch.MatchNext(value))
                    {
                        if (match.IsDone)
                            yield return match.GetMatch();
                        else
                        {
                            nextQueue.Enqueue(match);
                        }
                    }
                }

                if (nextQueue.Count == 0)
                    nextQueue.Enqueue(new MatchElement(MatchNextInternal));

                (nextQueue, currentQueue) = (currentQueue, nextQueue);
            }
        }

        protected static IEnumerable<MatchElement> MatchNext(Atom<T> instance, MatchElement match, T value) => instance.MatchNextInternal(match, value);

        protected abstract IEnumerable<MatchElement> MatchNextInternal(MatchElement match, T value);
    }
}