using Generex.Atoms;
using Generex.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public static class Generex
    {
        public static class Build
        {
            public static AlternativeBuilder<T>.InProgress Alternatives<T>(IEnumerable<Builder<T>> builders) => new(builders);

            public static AlternativeBuilder<T>.InProgress Alternatives<T>(Builder<T> builder, params Builder<T>[] furtherBuilders) => new(builder.Yield().Concat(furtherBuilders));

            public static LiteralBuilder<T>.InProgress Literal<T>(T literal, params T[] furtherLiterals) => new(literal.Yield().Concat(furtherLiterals));

            public static LiteralBuilder<T>.InProgress Literal<T>(IEnumerable<T> literals) => new(literals);

            public static RangeBuilder<T>.InProgress Range<T>(LiteralRange<T> range, params LiteralRange<T>[] furtherRanges) => new(range.Yield().Concat(furtherRanges));

            public static RangeBuilder<T>.InProgress Range<T>(IEnumerable<LiteralRange<T>> ranges) => new(ranges);

            public static SequenceBuilder<T>.InProgress Sequence<T>(Builder<T> builder, params Builder<T>[] furtherBuilders) => new(builder.Yield().Concat(furtherBuilders));

            public static SequenceBuilder<T>.InProgress Sequence<T>(IEnumerable<Builder<T>> builders) => new(builders);
        }

        public static class From
        {
            public static Atom<T> Alternatives<T>(IEnumerable<Atom<T>> atoms) => new Alternative<T>(atoms);

            public static Atom<T> Alternatives<T>(Atom<T> atom, params Atom<T>[] furtherAtoms) => Alternatives(atom.Yield().Concat(furtherAtoms));

            public static Atom<T> Literal<T>(T literal) => literal;

            public static Atom<T> Literals<T>(IEnumerable<T> literals) => Sequence(literals.Select(Literal));

            public static Atom<T> Literals<T>(T literal, params T[] extraLiterals) => Literals(literal.Yield().Concat(extraLiterals));

            public static Atom<T> Sequence<T>(IEnumerable<Atom<T>> atoms) => new Sequence<T>(atoms);

            public static Atom<T> Sequence<T>(Atom<T> atom, params Atom<T>[] furtherAtoms) => Sequence(atom.Yield().Concat(furtherAtoms));
        }

        public static class Repeat
        { }
    }
}