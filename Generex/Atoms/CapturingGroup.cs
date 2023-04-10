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

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            foreach (var nextMatch in MatchNext(Atom, currentMatch, value))
            {
                if (nextMatch.IsDone)
                {
                    nextMatch.SetCapture(CaptureReference,
                        nextMatch.GetParentSequence()
                            .TakeWhile(match => match.GetState(this, false))
                            .Select(match => match.Value)
                            .ToArray());

                    // Set to false to break the TakeWhile of any later capture
                    nextMatch.SetState(this, false);
                }
                else
                    // Mark as part of the current capture
                    nextMatch.SetState(this, true);

                yield return nextMatch;
            }
        }
    }
}