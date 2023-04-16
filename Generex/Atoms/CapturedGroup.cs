using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a pattern that matches a back-reference to the latest capture of another group, that is, a repetition of the sub-sequence.
    /// </summary>
    /// <inheritdoc/>
    public class CapturedGroup<T> : Generex<T>
    {
        public CaptureReference<T> CaptureReference { get; }

        public IEqualityComparer<T> EqualityComparer { get; }

        public CapturedGroup(CaptureReference<T> captureReference, IEqualityComparer<T>? equalityComparer = null)
        {
            CaptureReference = captureReference;
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"\\k<{CaptureReference}>";

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            var progress = -1;
            if (!currentMatch.TryGetLatestCapture(CaptureReference, out var capture))
                yield break;

            while (++progress < capture.Length && !currentMatch.IsInputEnd && EqualityComparer.Equals(capture[progress], currentMatch.NextValue))
                currentMatch = currentMatch.Next();

            if (progress >= capture.Length)
                yield return currentMatch;
        }
    }
}