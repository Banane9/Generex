using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern that matches any value.
    /// </summary>
    /// <inheritdoc/>
    public sealed class Wildcard<T> : Generex<T>
    {
        /// <inheritdoc/>
        public override string ToString() => ".";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            yield return currentMatch.Next();
        }
    }
}