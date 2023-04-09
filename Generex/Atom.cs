using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static implicit operator Atom<T>(Atom<T>[] atoms) => new Sequence<T>((IEnumerable<Atom<T>>)atoms);

        public static implicit operator Atom<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        public static Atom<T> operator |(Atom<T> leftAtom, Atom<T> rightAtom) => new Alternative<T>(leftAtom, rightAtom);

        public static Atom<T> operator +(Atom<T> leftAtom, Atom<T> rightAtom) => new Sequence<T>(leftAtom, rightAtom);

        public IEnumerable<Match<T>> MatchAll(IEnumerable<T> inputSequence, bool fromStartOnly = false)
        {
            var currentQueue = new Queue<MatchElement>();
            currentQueue.Enqueue(new MatchElement());

            var nextQueue = new Queue<MatchElement>();

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

                if (nextQueue.Count == 0 && fromStartOnly)
                    yield break;

                nextQueue.Enqueue(new MatchElement(index));

                ++index;
                (nextQueue, currentQueue) = (currentQueue, nextQueue);
            }
        }

        public abstract override string ToString();

        protected static IEnumerable<MatchElement> MatchNext(Atom<T> instance, MatchElement currentMatch, T value) => instance.MatchNextInternal(currentMatch, value);

        protected abstract IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value);
    }
}