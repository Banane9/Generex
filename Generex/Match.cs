using EnumerableToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public sealed class Match<T> : MatchedSequence<T>
    {
        private readonly Dictionary<ICaptureReference<T>, ICaptureGroup<T>> _capturedGroups;

        public MatchedSequence<T> FullMatch { get; }

        public IEnumerable<ICaptureGroup<T>> Groups => _capturedGroups.Values.AsSafeEnumerable();

        public ICaptureGroup<T> this[ICaptureReference<T> captureReference] => _capturedGroups[captureReference];

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

        public TCaptureGroup GetCaptureGroup<TCapture, TCaptureReference, TCaptureGroup>(CaptureReferenceBase<T, TCapture, TCaptureReference, TCaptureGroup> captureReference)
            where TCapture : MatchedSequence<T>
            where TCaptureReference : CaptureReferenceBase<T, TCapture, TCaptureReference, TCaptureGroup>
            where TCaptureGroup : CaptureGroupBase<T, TCapture, TCaptureReference, TCaptureGroup>
            => (TCaptureGroup)_capturedGroups[captureReference];

        private static Dictionary<ICaptureReference<T>, ICaptureGroup<T>> CollectCaptureGroups(IEnumerable<MatchState<T>> fullMatchSequence)
        {
            return fullMatchSequence.SelectMany(match => match.CaptureState)
                .Aggregate(new Dictionary<ICaptureReference<T>, List<MatchedSequence<T>>>(), (state, capture) =>
                    {
                        if (!state.TryGetValue(capture.Key, out var matches))
                        {
                            matches = [];
                            state.Add(capture.Key, matches);
                        }

                        matches.Add(capture.Value);
                        return state;
                    })
                .ToDictionary(captureGroups => captureGroups.Key, captureGroups => captureGroups.Key.GetCaptureGroup(captureGroups.Value));
        }
    }
}