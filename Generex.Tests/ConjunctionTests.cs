using Generex.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generex.Tests
{
    public class ConjunctionTests
    {
        [TestCase(10, ExpectedResult = new int[] { 0, 10 })]
        [TestCase(20, ExpectedResult = new int[] { 0, 10, 20 })]
        public int[] CommonMultiplesOf2And5(int n)
        {
            var commonMultiplesMatcher = new Conjunction<int>(new LazyQuantifier<int>(new LazyQuantifier<int>(new Wildcard<int>(), 2), 0, int.MaxValue),
                    new LazyQuantifier<int>(new LazyQuantifier<int>(new Wildcard<int>(), 5), 0, int.MaxValue));

            var results = commonMultiplesMatcher.MatchAll(Enumerable.Repeat(0, n), fromStartOnly: true);

            return results.Select(result => result.Length).ToArray();
        }
    }
}