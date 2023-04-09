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
        [TestCase("0x01", ExpectedResult = 1)]
        [TestCase("0xA2", ExpectedResult = 162)]
        [TestCase("0x3b", ExpectedResult = 59)]
        [TestCase("0xC4", ExpectedResult = 196)]
        [TestCase("0x5d", ExpectedResult = 93)]
        [TestCase("0xe6", ExpectedResult = 230)]
        [TestCase("0x7f", ExpectedResult = 127)]
        [TestCase("0x89", ExpectedResult = 137)]
        [TestCase("0xFF", ExpectedResult = 255)]
        public int HexNumber(string input)
        {
            var hexNumberMatcher = new Sequence<char>(new NonCapturingGroup<char>(new[] { '0', 'x' }), new Quantifier<char>(new Range<char>(new[] { new LiteralRange<char>('a', 'f'), new LiteralRange<char>('A', 'F'), new LiteralRange<char>('0', '9') }), 1, int.MaxValue));

            var result = hexNumberMatcher.MatchAll(input).LastOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(int.TryParse(new string(result.ToArray()), NumberStyles.AllowHexSpecifier, null, out var value), Is.True);

            return value;
        }

        [TestCase("0x01", ExpectedResult = 1)]
        [TestCase("0xA2", ExpectedResult = 162)]
        [TestCase("0x3b", ExpectedResult = 59)]
        [TestCase("0xC4", ExpectedResult = 196)]
        [TestCase("0x5d", ExpectedResult = 93)]
        [TestCase("0xe6", ExpectedResult = 230)]
        [TestCase("0x7f", ExpectedResult = 127)]
        [TestCase("0x89", ExpectedResult = 137)]
        [TestCase("0xFF", ExpectedResult = 255)]
        public int HexNumberFluentBuilder(string input)
        {
            var hexNumberMatcher = Generex.Sequence.Of(
                    Generex.Literal.Of('0', 'x').As.NonCapturingGroup,
                    Generex.Range.From('0').To('F').And.From('a').To('f').And.From('A').To('F').Repeat.AtLeastOnce
                ).Finish();

            var result = hexNumberMatcher.MatchAll(input).LastOrDefault();

            Assert.That(result, Is.Not.Null);
            Assert.That(int.TryParse(new string(result.ToArray()), NumberStyles.AllowHexSpecifier, null, out var value), Is.True);

            return value;
        }
    }
}