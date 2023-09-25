using Generex.Atoms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generex.Tests
{
    public class RegexTests
    {
        [TestCase("0x0001", ExpectedResult = 1)]
        [TestCase("0x03b", ExpectedResult = 59)]
        [TestCase("0x05d", ExpectedResult = 93)]
        [TestCase("0x7f", ExpectedResult = 127)]
        [TestCase("0x89", ExpectedResult = 137)]
        [TestCase("0xA2", ExpectedResult = 162)]
        [TestCase("0xC4", ExpectedResult = 196)]
        [TestCase("0xe6", ExpectedResult = 230)]
        [TestCase("0xFF", ExpectedResult = 255)]
        public int HexNumber(string input)
        {
            var hexNumberMatcher = new Sequence<char>(new NonCapturingGroup<char>(new[] { '0', 'x' }), new GreedyQuantifier<char>(new Range<char>(ranges: new[] { new LiteralRange<char>('a', 'f'), new LiteralRange<char>('A', 'F'), new LiteralRange<char>('0', '9') }), 1, int.MaxValue));

            Assert.That(hexNumberMatcher.HasMatch(input, out var result), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(int.TryParse(new string(result.ToArray()), NumberStyles.AllowHexSpecifier, null, out var value), Is.True);

            return value;
        }

        [TestCase("0x0001", ExpectedResult = 1)]
        [TestCase("0x03b", ExpectedResult = 59)]
        [TestCase("0x05d", ExpectedResult = 93)]
        [TestCase("0x7f", ExpectedResult = 127)]
        [TestCase("0x89", ExpectedResult = 137)]
        [TestCase("0xA2", ExpectedResult = 162)]
        [TestCase("0xC4", ExpectedResult = 196)]
        [TestCase("0xe6", ExpectedResult = 230)]
        [TestCase("0xFF", ExpectedResult = 255)]
        public int HexNumberFluentBuilder(string input)
        {
            var hexNumberMatcher = Generex.Literal.Of('0', 'x').As.NonCapturingGroup
                .FollowedBy.Range.From('0').To('9').And.From('a').To('f').And.From('A').To('F').GreedilyRepeat.AtLeastOnce
                .Finish();

            Assert.That(hexNumberMatcher.HasMatch(input, out var result), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(int.TryParse(new string(result.ToArray()), NumberStyles.AllowHexSpecifier, null, out var value), Is.True);

            return value;
        }

        [TestCase("1", ExpectedResult = new[] { 1 })]
        [TestCase("111", ExpectedResult = new[] { 111, 11, 1 })]
        [TestCase("aah99asd", ExpectedResult = new[] { 99, 9 })]
        [TestCase("152455", ExpectedResult = new[] { 1, 5, 2, 4, 55, 5 })]
        public int[] RepdigitFluentBuilder(string input)
        {
            var repdigitMatcher = Generex.Range.From('0').To('9').As.CapturingGroup(out var digit).Called("digit")
                .FollowedBy.CapturedGroup.ReferringBackTo(digit).GreedilyRepeat.AnyNumber
                .Finish();

            var results = repdigitMatcher.MatchAll(input, returnEveryMatch: false);

            var ints = results.Select(result => int.Parse(result.ToArray())).ToArray();
            return ints;
        }
    }
}