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
        private readonly Dictionary<CaptureReference<T>, MatchedSequence<T>> captureState = new();
        private readonly PeekAheadEnumerator peekAheadEnumerator;

        /// <summary>
        /// Gets all captures of the current match.
        /// </summary>
        public IEnumerable<KeyValuePair<CaptureReference<T>, MatchedSequence<T>>> CaptureState
        {
            get
            {
                foreach (var entry in captureState)
                    yield return entry;
            }
        }

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
        public bool IsMatchStart { get; }

        /// <summary>
        /// Gets the next value of the input sequence. Will be <c>default(<see cref="T"/>)</c> when <see cref="IsInputEnd">IsInputEnd</see> is <c>true</c>.
        /// </summary>
        public T NextValue { get; }

        /// <summary>
        /// Gets the previous state of the match attempt. Will be <c>null</c> when <see cref="IsMatchStart">IsMatchStart</see> is <c>true</c>.
        /// </summary>
        public MatchState<T>? Previous { get; }

        /// <summary>
        /// Gets the first state of this match attempt.
        /// </summary>
        public MatchState<T> Start { get; }

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
            IsMatchStart = true;
            Start = this;
            Index = 0;
        }

        private MatchState(MatchState<T> previous, bool newMatch)
        {
            peekAheadEnumerator = previous.peekAheadEnumerator.Snapshot();
            IsInputEnd = newMatch ? (!peekAheadEnumerator.MoveToCurrentPeekPosition() || previous.IsInputEnd) : !peekAheadEnumerator.MoveNext();
            NextValue = peekAheadEnumerator.Current;
            Capturing = previous.Capturing || newMatch;
            IsMatchStart = newMatch;
            Start = newMatch ? this : previous.Start;
            Previous = newMatch ? null : previous;
            Index = newMatch ? previous.Index : previous.Index + 1;
        }

        private MatchState(MatchState<T> template)
        {
            peekAheadEnumerator = template.peekAheadEnumerator.Snapshot();
            IsInputEnd = template.IsInputEnd;
            NextValue = template.NextValue;
            Capturing = template.Capturing;
            IsMatchStart = template.IsMatchStart;
            Start = template.Start;
            Previous = template.IsMatchStart ? null : new MatchState<T>(template.Previous!);
            Index = template.Index;
        }

        /// <summary>
        /// Creates a new <see cref="MatchState{T}"/> match chain and merges the <see cref="CaptureState">CaptureStates</see>
        /// of the <paramref name="source"/> into those of the <paramref name="target"/>.<para/>
        /// Input <see cref="MatchState{T}">MatchStates</see> must align with eachother.<br/>
        /// <see cref="MatchedSequence{T}">MatchSequences</see> must be identical when they appear in the same <see cref="MatchState{T}"/> position.
        /// </summary>
        /// <param name="target">The <see cref="MatchState{T}"/> to merge the <see cref="CaptureState">CaptureState</see> into.</param>
        /// <param name="source">The <see cref="MatchState{T}"/> to copy the <see cref="CaptureState">CaptureState</see> from.</param>
        /// <returns>The newly created and merged <see cref="MatchState{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">When one of the pre-conditions is broken.</exception>
        public static MatchState<T> Merge(MatchState<T> target, MatchState<T> source)
        {
            if (target != source)
                throw new InvalidOperationException("Can only merge MatchStates that align with eachother.");

            target = new MatchState<T>(target);
            var currentTarget = target;
            var currentSource = source;

            // Sequences should be identical, so only need to check one.
            // Don't need to include start one, as it should not have capture state.
            while (!currentTarget!.IsMatchStart)
            {
                foreach (var sourceCapture in currentSource!.captureState)
                {
                    if (currentTarget.captureState.TryGetValue(sourceCapture.Key, out var targetCaptureSequence))
                    {
                        if (!targetCaptureSequence.SequenceEqual(sourceCapture.Value))
                            throw new InvalidOperationException("MatchSequences for a given CaptureReference must be identical when they appear in the same MatchState position.");
                        else
                            continue;
                    }

                    currentTarget.captureState.Add(sourceCapture.Key, sourceCapture.Value);
                }

                currentTarget = currentTarget.Previous;
                currentSource = currentSource.Previous;
            }

            return target;
        }

        public static bool operator !=(MatchState<T>? leftMatch, MatchState<T>? rightMatch) => !(leftMatch == rightMatch);

        public static bool operator ==(MatchState<T>? leftMatch, MatchState<T>? rightMatch)
        {
            return ReferenceEquals(leftMatch, rightMatch)
                || (leftMatch is not null && rightMatch is not null
                    && ReferenceEquals(leftMatch.Start, rightMatch.Start)
                    && leftMatch.Index == rightMatch.Index);
        }

        /// <summary>
        /// Includes the current and every following input value in a new match by moving the end forward.
        /// </summary>
        /// <returns>The match states pointing at the next position in the input sequence.</returns>
        public IEnumerable<MatchState<T>> AllNext()
        {
            var current = this;
            while (!current.IsInputEnd)
            {
                current = current.Next();
                yield return current;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is MatchState<T> otherMatch && this == otherMatch;

        /// <inheritdoc/>
        public override int GetHashCode() => IsMatchStart ? base.GetHashCode() : unchecked(Start.GetHashCode() + Index);

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
            while (current != null);
        }

        /// <summary>
        /// Creates a new <see cref="MatchState{T}"/> match chain and merges the <see cref="CaptureState">CaptureStates</see>
        /// of the <paramref name="source"/> into the current one.<para/>
        /// Input <see cref="MatchState{T}">MatchStates</see> must align with the current one.<br/>
        /// <see cref="MatchedSequence{T}">MatchSequences</see> must be identical when they appear in the same <see cref="MatchState{T}"/> position.
        /// </summary>
        /// <param name="source">The <see cref="MatchState{T}"/> to copy the <see cref="CaptureState">CaptureState</see> from.</param>
        /// <returns>The newly created and merged <see cref="MatchState{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">When one of the pre-conditions is broken.</exception>
        public MatchState<T> Merge(MatchState<T> source) => Merge(this, source);

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
        public void SetCapture(CaptureReference<T> captureReference, IEnumerable<MatchState<T>> capture)
            => captureState[captureReference] = new MatchedSequence<T>(capture);

        /// <summary>
        /// Attempts to get the state for the given capture reference on the current match only.
        /// </summary>
        /// <param name="captureReference">The reference to get the capture for.</param>
        /// <param name="capture">The captured sequence if this returns <c>true</c>, otherwise <see cref="MatchedSequence{T}.Invalid"/>.</param>
        /// <returns><c>true</c> if a state for the given capture reference was found on the current match, otherwise <c>false</c>.</returns>
        public bool TryGetCapture(CaptureReference<T> captureReference, out MatchedSequence<T> capture)
        {
            if (captureState.TryGetValue(captureReference, out capture))
                return true;

            capture = MatchedSequence<T>.Invalid;
            return false;
        }

        /// <summary>
        /// Attempts to get the latest state for the given capture reference on the current match and any previous ones.
        /// </summary>
        /// <param name="captureReference">The reference to get the capture for.</param>
        /// <param name="capture">The captured sequence if this returns <c>true</c>, otherwise <see cref="MatchedSequence{T}.Invalid"/>.</param>
        /// <returns><c>true</c> if a state for the given capture reference was found, otherwise <c>false</c>.</returns>
        public bool TryGetLatestCapture(CaptureReference<T> captureReference, out MatchedSequence<T> capture)
        {
            foreach (var matchElement in GetParentSequence())
            {
                if (matchElement.TryGetCapture(captureReference, out capture))
                    return true;
            }

            capture = MatchedSequence<T>.Invalid;
            return false;
        }

        internal MatchState<T> AsStart() => new(this, true);

        internal Match<T> GetMatch()
        {
            var matchSequence = GetMatchSequence();

            if (matchSequence.Any(match => match.Capturing))
                return new Match<T>(matchSequence);

            if (matchSequence.Any())
                return new Match<T>(matchSequence, Index);

            return new Match<T>(Index);
        }
    }
}