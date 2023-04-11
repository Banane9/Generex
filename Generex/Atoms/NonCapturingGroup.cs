using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class NonCapturingGroup<T> : Generex<T>
    {
        public Generex<T> Atom { get; }

        public NonCapturingGroup(Generex<T> atom)
        {
            Atom = atom;
        }

        public override string ToString()
        {
            return $"(?:{Atom})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch)
        {
            var matchClone = currentMatch.Clone();
            matchClone.Capturing = false;

            foreach (var nextMatch in MatchNext(Atom, matchClone))
            {
                nextMatch.Capturing = true;
                yield return nextMatch;
            }
        }
    }
}