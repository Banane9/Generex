using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern who's sub-pattern's matched sequence gets saved
    /// and which can be referenced afterwards.<br/>
    /// This can be done either through a <see cref="CapturedGroup{T}"/> using
    /// the same <see cref="ICaptureReference{T}"/>, or with the resulting <see cref="Match{T}"/>.
    /// </summary>
    /// <inheritdoc/>
    public class CapturingGroup<T> : UnaryModifier<T>
    {
        /// <summary>
        /// Gets this pattern's <see cref="ICaptureReference{T}"/>
        /// which can be used to reference its sub-pattern's matched sequence.
        /// </summary>
        public ICaptureReference<T> CaptureReference { get; }

        public CapturingGroup(Generex<T> atom, ICaptureReference<T> captureReference) : base(atom)
        {
            CaptureReference = captureReference;
        }

        /// <inheritdoc/>
        public override string ToString() => $"(?<{CaptureReference}>{Atom})";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                var matchSequence = nextMatch.GetParentSequence()
                    .TakeUntil(match => match == currentMatch)
                    .Skip(1)
                    .Reverse();

                if (nextMatch.TrySetCapture(CaptureReference, matchSequence))
                    yield return nextMatch;
            }
        }
    }
}