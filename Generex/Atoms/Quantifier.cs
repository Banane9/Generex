using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public abstract class Quantifier<T> : UnaryModifier<T>
    {
        public int Maximum { get; }
        public int Minimum { get; }

        protected Quantifier(Generex<T> atom, int minimum, int maximum) : base(atom)
        {
            if (minimum < 0)
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be at least 0.");

            if (maximum < 1 || maximum < minimum)
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be at least 1 and >= minium.");

            Minimum = minimum;
            Maximum = maximum;
        }

        protected Quantifier(Generex<T> atom, int exactly) : this(atom, exactly, exactly)
        { }

        public override string ToString()
        {
            if (Minimum == Maximum)
                return $"{Atom.ToString(true)}{{{Minimum}}}";

            if (Maximum == int.MaxValue)
                if (Minimum == 0)
                    return $"{Atom.ToString(true)}*";
                else if (Minimum == 1)
                    return $"{Atom.ToString(true)}+";
                else
                    return $"{Atom.ToString(true)}{{{Minimum},}}";

            return $"{Atom.ToString(true)}{{{Minimum},{Maximum}}}";
        }
    }
}