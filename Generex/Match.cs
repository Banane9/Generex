using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class Match<T>
    {
        private readonly List<T> matchSequence;

        public bool Finished { get; }

        public IEnumerable<T> MatchedSequence
        {
            get
            {
                foreach (var value in matchSequence)
                    yield return value;
            }
        }

        private Func<Match<T>, T, IEnumerable<Match<T>>>? matchNext { get; }

        internal Match(Func<Match<T>, T, IEnumerable<Match<T>>> matchNext)
        {
            matchSequence = new();
            this.matchNext = matchNext;
        }

        internal Match(Match<T> match, Func<Match<T>, T, IEnumerable<Match<T>>> matchNext)
        {
            matchSequence = new(match.MatchedSequence);
            this.matchNext = matchNext;
        }

        internal Match(Match<T> match)
        {
            matchSequence = new(match.MatchedSequence);
            Finished = true;
        }

        internal void Add(T value)
        {
            matchSequence.Add(value);
        }

        internal IEnumerable<Match<T>> MatchNext(T value) => matchNext!(this, value);
    }
}