using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Builders
{
    public class LiteralBuilder<T> : Builder<T>
    {
        private readonly T[] values;

        private LiteralBuilder(IEnumerable<T> values)
        {
            this.values = values.ToArray();
        }

        public override Atom<T> Finish()
            => new Sequence<T>(values);

        public class InProgress : OpenBuilder<LiteralBuilder<T>, InProgress>

        {
            private readonly List<T> values;

            public InProgress(IEnumerable<T> values)
            {
                this.values = values.ToList();
            }

            public override LiteralBuilder<T> End() => new(values);

            public InProgress FollowedBy(T value)
            {
                values.Add(value);
                return this;
            }

            public InProgress FollowedBy(IEnumerable<T> values)
            {
                this.values.AddRange(values);
                return this;
            }

            public InProgress FollowedBy(T value, params T[] furtherValues) => FollowedBy(value.Yield().Concat(furtherValues));
        }
    }
}