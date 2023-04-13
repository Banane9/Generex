using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern who's sub-pattern's matched sequence gets saved and which can be referenced afterwards,
    /// either through a <see cref="CapturedGroup{T}"/> using the same <see cref="CaptureReference{T}"/> or in the resulting <see cref="Match{T}"/>.
    /// </summary>
    /// <inheritdoc/>
    public class CapturingGroup<T> : UnaryModifier<T>
    {
        /// <summary>
        /// Gets this pattern's <see cref="CaptureReference{T}"/> which can be used to reference its sub-pattern's matched sequence.
        /// </summary>
        public CaptureReference<T> CaptureReference { get; }

        public CapturingGroup(Generex<T> atom, CaptureReference<T> captureReference) : base(atom)
        {
            CaptureReference = captureReference;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"(?'{CaptureReference}'{Atom})";
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                nextMatch.SetCapture(CaptureReference,
                    nextMatch.GetParentSequence()
                        .Skip(1)
                        .TakeUntil(match => match != currentMatch)
                        .Select(match => match.NextValue!)
                        .Reverse()
                        .ToArray());

                yield return nextMatch;
            }
        }
    }
}