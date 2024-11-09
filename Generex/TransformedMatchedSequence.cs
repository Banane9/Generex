using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class TransformedMatchedSequence<TIn, TOut> : MatchedSequence<TIn>
    {
        public TOut Transformed { get; }

        internal TransformedMatchedSequence(IEnumerable<MatchState<TIn>> matchSequence, TOut transformed)
            : base(matchSequence)
        {
            Transformed = transformed;
        }

        internal TransformedMatchedSequence(int index, TOut transformed)
            : base(index)
        {
            Transformed = transformed;
        }
    }
}