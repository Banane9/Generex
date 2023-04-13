using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class NonCapturingGroup<T> : UnaryModifier<T>
    {
        public NonCapturingGroup(Generex<T> atom) : base(atom)
        { }

        public override string ToString()
        {
            return $"(?:{Atom})";
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            var matchClone = currentMatch.Clone();
            matchClone.Capturing = false;

            foreach (var nextMatch in ContinueMatch(Atom, matchClone))
            {
                nextMatch.Capturing = true;
                yield return nextMatch;
            }
        }
    }
}