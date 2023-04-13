using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class CapturingGroup<T> : UnaryModifier<T>
    {
        public CaptureReference<T> CaptureReference { get; }

        public CapturingGroup(Generex<T> atom, CaptureReference<T> captureReference) : base(atom)
        {
            CaptureReference = captureReference;
        }

        public override string ToString()
        {
            return $"(?'{CaptureReference}'{Atom})";
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
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