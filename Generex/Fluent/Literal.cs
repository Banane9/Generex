using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Fluent
{
    public interface IAlternativeComparingLiteral<T>
    {
        IAlternativeLiteral<T> Of(IEnumerable<T> literals);

        IAlternativeLiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    public interface IAlternativeLiteral<T> : IAlternativeAtom<T>
    {
        IAlternativeLiteral<T> And(T literal, params T[] furtherLiterals);

        IAlternativeLiteral<T> And(IEnumerable<T> literals);
    }

    public interface IAlternativeLiteralStart<T> : IAlternativeComparingLiteral<T>
    {
        IAlternativeComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    public interface IComparingLiteral<T>
    {
        ILiteral<T> Of(IEnumerable<T> literals);

        ILiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    public interface ILiteral<T> : IAtom<T>
    {
        ILiteral<T> And(T literal, params T[] furtherLiterals);

        ILiteral<T> And(IEnumerable<T> literals);
    }

    public interface ILiteralStart<T> : IComparingLiteral<T>
    {
        IComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    public interface ISequenceComparingLiteral<T>
    {
        ISequenceLiteral<T> Of(IEnumerable<T> literals);

        ISequenceLiteral<T> Of(T literal, params T[] furtherLiterals);
    }

    public interface ISequenceLiteral<T> : ISequenceAtom<T>
    {
        ISequenceLiteral<T> And(T literal, params T[] furtherLiterals);

        ISequenceLiteral<T> And(IEnumerable<T> literals);
    }

    public interface ISequenceLiteralStart<T> : ISequenceComparingLiteral<T>
    {
        ISequenceComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer);
    }

    internal class Literal<T> : Atom<T>, ILiteral<T>, ILiteralStart<T>, IAlternativeLiteral<T>, IAlternativeLiteralStart<T>, ISequenceLiteral<T>, ISequenceLiteralStart<T>
    {
        private readonly List<T> literals = new();
        private IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

        public Literal(IParentAtom<T>? parent = null) : base(parent)
        { }

        ISequenceLiteral<T> ISequenceLiteral<T>.And(T literal, params T[] furtherLiterals)
            => ((ISequenceLiteral<T>)this).And(literal.Yield().Concat(furtherLiterals));

        ISequenceLiteral<T> ISequenceLiteral<T>.And(IEnumerable<T> literals)
        {
            this.literals.AddRange(literals);
            return this;
        }

        IAlternativeLiteral<T> IAlternativeLiteral<T>.And(T literal, params T[] furtherLiterals) =>
            ((IAlternativeLiteral<T>)this).And(literal.Yield().Concat(furtherLiterals));

        IAlternativeLiteral<T> IAlternativeLiteral<T>.And(IEnumerable<T> literals)
        {
            this.literals.AddRange(literals);
            return this;
        }

        public ILiteral<T> And(T literal, params T[] furtherLiterals)
            => And(literal.Yield().Concat(furtherLiterals));

        public ILiteral<T> And(IEnumerable<T> literals)
        {
            this.literals.AddRange(literals);
            return this;
        }

        ISequenceLiteral<T> ISequenceComparingLiteral<T>.Of(IEnumerable<T> literals)
                    => ((ISequenceLiteral<T>)this).And(literals);

        ISequenceLiteral<T> ISequenceComparingLiteral<T>.Of(T literal, params T[] extraLiterals)
            => ((ISequenceLiteral<T>)this).And(literal, extraLiterals);

        public ILiteral<T> Of(IEnumerable<T> literals) => And(literals);

        public ILiteral<T> Of(T literal, params T[] furtherLiterals) => And(literal, furtherLiterals);

        IAlternativeLiteral<T> IAlternativeComparingLiteral<T>.Of(IEnumerable<T> literals)
            => ((IAlternativeLiteral<T>)this).And(literals);

        IAlternativeLiteral<T> IAlternativeComparingLiteral<T>.Of(T literal, params T[] furtherLiterals)
            => ((IAlternativeLiteral<T>)this).And(literal, furtherLiterals);

        public IComparingLiteral<T> Using(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            return this;
        }

        IAlternativeComparingLiteral<T> IAlternativeLiteralStart<T>.Using(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            return this;
        }

        ISequenceComparingLiteral<T> ISequenceLiteralStart<T>.Using(IEqualityComparer<T> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            return this;
        }

        protected override Generex<T> FinishInternal()
        {
            if (literals.Count == 1)
                return new Atoms.Literal<T>(literals[0], equalityComparer);

            return new Atoms.Sequence<T>(literals.Select(literal => new Atoms.Literal<T>(literal, equalityComparer)));
        }
    }
}