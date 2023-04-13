using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    public class Alternative<T> : Chain<T>
    {
        public Alternative(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : base(atom, furtherAtoms)
        { }

        public Alternative(IEnumerable<Generex<T>> atoms) : base(atoms)
        { }

        public override string ToString(bool grouped)
        {
            if (Length == 0)
                return Atoms.First().ToString(grouped);

            var alternatives = string.Join("|", Atoms.Select(atom => atom.ToString()));

            if (grouped)
                return $"({alternatives})";
            else
                return alternatives;
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
            => Atoms.SelectMany(atom => ContinueMatch(atom, currentMatch.Clone()));
    }
}