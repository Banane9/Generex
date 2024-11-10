using System.Collections;
using System.Collections.Generic;

namespace Generex
{
    public sealed partial class MatchState<T>
    {
        private sealed class PeekAheadEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly List<T> _peekedElements = [default!];
            private int _currentIndex = 0;

            /// <inheritdoc/>
            public T Current => _peekedElements[_currentIndex];

            object IEnumerator.Current => Current!;

            private int CurrentMaxPeakIndex => _peekedElements.Count - 1;

            public PeekAheadEnumerator(IEnumerator<T> enumerator)
            {
                _enumerator = enumerator;
            }

            public PeekAheadEnumerator(IEnumerable<T> enumerable)
                : this(enumerable.GetEnumerator())
            { }

            private PeekAheadEnumerator(PeekAheadEnumerator template)
            {
                _enumerator = template._enumerator;
                _peekedElements = template._peekedElements;
                _currentIndex = template._currentIndex;
            }

            /// <inheritdoc/>
            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext()
            {
                lock (_peekedElements)
                {
                    if (_currentIndex == CurrentMaxPeakIndex)
                    {
                        ++_currentIndex;

                        if (_enumerator.MoveNext())
                        {
                            _peekedElements.Add(_enumerator.Current);
                            return true;
                        }

                        _peekedElements.Add(default!);
                        return false;
                    }

                    // When currentIndex before the end of peekedElements
                    ++_currentIndex;
                    return true;
                }
            }

            public bool MoveNextAndResetPeek()
            {
                lock (_peekedElements)
                {
                    ResetPeek();

                    if (CurrentMaxPeakIndex == 0)
                    {
                        if (_enumerator.MoveNext())
                        {
                            _peekedElements[0] = _enumerator.Current;
                            return true;
                        }

                        _peekedElements[0] = default!;
                        return false;
                    }

                    // When peekedElements.Count > 1
                    _peekedElements.RemoveAt(0);
                    return true;
                }
            }

            public bool MoveToCurrentPeekPosition()
            {
                lock (_peekedElements)
                {
                    _peekedElements.RemoveRange(0, _currentIndex);
                    ResetPeek();
                    return true;
                }
            }

            public void Reset()
            {
                lock (_peekedElements)
                {
                    ResetPeek();
                    _enumerator.Reset();
                    _peekedElements.Clear();
                    _peekedElements.Add(default!);
                }
            }

            public void ResetPeek() => _currentIndex = 0;

            public PeekAheadEnumerator Snapshot() => new(this);
        }
    }
}