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

        public IEnumerator<Atom<T>> GetEnumerator() => Atoms.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Atoms).GetEnumerator();

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            var progress = currentMatch.GetLatestState(this, 0);

            if (progress >= atoms.Length)
                yield break;

            foreach (var nextMatch in MatchNext(atoms[progress], currentMatch, value))
            {
                // Advance progress when nested match is complete
                if (nextMatch.IsDone)
                {
                    var newProgress = progress + 1;
                    nextMatch.SetState(this, newProgress);
                    nextMatch.IsDone = newProgress >= atoms.Length;
                }

                yield return nextMatch;
            }
        }
    }
}