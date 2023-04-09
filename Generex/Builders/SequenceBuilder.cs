using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Builders
{
    public class SequenceBuilder<T> : GroupBuilder<T>
    {
        private SequenceBuilder(IEnumerable<Builder<T>> builders) : base(builders)
        { }

        public override Atom<T> Finish()
            => new Sequence<T>(builders.Select(b => b.Finish()));

        public class InProgress : OpenGroupBuilder<SequenceBuilder<T>, InProgress>
        {
            public InProgress(IEnumerable<Builder<T>> builders) : base(builders)
            { }

            public override SequenceBuilder<T> End() => new(builders);

            public InProgress FollowedBy(Builder<T> builder)
            {
                builders.Add(builder);
                return this;
            }
        }
    }
}