using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class NonCapturingGroup<T> : UnaryModifier<T>
    {
        public NonCapturingGroup(Generex<T> atom) : base(atom)
        { }

        public override string ToString(bool grouped)
        {
            return $"(?:{Atom})";
        }

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
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