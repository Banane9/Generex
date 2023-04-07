using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Generex
{
    public class Sequence<T> : Atom<T>, IEnumerable<Atom<T>>
    {
        private readonly Atom<T>[] atoms;

        public IEnumerable<Atom<T>> Atoms
        {
            get
            {
                foreach (var atom in atoms)
                    yield return atom;
            }
        }

        public Sequence(params Atom<T>[] atoms) : base(atoms.First().EqualityComparer)
        {
            this.atoms = atoms;
        }

        public Sequence(IEnumerable<Atom<T>> atoms) : this(atoms.ToArray())
        { }

        public static implicit operator Sequence<T>(Atom<T>[] atoms) => new(atoms);

        public static implicit operator Sequence<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement match, T value)
        {
            return matchNext(match, value);
        }

        private MatchNextValue generateNextMatch(int currentSequenceIndex)
        {
            return (match, value) => matchNext(match, value, currentSequenceIndex);
        }

        private IEnumerable<MatchElement> matchNext(MatchElement match, T value, int currentSequenceIndex = 0)
        {
            foreach (var newMatch in MatchNext(atoms[currentSequenceIndex], match, value))
            {
                if (!newMatch.IsDone)
                    newMatch.MatchNextValue = generateNextMatch(currentSequenceIndex);
                else if (currentSequenceIndex + 1 < atoms.Length)
                    newMatch.MatchNextValue = generateNextMatch(currentSequenceIndex + 1);

                yield return newMatch;
            }
        }
    }
}