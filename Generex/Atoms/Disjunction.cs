﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a range of sub-patterns, at least one of which needs to match.
    /// </summary>
    /// <inheritdoc/>
    public class Disjunction<T> : Junction<T>
    {
        public Disjunction(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : base(atom, furtherAtoms)
        { }

        public Disjunction(IEnumerable<Generex<T>> atoms) : base(atoms)
        { }

        /// <inheritdoc/>
        public override string ToString(bool grouped)
        {
            if (Length == 1)
                return Atoms.First().ToString(grouped);

            var disjuncts = string.Join("|", Atoms.Select(atom => atom.ToString()));

            if (grouped)
                return $"({disjuncts})";
            else
                return disjuncts;
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (Length == 0)
                return [];

            if (Length == 1)
                return ContinueMatch(Atoms.First(), currentMatch);

            return MatchDisjunctions(currentMatch);
        }

        private IEnumerable<MatchState<T>> MatchDisjunctions(MatchState<T> currentMatch)
        {
            var results = new HashSet<MatchState<T>>();
            foreach (var nextMatch in Atoms.SelectMany(atom => ContinueMatch(atom, currentMatch)))
            {
                if (results.Add(nextMatch))
                    yield return nextMatch;
            }
        }
    }
}