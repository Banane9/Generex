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

        public override string ToString(bool grouped)
            => $"\\k'{CaptureReference}'";

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
        {
            var progress = -1;
            if (!currentMatch.TryGetLatestCapture(CaptureReference, out var capture))
                yield break;

            while (++progress < capture.Length && currentMatch.HasNext && EqualityComparer.Equals(capture[progress], currentMatch.NextValue))
                currentMatch = currentMatch.Next();

            if (progress >= capture.Length)
            {
                currentMatch.IsDone = true;
                yield return currentMatch;
            }
        }
    }
}