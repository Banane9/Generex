using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Generex
{
    public sealed partial class MatchState<T>
    {
        private readonly Dictionary<CaptureReference<T>, T[]> captureState = new();
        private readonly PeekAheadEnumerator peekAheadEnumerator;
        public bool Capturing { get; set; } = true;
        public int Index { get; }
        public bool IsInputEnd { get; }
        public bool IsInputStart => Index == 0;
        public bool IsMatchEnd { get; set; }
        public bool IsMatchStart => Previous == null;
        public T NextValue { get; }

        public MatchState<T>? Previous { get; }

        public MatchState(IEnumerable<T> inputSequence)
        {
            peekAheadEnumerator = new PeekAheadEnumerator(inputSequence);
            IsInputEnd = peekAheadEnumerator.MoveNextAndResetPeek();
            NextValue = peekAheadEnumerator.Current;
            Index = 0;
        }

        private MatchState(MatchState<T> template, int index)
        {
            peekAheadEnumerator = template.peekAheadEnumerator.Snapshot();
            NextValue = template.NextValue;
            Capturing = template.Capturing;
            Previous = template.Previous;
            IsInputEnd = template.IsInputEnd;
            IsMatchEnd = template.IsMatchEnd;
            Index = index;
        }

        private MatchState(MatchState<T> previous)
        {
            peekAheadEnumerator = previous.peekAheadEnumerator.Snapshot();
            IsInputEnd = peekAheadEnumerator.MoveNext();
            NextValue = peekAheadEnumerator.Current;
            Capturing = previous.Capturing;
            Previous = previous;
            Index = previous.Index + 1;
        }

        public MatchState<T> Clone()
        {
            var clone = new MatchState<T>(this, Index);

            foreach (var state in captureState)
                clone.SetCapture(state.Key, state.Value);

            return clone;
        }

        public MatchState<T> DoneWithNext() => new(this) { IsMatchEnd = true };

        public Match<T> GetMatch()
        {
            var matchSequence = GetMatchSequence().ToArray();
            var start = matchSequence[0].Index;
            var end = matchSequence[^1].Index;

            return new Match<T>(matchSequence.Select(element => element.NextValue!), matchSequence.Where(element => element.Capturing).Select(element => element.NextValue!), start, end);
        }

        public IEnumerable<MatchState<T>> GetMatchSequence()
        {
            return GetParentSequence()
                .Skip(1)
                .Reverse();
        }

        public IEnumerable<MatchState<T>> GetParentSequence()
        {
            var current = this;
            do
            {
                yield return current;
                current = current.Previous;
            }
            while (!current!.IsMatchStart && !current.IsMatchEnd);

            yield return current;
        }

        public MatchState<T> Next() => new(this);

        public void SetCapture(CaptureReference<T> captureReference, T[] capture)
            => captureState[captureReference] = capture;

        public bool TryGetCapture(CaptureReference<T> captureReference, out T[] capture)
        {
            if (captureState.TryGetValue(captureReference, out capture))
                return true;

            capture = Array.Empty<T>();
            return false;
        }

        public bool TryGetLatestCapture(CaptureReference<T> captureReference, out T[] capture)
        {
            foreach (var matchElement in GetParentSequence())
                if (matchElement.TryGetCapture(captureReference, out capture))
                    return true;

            capture = Array.Empty<T>();
            return false;
        }
    }
}