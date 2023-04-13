using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Generex
{
    public sealed partial class MatchState<T>
    {
        private sealed class PeekAheadEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;
            private readonly List<T> peekedElements = new() { default! };
            private int currentIndex = 0;

            public T Current => peekedElements[currentIndex];

            object IEnumerator.Current => Current!;
            private int CurrentMaxPeakIndex => peekedElements.Count - 1;

            public PeekAheadEnumerator(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }

            public PeekAheadEnumerator(IEnumerable<T> enumerable)
                : this(enumerable.GetEnumerator())
            { }

            private PeekAheadEnumerator(PeekAheadEnumerator template)
            {
                enumerator = template.enumerator;
                peekedElements = template.peekedElements;
                currentIndex = template.currentIndex;
            }

            public void Dispose() => enumerator.Dispose();

            public bool MoveNext()
            {
                if (currentIndex == CurrentMaxPeakIndex)
                {
                    ++currentIndex;

                    if (enumerator.MoveNext())
                    {
                        peekedElements.Add(enumerator.Current);
                        return true;
                    }

                    peekedElements.Add(default!);
                    return false;
                }

                // When currentIndex before the end of peekedElements
                ++currentIndex;
                return true;
            }

            public bool MoveNextAndResetPeek()
            {
                ResetPeek();

                if (CurrentMaxPeakIndex == 0)
                {
                    if (enumerator.MoveNext())
                    {
                        peekedElements[0] = enumerator.Current;
                        return true;
                    }

                    peekedElements[0] = default!;
                    return false;
                }

                // When peekedElements.Count > 1
                peekedElements.RemoveAt(0);
                return true;
            }

            public int MoveToPeekedEnd(int offset = 0)
            {
                ResetPeek();
                var removed = CurrentMaxPeakIndex - offset;
                peekedElements.RemoveRange(0, CurrentMaxPeakIndex - offset);

                return removed;
            }

            public void Reset()
            {
                ResetPeek();
                enumerator.Reset();
                peekedElements.Clear();
                peekedElements.Add(default!);
            }

            public void ResetPeek() => currentIndex = 0;

            public PeekAheadEnumerator Snapshot() => new(this);
        }
    }
}