using Generex.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generex.Tests
{
    public class QuantifierTests
    {
        private readonly Generex<int> fiveFivesMatcher = new GreedyQuantifier<int>(5, 5);

        [TestCase(new int[] { 5, 5, 5, -1000, 5, 5 }, ExpectedResult = 0)]
        [TestCase(new int[] { 5000, 5, 5, 5, 5, 5, 10, -5, 5, 6, 5, 5, 5, 5, 5, 5 }, ExpectedResult = 3)]
        public int MatchesCount(int[] sequence)
        {
            var result = fiveFivesMatcher.MatchAll(sequence).ToArray();

            Assert.That(result.SelectMany(r => r.MatchedSequence), Has.All.EqualTo(5));

            return result.Length;
        }

        [TestCase(new int[] { 5000, 5, 5, 5, 5, 5, 10, -5, 5, 6, 5, 5, 5, 5, 5, 5 }, ExpectedResult = 1)]
        public int MatchesStartIndex(int[] sequence)
        {
            var result = fiveFivesMatcher.MatchAll(sequence).FirstOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(5));
            Assert.That(result.StartIndex + 4, Is.EqualTo(result.EndIndex));
            Assert.That(result, Has.All.EqualTo(5));

            return result.StartIndex;
        }
    }
}