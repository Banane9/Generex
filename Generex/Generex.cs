using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Generex
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">The type of the elements in the input sequence.</typeparam>
    public abstract partial class Generex<T>
    {
        protected static string EscapedSequenceSeparator { get; } = "\\" + SequenceSeparator;
        protected static string SequenceSeparator { get; } = typeof(T) == typeof(char) ? "" : "⋅";

        [return: NotNullIfNotNull(nameof(literal))]
        public static string? EscapeLiteral(string? literal)
            => literal?.Replace(SequenceSeparator, EscapedSequenceSeparator);

        public static implicit operator Generex<T>(T value) => new Literal<T>(value);

        public static implicit operator Generex<T>(Generex<T>[] atoms) => new Sequence<T>((IEnumerable<Generex<T>>)atoms);

        public static implicit operator Generex<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        public static Generex<T> operator |(Generex<T> leftAtom, Generex<T> rightAtom) => new Alternative<T>(leftAtom, rightAtom);

        public static Generex<T> operator +(Generex<T> leftAtom, Generex<T> rightAtom) => new Sequence<T>(leftAtom, rightAtom);

        public Match<T>? Match(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => MatchAll(inputSequence, false, fromStartOnly).FirstOrDefault();

        public IEnumerable<Match<T>> MatchAll(IEnumerable<T> inputSequence, bool restartFromEveryValue = true, bool fromStartOnly = false)
        {
            var startMatch = new MatchState<T>(inputSequence);
            bool tryWithoutNext;

            do
            {
                tryWithoutNext = startMatch.IsInputEnd;
                var hadSuccessfulMatch = false;

                var match = ContinueMatchInternal(startMatch).FirstOrDefault();
                if (match != null)
                {
                    hadSuccessfulMatch = true;
                    yield return match.GetMatch();
                }

                if (restartFromEveryValue || !hadSuccessfulMatch)
                    startMatch = startMatch.Next();
                else
                    startMatch = match!;

                startMatch.IsMatchEnd = true;
            }
            while (!fromStartOnly && (startMatch.IsInputEnd || tryWithoutNext));
        }

        public IEnumerable<Match<T>> MatchSequential(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => MatchAll(inputSequence, false, fromStartOnly);

        public abstract override string ToString();

        public virtual string ToString(bool grouped) => ToString();

        protected static IEnumerable<MatchState<T>> ContinueMatch(Generex<T> instance, MatchState<T> currentMatch)
            => instance.ContinueMatchInternal(currentMatch);

        protected abstract IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch);
    }
}