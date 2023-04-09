using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public abstract partial class Generex<T>
    {
        protected delegate IEnumerable<MatchElement> MatchNextValue(MatchElement currentMatch, T value);

        protected class MatchElement
        {
            private readonly Lazy<Dictionary<Generex<T>, object>> matchState = new();

            public bool Capturing { get; set; } = true;
            public int Index { get; set; }
            public bool IsDone { get; set; }

            public bool IsStart => Previous == null;
            public MatchElement? Previous { get; }

            public T? Value { get; }

            public MatchElement(int index = -1)
            {
                Index = index;
            }

            public MatchElement(MatchElement previous, T value) : this(previous.Index + 1)
            {
                Previous = previous;
                Value = value;
            }

            public MatchElement Clone()
            {
                var clone = new MatchElement(Previous!, Value!);

                if (matchState.IsValueCreated)
                    foreach (var state in matchState.Value)
                        clone.SetState(state.Key, state.Value);

                return clone;
            }

            public MatchElement DoneWithNext(T value) => new(this, value) { IsDone = true };

            public TState? GetLatestState<TState>(Generex<T> atom, TState? defaultState = default)
            {
                if (TryGetLatestState<TState>(atom, out var state))
                    return state;

                return defaultState;
            }

            public Match<T> GetMatch()
            {
                var matchSequence = GetMatchSequence().ToArray();
                var start = IsStart ? Index : matchSequence[0].Index;
                var end = matchSequence[^1].Index;

                return new Match<T>(matchSequence.Select(element => element.Value!), matchSequence.Where(element => element.Capturing).Select(element => element.Value!), start, end);
            }

            public IEnumerable<MatchElement> GetMatchSequence()
            {
                var matchStack = new Stack<MatchElement>();
                var current = this;

                while (!current!.IsStart)
                {
                    matchStack.Push(current);
                    current = current.Previous;
                }

                return matchStack.ToArray();
            }

            public MatchElement Next(T value) => new(this, value);

            public void SetState<TState>(Generex<T> atom, TState state) => matchState.Value[atom] = state!;

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

            protected IEnumerable<MatchElement> GetParentSequence()
            {
                var current = this;
                while (!current!.IsStart)
                {
                    yield return current;
                    current = current.Previous;
                }
            }
        }
    }
}