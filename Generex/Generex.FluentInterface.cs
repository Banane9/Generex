using Generex.Atoms;
using Generex.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public static class Generex
    {
        public static class CapturedGroup
        {
            public static ICapturedGroupEnd<T> ReferringBackTo<T>(CaptureReference<T> captureReference) => new Fluent.CapturedGroup<T>().ReferringBackTo(captureReference);
        }

        public static class From
        {
            public static Generex<T> Alternatives<T>(IEnumerable<Generex<T>> atoms) => new Atoms.Alternative<T>(atoms);

            public static Generex<T> Alternatives<T>(Generex<T> atom, params Generex<T>[] furtherAtoms) => Alternatives(atom.Yield().Concat(furtherAtoms));

            public static Generex<T> Literal<T>(T literal) => literal;

            public static Generex<T> Literals<T>(IEnumerable<T> literals) => Sequence(literals.Select(Literal));

            public static Generex<T> Literals<T>(T literal, params T[] extraLiterals) => Literals(literal.Yield().Concat(extraLiterals));

            public static Generex<T> Sequence<T>(IEnumerable<Generex<T>> atoms) => new Atoms.Sequence<T>(atoms);

            public static Generex<T> Sequence<T>(Generex<T> atom, params Generex<T>[] furtherAtoms) => Sequence(atom.Yield().Concat(furtherAtoms));
        }

        public static class Literal
        {
            public static ILiteral<T> Of<T>(IEnumerable<T> literals) => new Fluent.Literal<T>().Of(literals);

            public static ILiteral<T> Of<T>(T literal, params T[] extraLiterals) => new Fluent.Literal<T>().Of(literal.Yield().Concat(extraLiterals));

            public static IComparingLiteral<T> Using<T>(IEqualityComparer<T> equalityComparer) => new Fluent.Literal<T>().Using(equalityComparer);
        }

        public static class Range
        {
            public static IOpenRange<T> From<T>(T minium) => new Fluent.Range<T>().From(minium);

            public static IRange<T> Of<T>(T literal) => new Fluent.Range<T>().Of(literal);

            public static IRangeAddition<T> Using<T>(IComparer<T> comparer) => new Fluent.Range<T>().Using(comparer);
        }

        public static class Sequence
        {
            public static ISequenceAtom<T> Of<T>(IFinishableAtom<T> atom, params IFinishableAtom<T>[] furtherAtoms)
            {
                var sequence = new Fluent.Sequence<T>(atom);
                sequence.AddRange(furtherAtoms);

                return sequence;
            }

            public static ISequenceAtom<T> Of<T>(IEnumerable<IFinishableAtom<T>> atoms)
            {
                var sequence = new Fluent.Sequence<T>(atoms.First());
                sequence.AddRange(atoms.Skip(1));

                return sequence;
            }
        }
    }
}