using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class CapturedGroup<T> : Generex<T>
    {
        public CaptureReference<T> CaptureReference { get; }

        public IEqualityComparer<T> EqualityComparer { get; }

        public CapturedGroup(CaptureReference<T> captureReference, IEqualityComparer<T>? equalityComparer = null)
        {
            CaptureReference = captureReference;
            EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        public override string ToString()
            => $"\\k'{CaptureReference}'";

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            // If the capture group was never matched before, this can never match
            if (!currentMatch.TryGetLatestState(this, out (T[], int) state)
             && !currentMatch.TryGetLatestCapture(CaptureReference, out state.Item1))
                yield break;

            var (capture, progress) = state;

            // Really about Length == 0
            if (progress >= capture.Length)
            {
                // Make a zero-width match
                var newMatch = currentMatch.Clone();
                newMatch.SetState(this, (capture, 0));
                newMatch.IsDone = true;

                yield return newMatch;
                yield break;
            }

            if (!EqualityComparer.Equals(capture[progress], value))
                yield break;

            var nextMatch = currentMatch.Next(value);

            if (++progress >= capture.Length)
            {
                nextMatch.IsDone = true;
                nextMatch.SetState(this, (capture, 0));
            }
            else
            {
                nextMatch.IsDone = false;
                nextMatch.SetState(this, (capture, progress));
            }

            yield return nextMatch;
        }
    }
}