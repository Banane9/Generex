﻿using Generex.Atoms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a regex-like pattern, which can be used to find sequences of values in an input sequence, that fulfill specific conditions.
    /// </summary>
    /// <remarks>
    /// To optimize the performance of finding matches in a sequence, the most important criterium is to minimize backtracking.
    /// Backtracking happens whenever the matching process has to move back to a previous state, change it, and then try matching forward again.
    /// The most common reason for this to happen are quantifiers, which can match less or more than they're intended to,
    /// forcing the matching process to return to them before eventually matching the right amount.<br/>
    /// The most efficient patterns are thus <i>deterministic</i> ones. To be deterministic, there can't be any uncertainty
    /// as to which sub-pattern a value in the input sequence has to be matched to at any point.
    /// <para/>
    /// To match a sequence consisting only of the characters <c>a</c> and <c>b</c>, in which no <c>b</c> follows a <c>b</c>,
    /// one could use the pattern: <c>b?(a|ab)*</c>. This ensures a <c>b</c> can only follow an <c>a</c> after the first letter, so there can't be any <c>bb</c>s.<br/>
    /// However for each <c>a</c> in the sequence, it's unclear whether it should be matched to the first or second option.
    /// Specifically, it requires backtracking whenever a <c>b</c> appears (since the first option will be tried first).<br/>
    /// While this might not seem like a big problem in this small example, it can become much worse with
    /// more complex sub-patterns and even little slowdowns can add up over enough time.<br/>
    /// In this case, the problem could be avoided, by instead specifying the pattern as: <c>(a|ba?)*</c>.<br/>
    /// This way, each <c>b</c> must be matched to the second option, and each <c>a</c> either follows a <c>b</c>
    /// or must be matched to the first option. The <c>?</c> is only there to allow a <c>b</c> as the end of the sequence.<br/>
    /// Non-determinism can't always be avoided, as only a subset of regular expressions is expressible deterministically.
    /// However it still makes sense to consider where patterns can be made as deterministic as possible.
    /// <para/>
    /// Matches get evaluated lazily, so as long as the matches themselves are finite, infinite input sequences pose no problem.
    /// However this also applies to sub-matches, which means that unlimited <see cref="GreedyQuantifier{T}"/>s should only be used,
    /// when it's known that there will never be an infinite sequence of matches for it.<br/>
    /// This is especially important for the <see cref="Wildcard{T}"/> pattern, which should only be used with <see cref="LazyQuantifier{T}"/>s
    /// or a hard cap for the maximum number of matched input values.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public abstract partial class Generex<T>
    {
        protected static string EscapedSequenceSeparator { get; } = "\\" + SequenceSeparator;
        protected static string SequenceSeparator { get; } = typeof(T) == typeof(char) ? "" : "⋅";

        /// <summary>
        /// Replaces any occurences of the sequence separator character by an escaped version.<br/>
        /// Used by the <see cref="ToString"/> implementations for some pattern.
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
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
            bool wasEnd;

            do
            {
                wasEnd = startMatch.IsInputEnd;
                var hadSuccessfulMatch = false;

                var match = ContinueMatchInternal(startMatch).FirstOrDefault();
                if (match != null)
                {
                    hadSuccessfulMatch = true;
                    yield return match.GetMatch();
                }

                if (restartFromEveryValue || !hadSuccessfulMatch)
                    startMatch = startMatch.Next().AsStart();
                else
                    startMatch = match!.AsStart();
            }
            while (!fromStartOnly && !wasEnd);
        }

        public IEnumerable<Match<T>> MatchSequential(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => MatchAll(inputSequence, false, fromStartOnly);

        /// <summary>
        /// Returns a string that represents the current pattern.
        /// </summary>
        /// <returns>A string that represents the current pattern.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Returns a string that represents the current pattern, specifying whether it should be grouped.
        /// </summary>
        /// <param name="grouped">Whether the pattern should be wrapped as a group, when necessary.</param>
        /// <returns>A string that represents the current pattern.</returns>
        public virtual string ToString(bool grouped) => ToString();

        protected static IEnumerable<MatchState<T>> ContinueMatch(Generex<T> instance, MatchState<T> currentMatch)
            => instance.ContinueMatchInternal(currentMatch);

        protected abstract IEnumerable<MatchState<T>> ContinueMatchInternal(MatchState<T> currentMatch);
    }
}