using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents the current position and other state in a match attempt.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public sealed partial class MatchState<T>
    {
        private readonly Dictionary<CaptureReference<T>, T[]> captureState = new();
        private readonly PeekAheadEnumerator peekAheadEnumerator;

        /// <summary>
        /// Gets or sets whether the <see cref="NextValue">NextValue</see> will be included in the final result.
        /// </summary>
        public bool Capturing { get; set; } = true;

        /// <summary>
        /// Gets the <see cref="NextValue">NextValue</see>'s zero-based position in the input sequence.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets whether this is the end of the input sequence.
        /// </summary>
        public bool IsInputEnd { get; }

        /// <summary>
        /// Gets whether this is the beginning of the input sequence.
        /// </summary>
        public bool IsInputStart { get; } = false;

        /// <summary>
        /// Gets whether this is the beginning of a match attempt.
        /// </summary>
        public bool IsMatchStart => Previous == null;

        /// <summary>
        /// Gets the next value of the input sequence. Will be <c>default(<see cref="T"/>)</c> when <see cref="IsInputEnd">IsInputEnd</see> is <c>true</c>.
        /// </summary>
        public T NextValue { get; }

        /// <summary>
        /// Gets the previous state of the match attempt. Will be <c>null</c> when <see cref="IsMatchStart">IsMatchStart</see> is <c>true</c>.
        /// </summary>
        public MatchState<T>? Previous { get; }

        /// <summary>
        /// Creates a new match attempt at the beginning of the given input sequence.
        /// </summary>
        /// <param name="inputSequence">The input sequence to attempt to find a match in.</param>
        public MatchState(IEnumerable<T> inputSequence)
        {
            peekAheadEnumerator = new PeekAheadEnumerator(inputSequence);
            IsInputEnd = !peekAheadEnumerator.MoveNextAndResetPeek();
            NextValue = peekAheadEnumerator.Current;
            IsInputStart = true;
            Index = 0;
        }

        private MatchState(MatchState<T> previous, bool newMatch)
        {
            peekAheadEnumerator = previous.peekAheadEnumerator.Snapshot();
            IsInputEnd = newMatch ? (!peekAheadEnumerator.MoveToCurrentPeekPosition() || previous.IsInputEnd) : !peekAheadEnumerator.MoveNext();
            NextValue = peekAheadEnumerator.Current;
            Capturing = previous.Capturing || newMatch;
            Previous = newMatch ? null : previous;
            Index = newMatch ? previous.Index : previous.Index + 1;
        }

        /// <summary>
        /// Gets the current match sequence. That is: all previous match states before the current one, in input order.<br/>
        /// Will be zero length if <see cref="IsMatchStart">IsMatchStart</see> is <c>true</c>.
        /// </summary>
        /// <returns>The current match sequence.</returns>
        public IEnumerable<MatchState<T>> GetMatchSequence()
        {
            return GetParentSequence()
                .Skip(1)
                .Reverse();
        }

        /// <summary>
        /// Gets the previous match state sequence. That is: this and all previous match states in parent order.
        /// </summary>
        /// <returns>The previous match state sequence.</returns>
        public IEnumerable<MatchState<T>> GetParentSequence()
        {
            var current = this;
            do
            {
                yield return current;
                current = current.Previous;
            }
            while (!current!.IsMatchStart);

            yield return current;
        }

        /// <summary>
        /// Includes the current input value in the match by moving the end forward.
        /// </summary>
        /// <returns>The match state pointing at the next position in the input sequence.</returns>
        public MatchState<T> Next() => new(this, false);

        /// <summary>
        /// Sets the latest capture for the given capture reference.<br/>
        /// Should be used on the <see cref="Next"/> match state after the capture.
        /// </summary>
        /// <param name="captureReference">The reference to set the capture for.</param>
        /// <param name="capture">The captured sequence.</param>
        public void SetCapture(CaptureReference<T> captureReference, T[] capture)
            => captureState[captureReference] = capture;

        /// <summary>
        /// Attempts to get the state for the given capture reference on the current match only.
        /// </summary>
        /// <param name="captureReference">The reference to get the capture for.</param>
        /// <param name="capture">The captured sequence if this returns <c>true</c>, otherwise <see cref="Array.Empty{T}"/>.</param>
        /// <returns><c>true</c> if a state for the given capture reference was found on the current match, otherwise <c>false</c>.</returns>
        public bool TryGetCapture(CaptureReference<T> captureReference, out T[] capture)
        {
            if (captureState.TryGetValue(captureReference, out capture))
                return true;

            capture = Array.Empty<T>();
            return false;
        }

        /// <summary>
        /// Attempts to get the latest state for the given capture reference on the current match and any previous ones.
        /// </summary>
        /// <param name="captureReference">The reference to get the capture for.</param>
        /// <param name="capture">The captured sequence if this returns <c>true</c>, otherwise <see cref="Array.Empty{T}"/>.</param>
        /// <returns><c>true</c> if a state for the given capture reference was found, otherwise <c>false</c>.</returns>
        public bool TryGetLatestCapture(CaptureReference<T> captureReference, out T[] capture)
        {
            foreach (var matchElement in GetParentSequence())
                if (matchElement.TryGetCapture(captureReference, out capture))
                    return true;

            capture = Array.Empty<T>();
            return false;
        }

        internal MatchState<T> AsStart() => new(this, true);

        internal Match<T> GetMatch()
        {
            var matchSequence = GetMatchSequence().ToArray();
            var start = matchSequence[0].Index;
            var end = matchSequence[^1].Index;

            return new Match<T>(matchSequence.Select(element => element.NextValue!), matchSequence.Where(element => element.Capturing).Select(element => element.NextValue!), start, end);
        }
    }
}