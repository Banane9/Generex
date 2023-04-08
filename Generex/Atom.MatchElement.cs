using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public abstract partial class Atom<T>
    {
        protected delegate IEnumerable<MatchElement> MatchNextValue(MatchElement currentMatch, T value);

        protected class MatchElement
        {
            private readonly Lazy<Dictionary<Atom<T>, object>> matchState = new();

            public bool IsDone { get; set; }

            public bool IsStart => Previous == null;

            public MatchElement? Previous { get; }

            public T? Value { get; }

            public MatchElement()
            { }

            public MatchElement(MatchElement previous, T value)
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

            public TState? GetLatestState<TState>(Atom<T> atom, TState? defaultState = default)
            {
                if (TryGetLatestState<TState>(atom, out var state))
                    return state;

                return defaultState;
            }

            public Match<T> GetMatch()
            {
                return new Match<T>(GetMatchSequence().Select(element => element.Value!));
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

            public void SetState<TState>(Atom<T> atom, TState state) => matchState.Value[atom] = state!;

            public bool TryGetLatestState<TState>(Atom<T> atom, out TState? state)
            {
                foreach (var matchElement in GetParentSequence())
                    if (matchElement.TryGetState(atom, out state))
                        return true;

                state = default;
                return false;
            }

            public bool TryGetState<TState>(Atom<T> atom, out TState? state)
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