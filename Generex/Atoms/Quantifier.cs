using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class Quantifier<T> : Generex<T>
    {
        public Generex<T> Atom { get; }

        public int Maximum { get; }
        public int Minimum { get; }

        public Quantifier(Generex<T> atom, int minimum, int maximum)
        {
            if (minimum < 0)
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be at least 0.");

            if (maximum < 1 || maximum < minimum)
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be at least 1 and >= minium.");

            Atom = atom;
            Minimum = minimum;
            Maximum = maximum;
        }

        public Quantifier(Generex<T> atom, int exactly) : this(atom, exactly, exactly)
        { }

        public override string ToString()
        {
            if (Minimum == Maximum)
                return $"{Atom}{{{Minimum}}}";

            if (Maximum == int.MaxValue)
                if (Minimum == 0)
                    return $"{Atom}*";
                else if (Minimum == 1)
                    return $"{Atom}+";
                else
                    return $"{Atom}{{{Minimum},}}";

            return $"{Atom}{{{Minimum},{Maximum}}}";
        }

        protected override bool MatchEndInternal(MatchElement currentMatch)
        {
            // Only approve zero-width matches at the end.
            // Others were already returned in the last MatchNextInternal.
            var progress = currentMatch.GetLatestState(this, 0);
            return progress == 0 && Minimum == 0;
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch)
        {
            var originalMatch = currentMatch.Clone();

            foreach (var quantityMatch in MatchQuantity(currentMatch))
                yield return quantityMatch;

            if (Minimum == 0)
            {
                originalMatch.IsDone = true;
                yield return originalMatch;
            }
        }

        private IEnumerable<MatchElement> MatchQuantity(MatchElement currentMatch, int progress = 1, bool tryWithoutNext = true)
        {
            foreach (var nextMatch in MatchNext(Atom, currentMatch))
            {
                nextMatch.IsDone = false;

                // Only recurse when it's not at the max number yet
                if (progress < Maximum && (tryWithoutNext || nextMatch.HasNext))
                    foreach (var futureMatch in MatchQuantity(nextMatch, progress + 1, nextMatch.HasNext))
                        yield return futureMatch;

                // Greedy order: shorter matches at the bottom
                if (progress >= Minimum)
                {
                    nextMatch.IsDone = true;
                    yield return nextMatch;
                }
            }
        }
    }
}