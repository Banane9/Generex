using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Ensures that the pattern inside this can only be matched all-or-nothing,
    /// preventing any backtracking into it.
    /// </summary>
    public class AtomicGroup<T> : UnaryModifier<T>
    {
        public AtomicGroup(Generex<T> atom) : base(atom)
        { }

        public override string ToString(bool grouped)
            => $"(?>{Atom})";

        protected override IEnumerable<MatchElement<T>> MatchNextInternal(MatchElement<T> currentMatch)
        {
            if (MatchNext(Atom, currentMatch).FirstOrDefault() is MatchElement<T> matchChoice)
                yield return matchChoice;

            yield break;
        }
    }
}