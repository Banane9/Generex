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

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            // Shouldn't happen, but just to be sure
            if (progress >= Maximum)
                yield break;

            foreach (var nextMatch in MatchNext(Atom, currentMatch, value))
            {
                // Advance progress and return match(es) when inner is done
                if (nextMatch.IsDone)
                {
                    var newProgress = progress + 1;

                    // Return incomplete match whenever progress isn't at the max yet
                    // This has to be done first, because otherwise the state may be changed and cloned wrongly
                    if (newProgress < Maximum)
                    {
                        var nextMatchClone = nextMatch.Clone();
                        nextMatchClone.SetState(this, newProgress);
                        nextMatchClone.IsDone = false;

                        yield return nextMatchClone;
                    }

                    // Return (additional) complete match whenever progress fits the quantifier
                    if (newProgress >= Minimum && newProgress <= Maximum)
                    {
                        // Reset state so this can be used again
                        nextMatch.SetState(this, 0);
                        yield return nextMatch;
                    }
                }
                else
                    yield return nextMatch;
            }
        }
    }
}