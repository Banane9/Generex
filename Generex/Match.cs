using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public class Match<T> : IEnumerable<T>
    {
        private readonly T[] matchSequence;

        public int EndIndex { get; }
        public int Length => matchSequence.Length;

        public IEnumerable<T> MatchedSequence
        {
            get
            {
                foreach (var value in matchSequence)
                    yield return value;
            }
        }

        public int StartIndex { get; }

        public T this[int index] => matchSequence[index];

        internal Match(IEnumerable<T> matchSequence, int start, int end)
        {
            this.matchSequence = matchSequence.ToArray();
            StartIndex = start;
            EndIndex = end;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)matchSequence).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return matchSequence.GetEnumerator();
        }
    }
}