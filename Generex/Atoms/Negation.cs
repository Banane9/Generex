using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class Negation<T> : Generex<T>
    {
        public Generex<T> Atom { get; }

        public Negation(Generex<T> atom)
        {
            Atom = atom;
        }

        public override string ToString()
            => $"(?!:{Atom})";

        protected override bool MatchEndInternal(MatchElement currentMatch)
            => !MatchEnd(Atom, currentMatch);

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var matches = false;
            foreach (var nextMatch in MatchNext(Atom, currentMatch, value))
            {
                matches = true;

                // Block done matches
                if (!nextMatch.IsDone)
                    yield return nextMatch;
            }

            if (!matches)
                yield return currentMatch.DoneWithNext(value);
        }
    }
}