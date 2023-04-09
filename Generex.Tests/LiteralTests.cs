using NUnit.Framework;

namespace Generex.Tests
{
    public class LiteralTests
    {
        private readonly Generex<int> fiveMatcher = 5;

        [TestCase(new int[] { 5000, 10, 50, -5 }, ExpectedResult = 0)]
        [TestCase(new int[] { 5, 10, 5, -1000, 50 }, ExpectedResult = 2)]
        [TestCase(new int[] { 0, 1, 3, 5, -10000, -2241, 5, 5 }, ExpectedResult = 3)]
        public int MatchesMultiple(int[] sequence)
        {
            var result = fiveMatcher.MatchAll(sequence).ToArray();

            Assert.That(result.SelectMany(r => r.MatchedSequence), Has.All.EqualTo(5));

            return result.Length;
        }

        [TestCase(new int[] { 5 }, ExpectedResult = 0)]
        [TestCase(new int[] { 5, 10, 50, -5 }, ExpectedResult = 0)]
        [TestCase(new int[] { -5, 10, 5, -1000, 50 }, ExpectedResult = 2)]
        [TestCase(new int[] { 0, 1, 3, 5 }, ExpectedResult = 3)]
        public int MatchesSingle(int[] sequence)
        {
            var result = fiveMatcher.MatchAll(sequence).FirstOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result.StartIndex, Is.EqualTo(result.EndIndex));
            Assert.That(result[0], Is.EqualTo(5));

            return result.StartIndex;
        }
    }
}