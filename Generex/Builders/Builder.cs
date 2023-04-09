using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Builders
{
    public abstract class Builder<T>
    {
        public CaptureBuilder<T>.InProgress Capture => new(this);

        public QuantifierBuilder<T>.InProgress Repeat => new(this);

        public abstract Atom<T> Finish();

        public abstract class OpenBuilder<TBuilder, TOpenBuilder>
            where TBuilder : Builder<T>
            where TOpenBuilder : OpenBuilder<TBuilder, TOpenBuilder>
        {
            public CaptureBuilder<T>.InProgress Capture => new(End());

            public QuantifierBuilder<T>.InProgress Repeat => new(End());

            public abstract TBuilder End();

            public TOpenBuilder SnapshotTo(out TBuilder builder)
            {
                builder = End();
                return (TOpenBuilder)this;
            }
        }
    }

    public abstract class GroupBuilder<T> : Builder<T>
    {
        protected readonly Builder<T>[] builders;

        protected GroupBuilder(IEnumerable<Builder<T>> builders)
        {
            this.builders = builders.ToArray();
        }

        public abstract class OpenGroupBuilder<TBuilder, TOpenBuilder> : OpenBuilder<TBuilder, TOpenBuilder>
            where TBuilder : GroupBuilder<T>
            where TOpenBuilder : OpenGroupBuilder<TBuilder, TOpenBuilder>
        {
            protected readonly List<Builder<T>> builders;

            protected OpenGroupBuilder(IEnumerable<Builder<T>> builders)
            {
                this.builders = builders.ToList();
            }
        }
    }
}