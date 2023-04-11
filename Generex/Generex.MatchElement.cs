using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Generex
{
    public abstract partial class Generex<T>
    {
        protected delegate IEnumerable<MatchElement> MatchNextValue(MatchElement currentMatch, T value);

        protected class MatchElement
        {
            private readonly Lazy<Dictionary<CaptureReference<T>, T[]>> captureState = new();
            private readonly Lazy<Dictionary<Generex<T>, object>> matchState = new();
            private readonly PeekAheadEnumerator<T> peekAheadEnumerator;
            public bool Capturing { get; set; } = true;
            public bool HasNext { get; }
            public int Index { get; set; }
            public bool IsDone { get; set; }

            public bool IsStart => Previous == null;
            public T NextValue { get; }
            public MatchElement? Previous { get; }

            public MatchElement(MatchElement template, int index)
            {
                peekAheadEnumerator = template.peekAheadEnumerator.Snapshot();
                NextValue = template.NextValue;
                Capturing = template.Capturing;
                Previous = template.Previous;
                HasNext = template.HasNext;
                IsDone = template.IsDone;
                Index = index;
            }

            public MatchElement(IEnumerable<T> inputSequence)
            {
                peekAheadEnumerator = new PeekAheadEnumerator<T>(inputSequence);
                HasNext = peekAheadEnumerator.MoveNextAndResetPeek();
                NextValue = peekAheadEnumerator.Current;
                Index = -1;
            }

            public MatchElement(MatchElement previous)
            {
                peekAheadEnumerator = previous.peekAheadEnumerator.Snapshot();
                HasNext = peekAheadEnumerator.MoveNext();
                NextValue = peekAheadEnumerator.Current;
                Capturing = previous.Capturing;
                Previous = previous;
                Index = previous.Index + 1;
            }

            public MatchElement Clone()
            {
                var clone = new MatchElement(this, Index);

                if (matchState.IsValueCreated)
                    foreach (var state in matchState.Value)
                        clone.SetState(state.Key, state.Value);

                if (captureState.IsValueCreated)
                    foreach (var state in captureState.Value)
                        clone.SetCapture(state.Key, state.Value);

                return clone;
            }

            public MatchElement DoneWithNext() => new(this) { IsDone = true };

            public TState? GetLatestState<TState>(Generex<T> atom, TState? defaultState = default)
            {
                if (TryGetLatestState<TState>(atom, out var state))
                    return state;

                return defaultState;
            }

            public Match<T> GetMatch()
            {
                var matchSequence = GetMatchSequence().ToArray();
                var start = matchSequence[0].Index + 1;
                var end = matchSequence[^1].Index + 1;

                return new Match<T>(matchSequence.Select(element => element.NextValue!), matchSequence.Where(element => element.Capturing).Select(element => element.NextValue!), start, end);
            }

            public IEnumerable<MatchElement> GetMatchSequence()
            {
                return GetParentSequence()
                    .Skip(1)
                    .Reverse();
            }

            public IEnumerable<MatchElement> GetParentSequence()
            {
                var current = this;
                do
                {
                    yield return current;
                    current = current.Previous;
                }
                while (!current!.IsStart && !current.IsDone);

                yield return current;
            }

            public TState? GetState<TState>(Generex<T> atom, TState? defaultState = default)
            {
                if (TryGetState<TState>(atom, out var state))
                    return state;

                return defaultState;
            }

            public MatchElement Next() => new(this);

            public void SetCapture(CaptureReference<T> captureReference, T[] capture)
                => captureState.Value[captureReference] = capture;

            public void SetState<TState>(Generex<T> atom, TState state)
                => matchState.Value[atom] = state!;

            public bool TryGetCapture(CaptureReference<T> captureReference, out T[] capture)
            {
                if (captureState.IsValueCreated && captureState.Value.TryGetValue(captureReference, out capture))
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

            public bool TryGetLatestState<TState>(Generex<T> atom, out TState? state)
            {
                foreach (var matchElement in GetParentSequence())
                    if (matchElement.TryGetState(atom, out state))
                        return true;

                state = default;
                return false;
            }

            public bool TryGetState<TState>(Generex<T> atom, out TState? state)
            {
                if (matchState.IsValueCreated && matchState.Value.TryGetValue(atom, out var value))
                {
                    state = (TState)value;
                    return true;
                }

                state = default;
                return false;
            }
        }
    }
}