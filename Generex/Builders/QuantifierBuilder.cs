using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Builders
{
    public class QuantifierBuilder<T> : Builder<T>
    {
        private readonly Builder<T> builder;
        private readonly int maximum;
        private readonly int minimum;

        private QuantifierBuilder(Builder<T> builder, int minimum, int maximum)
        {
            this.builder = builder;
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public override Atom<T> Finish()
            => new Quantifier<T>(builder.Finish(), minimum, maximum);

        public class InProgress
        {
            private readonly Builder<T> builder;

            public QuantifierBuilder<T> Any => new(builder, 0, int.MaxValue);

            public QuantifierBuilder<T> AtLeastOnce => new(builder, 1, int.MaxValue);

            public QuantifierBuilder<T> AtMostOnce => new(builder, 0, 1);

            public QuantifierBuilder<T> this[int minimum, int maximum] => Range(minimum, maximum);

            internal InProgress(Builder<T> builder)
            {
                this.builder = builder;
            }

            public QuantifierBuilder<T> Exactly(int times) => new(builder, times, times);

            public QuantifierBuilder<T> Range(int minimum, int maximum) => new(builder, minimum, maximum);
        }
    }
}