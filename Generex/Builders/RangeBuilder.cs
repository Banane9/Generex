using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Builders
{
    public class RangeBuilder<T> : Builder<T>
    {
        private readonly List<LiteralRange<T>> ranges;

        private RangeBuilder(IEnumerable<LiteralRange<T>> ranges)
        {
            this.ranges = ranges.ToList();
        }

        public override Atom<T> Finish()
            => new Range<T>(ranges);

        public class InProgress : OpenBuilder<RangeBuilder<T>, InProgress>
        {
            private readonly List<LiteralRange<T>> ranges;

            public InProgress(IEnumerable<LiteralRange<T>> ranges)
            {
                this.ranges = ranges.ToList();
            }

            public override RangeBuilder<T> End() => new(ranges);

            public InProgress Or(LiteralRange<T> range)
            {
                ranges.Add(range);
                return this;
            }
        }
    }
}