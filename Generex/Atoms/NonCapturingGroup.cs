﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Atoms
{
    public class NonCapturingGroup<T> : Generex<T>
    {
        public Generex<T> Atom { get; }

        public NonCapturingGroup(Generex<T> atom) : base(atom.EqualityComparer)
        {
            Atom = atom;
        }

        public override string ToString()
        {
            return $"(?:{Atom})";
        }

        protected override IEnumerable<MatchElement> MatchNextInternal(MatchElement currentMatch, T value)
        {
            foreach (var nextMatch in MatchNext(Atom, currentMatch, value))
            {
                nextMatch.Capturing = false;
                yield return nextMatch;
            }
        }
    }
}