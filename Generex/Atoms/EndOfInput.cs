using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern that matches the end of the input sequence without advancing.<br/>
    /// This and <see cref="StartOfInput{T}"/> make the most sense as part of a <see cref="Sequence{T}"/>.
    /// </summary>
    /// <inheritdoc/>
    public class EndOfInput<T> : Generex<T>
    {
        /// <inheritdoc/>
        public override string ToString() => "$";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (currentMatch.IsInputEnd)
                yield return currentMatch;
        }
    }
}