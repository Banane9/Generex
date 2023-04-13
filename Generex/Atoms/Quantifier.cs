using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class Quantifier<T> : UnaryModifier<T>
    {
        public int Maximum { get; }
        public int Minimum { get; }

        public Quantifier(Generex<T> atom, int minimum, int maximum) : base(atom)
        {
            if (minimum < 0)
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be at least 0.");

            if (maximum < 1 || maximum < minimum)
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be at least 1 and >= minium.");

            Minimum = minimum;
            Maximum = maximum;
        }

        public Quantifier(Generex<T> atom, int exactly) : this(atom, exactly, exactly)
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

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            var originalMatch = currentMatch.Clone();

            foreach (var quantityMatch in MatchQuantity(currentMatch))
                yield return quantityMatch;

            if (Minimum == 0)
            {
                originalMatch.IsMatchEnd = true;
                yield return originalMatch;
            }
        }

        private IEnumerable<MatchState<T>> MatchQuantity(MatchState<T> currentMatch, int progress = 1, bool tryWithoutNext = true)
        {
            foreach (var nextMatch in ContinueMatch(Atom, currentMatch))
            {
                nextMatch.IsMatchEnd = false;

                // Only recurse when it's not at the max number yet
                if (progress < Maximum && (tryWithoutNext || nextMatch.IsInputEnd))
                    foreach (var futureMatch in MatchQuantity(nextMatch, progress + 1, nextMatch.IsInputEnd))
                        yield return futureMatch;

                // Greedy order: shorter matches at the bottom
                if (progress >= Minimum)
                {
                    nextMatch.IsMatchEnd = true;
                    yield return nextMatch;
                }
            }
        }
    }
}