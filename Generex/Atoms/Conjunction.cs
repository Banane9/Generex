using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generex.Atoms
{
    /// <summary>
    /// Represents a range of sub-patterns, all of which need to match with the same length.
    /// </summary>
    /// <inheritdoc/>
    public class Conjunction<T> : Junction<T>
    {
        public Conjunction(Generex<T> atom, params Generex<T>[] furtherAtoms)
            : base(atom, furtherAtoms)
        { }

        public Conjunction(IEnumerable<Generex<T>> atoms) : base(atoms)
        { }

        /// <inheritdoc/>
        public override string ToString(bool grouped)
        {
            if (Length == 1)
                return Atoms.First().ToString(grouped);

            var conjuncts = string.Join("&", Atoms.Select(atom => atom.ToString()));

            if (grouped)
                return $"({conjuncts})";
            else
                return conjuncts;
        }

        protected override IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch)
        {
            if (Length == 0)
                return currentMatch.AllNext();

            if (Length == 1)
                return ContinueMatch(Atoms.First(), currentMatch);

            var results = Atoms.Select(atom => ContinueMatch(atom, currentMatch)).ToArray();

            var i = -1;
            var result = new List<MatchState<T>>(results[0]);
            var resultTemp = new List<MatchState<T>>(result.Count);
            var conjuncts = results.Skip(1).Select(r => r.ToDictionary(m => m)).ToArray();

            // Filter through all resulting Match states to
            // only take the ones returned by all conjuncted Atoms
            while (result.Count > 0 && ++i < conjuncts.Length)
            {
                var conjunct = conjuncts[i];

                foreach (var resultMatch in result)
                {
                    if (conjunct.TryGetValue(resultMatch, out var conjunctMatch))
                        resultTemp.Add(resultMatch.Merge(conjunctMatch));
                }

                (resultTemp, result) = (result, resultTemp);
                resultTemp.Clear();
            }

            return result;
        }
    }
}