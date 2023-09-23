using System;
using System.Collections.Generic;
using System.Text;

namespace Generex.Fluent
{
    internal class Wildcard<T> : Atom<T>
    {
        public Wildcard(IParentAtom<T>? parent = null) : base(parent)
        { }

        protected override Generex<T> finishInternal() => new Atoms.Wildcard<T>();
    }
}