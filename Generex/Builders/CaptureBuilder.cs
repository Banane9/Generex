using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Builders
{
    public class CaptureBuilder<T> : Builder<T>
    {
        private readonly Builder<T> builder;

        private readonly CaptureReference? captureReference;

        private CaptureBuilder(Builder<T> builder, CaptureReference? captureReference = null)
        {
            this.builder = builder;
            this.captureReference = captureReference;
        }

        public override Atom<T> Finish()
        {
            if (captureReference == null)
                return new NonCapturingGroup<T>(builder.Finish());

            throw new NotImplementedException();
        }

        public class InProgress
        {
            private readonly Builder<T> builder;

            public CaptureBuilder<T> AsNothing => new(builder);

            internal InProgress(Builder<T> builder)
            {
                this.builder = builder;
            }

            public CaptureBuilder<T> As(out CaptureReference captureReference)
            {
                captureReference = new CaptureReference();
                return new(builder, captureReference);
            }
        }
    }
}