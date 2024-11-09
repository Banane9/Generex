using EnumerableToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public sealed class Match<T> : MatchedSequence<T>
    {
        private readonly Dictionary<CaptureReference<T>, CaptureGroup> _capturedGroups;

        public MatchedSequence<T> FullMatch { get; }

        public IEnumerable<CaptureGroup> Groups => _capturedGroups.Values.AsSafeEnumerable();

        public CaptureGroup this[CaptureReference<T> captureReference] => _capturedGroups[captureReference];

        internal Match(IEnumerable<MatchState<T>> fullMatchSequence)
            : base(fullMatchSequence.Where(match => match.Capturing))
        {
            FullMatch = new(fullMatchSequence);
            _capturedGroups = CollectCaptureGroups(fullMatchSequence);
        }

        internal Match(int index) : base(index)
        {
            FullMatch = new(index);
            _capturedGroups = [];
        }

        internal Match(IEnumerable<MatchState<T>> fullMatchSequence, int index) : base(index)
        {
            FullMatch = new(fullMatchSequence);
            _capturedGroups = CollectCaptureGroups(fullMatchSequence);
        }

        private static Dictionary<CaptureReference<T>, CaptureGroup> CollectCaptureGroups(IEnumerable<MatchState<T>> fullMatchSequence)
        {
            return fullMatchSequence.SelectMany(match => match.CaptureState)
                .Aggregate(new Dictionary<CaptureReference<T>, List<MatchedSequence<T>>>(), (state, capture) =>
                    {
                        if (!state.TryGetValue(capture.Key, out var matches))
                        {
                            matches = [];
                            state.Add(capture.Key, matches);
                        }

                        matches.Add(capture.Value);
                        return state;
                    })
                .ToDictionary(captureGroups => captureGroups.Key, captureGroups => new CaptureGroup(captureGroups.Key, captureGroups.Value));
        }

        public class CaptureGroup : IEnumerable<MatchedSequence<T>>
        {
            protected readonly MatchedSequence<T>[] captures;

            public CaptureReference<T> CaptureReference { get; }

            public MatchedSequence<T> First => captures[0];

            public MatchedSequence<T> Last => captures[^1];

            internal CaptureGroup(CaptureReference<T> captureReference, IEnumerable<MatchedSequence<T>> captures)
            {
                CaptureReference = captureReference;
                this.captures = captures.ToArray();
            }

            public IEnumerator<MatchedSequence<T>> GetEnumerator()
                => ((IEnumerable<MatchedSequence<T>>)captures).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => captures.GetEnumerator();
        }

        public class TransformedCaptureGroup<TOut> : CaptureGroup, IEnumerable<TransformedMatchedSequence<T, TOut>>
        {
            public new TransformingCaptureReference<T, TOut> CaptureReference
                => (TransformingCaptureReference<T, TOut>)base.CaptureReference;

            public new TransformedMatchedSequence<T, TOut> First => (TransformedMatchedSequence<T, TOut>)captures[0];

            public new TransformedMatchedSequence<T, TOut> Last => (TransformedMatchedSequence<T, TOut>)captures[^1];

            internal TransformedCaptureGroup(TransformingCaptureReference<T, TOut> captureReference, IEnumerable<TransformedMatchedSequence<T, TOut>> captures)
                : base(captureReference, captures)
            { }

            public new IEnumerator<TransformedMatchedSequence<T, TOut>> GetEnumerator()
                => captures.Cast<TransformedMatchedSequence<T, TOut>>().GetEnumerator();
        }
    }
}