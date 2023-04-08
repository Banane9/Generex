using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class Quantifier<T> : Atom<T>
    {
        public Atom<T> Atom { get; }

        public int Maximum { get; }
        public int Minimum { get; }

        public Quantifier(Atom<T> atom, int minimum, int maximum) : base(atom.EqualityComparer)
        {
            if (minimum < 0)
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be at least 0.");

            if (maximum < 1 || maximum < minimum)
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be at least 1 and >= minium.");

            Atom = atom;
            Minimum = minimum;
            Maximum = maximum;
        }

        public Quantifier(Atom<T> atom, int exactly) : this(atom, exactly, exactly)
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

            if (progress >= Maximum)
                yield break;

            foreach (var nextMatch in MatchNext(Atom, currentMatch, value))
            {
                if (nextMatch.IsDone)
                {
                    // Advance progress and return match(es)
                    var newProgress = progress + 1;
                    nextMatch.SetState(this, newProgress);

                    // Return complete match whenever progress fits the quantifier
                    if (newProgress >= Minimum && newProgress <= Maximum)
                        yield return nextMatch;

                    // Return (additional) incomplete match whenever progress isn't at the max yet
                    if (newProgress < Maximum)
                    {
                        var nextMatchClone = nextMatch.Clone();
                        nextMatchClone.IsDone = false;

                        yield return nextMatchClone;
                    }
                }
                else
                    yield return nextMatch;
            }
        }
    }
}