using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public sealed class Wildcard<T> : Generex<T>
    {
        public override string ToString() => ".";

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
        {
            yield return currentMatch.DoneWithNext();
        }
    }
}