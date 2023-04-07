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
            public bool IsDone => MatchNextValue == null;
            public bool IsStart => Previous == null;

            public MatchNextValue? MatchNextValue { get; set; }
            public MatchElement? Previous { get; }

            public T? Value { get; }

            public MatchElement(MatchNextValue? matchNextValue = null)
            {
                MatchNextValue = matchNextValue;
            }

            public MatchElement(MatchElement previous, T value, MatchNextValue? matchNextValue = null)
                : this(matchNextValue)
            {
                Previous = previous;
                Value = value;
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

            public IEnumerable<MatchElement> MatchNext(T value) => MatchNextValue?.Invoke(this, value) ?? new[] { this };

            public MatchElement Next(T value, MatchNextValue? matchNextValue = null) => new(this, value, matchNextValue);
        }
    }
}