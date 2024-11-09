using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public class MatchedSequence<T> : IEnumerable<T>
    {
        private readonly T[] sequence;

        public static MatchedSequence<T> Invalid { get; } = new MatchedSequence<T>(-1);

        public int EndIndex { get; }

        public int Length => sequence.Length;

        public int StartIndex { get; }

        public T this[int index] => sequence[index];

        internal MatchedSequence(IEnumerable<MatchState<T>> matchSequence)
        {
            var matches = matchSequence.ToArray();

            if (matches.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(matchSequence), "Match Sequence must have at least one element!");

            sequence = matchSequence.Select(match => match.NextValue).ToArray();
            StartIndex = matches[0].Index;
            EndIndex = matches[^1].Index;
        }

        internal MatchedSequence(int index)
        {
            sequence = [];
            StartIndex = index;
            EndIndex = index;
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)sequence).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => sequence.GetEnumerator();
    }
}