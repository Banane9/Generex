using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Builders
{
    public class CaptureBuilder<T> : Builder<T>
    {
        private readonly Builder<T> builder;

        // Make this a special object used for referring back to a group
        private readonly object? captureIdentifier;

        private CaptureBuilder(Builder<T> builder, object? captureIdentifier = null)
        {
            this.builder = builder;
            this.captureIdentifier = captureIdentifier;
        }

        public override Atom<T> Finish()
        {
            if (captureIdentifier == null)
                return new NonCapturing<T>(builder.Finish());

            throw new NotImplementedException();
        }

        public class InProgress
        {
            private readonly Builder<T> builder;

            public CaptureBuilder<T> Nothing => new(builder);

            internal InProgress(Builder<T> builder)
            {
                this.builder = builder;
            }

            public CaptureBuilder<T> As(out object captureIdentifier)
            {
                captureIdentifier = new object();
                return new(builder, captureIdentifier);
            }
        }
    }
}