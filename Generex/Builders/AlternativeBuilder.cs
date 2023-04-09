using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Builders
{
    public class AlternativeBuilder<T> : GroupBuilder<T>
    {
        private AlternativeBuilder(IEnumerable<Builder<T>> builders) : base(builders)
        { }

        public override Atom<T> Finish()
            => new Alternative<T>(builders.Select(b => b.Finish()));

        public class InProgress : OpenGroupBuilder<AlternativeBuilder<T>, InProgress>
        {
            public InProgress(IEnumerable<Builder<T>> builders) : base(builders)
            { }

            public override AlternativeBuilder<T> End() => new(builders);

            public InProgress Or(Builder<T> builder)
            {
                builders.Add(builder);
                return this;
            }
        }
    }
}