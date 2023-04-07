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

        public IEnumerable<T> MatchedSequence
        {
            get
            {
                foreach (var value in matchSequence)
                    yield return value;
            }
        }

        internal Match(IEnumerable<T> matchSequence)
        {
            this.matchSequence = matchSequence.ToArray();
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