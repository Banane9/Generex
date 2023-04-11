using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class CapturingGroup<T> : Generex<T>
    {
        public Generex<T> Atom { get; }
        public CaptureReference<T> CaptureReference { get; }

        public CapturingGroup(CaptureReference<T> captureReference, Generex<T> atom)
        {
            CaptureReference = captureReference;
            Atom = atom;
        }

        public override string ToString()
        {
            return $"(?'{CaptureReference}'{Atom})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch)
        {
            foreach (var nextMatch in MatchNext(Atom, currentMatch))
            {
                nextMatch.SetCapture(CaptureReference,
                    nextMatch.GetParentSequence()
                        .Skip(1)
                        .TakeUntil(match => match != currentMatch)
                        .Select(match => match.NextValue!)
                        .Reverse()
                        .ToArray());

                yield return nextMatch;
            }
        }
    }
}