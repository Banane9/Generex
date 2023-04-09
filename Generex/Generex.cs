using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex
{
    public static class Generex
    {
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