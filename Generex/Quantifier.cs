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

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement match, T value)
        {
            var progress = match.GetLatestState(this, 0);

            if (progress >= Maximum)
                yield break;

            foreach (var nextMatch in MatchNext(Atom, match, value))
            {
                if (nextMatch.IsDone)
                {
                    nextMatch.SetState(this, progress + 1);
                    nextMatch
                }
            }
        }

        private MatchNextValue generateMatchNext(int count)
        {
            return (match, value) => matchNext(match, value, count);
        }

        private IEnumerable<MatchElement> matchNext(MatchElement match, T value, int count = 0)
        {
            MatchNext(Atom, ma)
        }
    }
}