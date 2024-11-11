using EnumerableToolkit;
using Generex.Atoms;
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
    /// <remarks><para>
    /// To optimize the performance of finding matches in a sequence, the most important criterium is to minimize backtracking.
    /// Backtracking happens whenever the matching process has to move back to a previous state, change it, and then try matching forward again.
    /// The most common reason for this to happen are quantifiers, which can match less or more than they're intended to,
    /// forcing the matching process to return to them before eventually matching the right amount.<br/>
    /// The most efficient patterns are thus <i>deterministic</i> ones. To be deterministic, there can't be any uncertainty
    /// as to which sub-pattern a value in the input sequence has to be matched to at any point.
    /// </para><para>
    /// To match a sequence consisting only of the characters <c>a</c> and <c>b</c>, in which no <c>b</c> follows a <c>b</c>,
    /// one could use the pattern: <c>b?(a|ab)*</c>. This ensures a <c>b</c> can only follow an <c>a</c> after the first letter, so there can't be any <c>bb</c>s.<br/>
    /// However for each <c>a</c> in the sequence, it's unclear whether it should be matched to the first or second option.
    /// Specifically, it requires backtracking whenever a <c>b</c> appears (since the first option will be tried first).
    /// </para><para>
    /// While this might not seem like a big problem in this small example, it can become much worse with
    /// more complex sub-patterns and even little slowdowns can add up over enough time.<br/>
    /// In this case, the problem could be avoided as well, by instead specifying the pattern as: <c>b?(a+b)*</c>.<br/>
    /// This way, each <c>b</c> after the first character must be preceeded by <c>a</c>s - no <c>b</c>s.
    /// </para><para>
    /// Non-determinism can't always be avoided, as only a subset of regular expressions is expressible deterministically.<br/>
    /// However, it still makes sense to consider where patterns can be made as deterministic as possible to improve performance.
    /// </para><para>
    /// Matches get evaluated lazily, so as long as the matches themselves are always finite, infinite input sequences pose no problem.
    /// However this also applies to sub-matches, which means that unlimited <see cref="GreedyQuantifier{T}"/>s should only be used,
    /// when it's known that there will never be an infinite sequence of matches for it.<br/>
    /// This is especially important for the <see cref="Wildcard{T}"/> pattern, which should only be used with <see cref="LazyQuantifier{T}"/>s
    /// or a hard cap for the maximum number of matched input values.
    /// </para></remarks>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public abstract partial class Generex<T>
    {
        /// <summary>
        /// Whether <typeparamref name="T"/> is <see cref="char"/>.
        /// </summary>
        protected static readonly bool isChar = typeof(T) == typeof(char);

        /// <summary>
        /// Contains all meta characters that need to be escaped
        /// when applying <see cref="ToString()"/> to a pattern.
        /// </summary>
        protected static HashSet<char> metaCharacters =
        [
            '(', ')', '[', ']', '{', '}', '|', '&',
            '.', '\\', '+', '*', '?', '^', '$'
        ];

        /// <summary>
        /// The string used to separate sequences of literals.
        /// </summary>
        /// <remarks>
        /// Space for <see cref="char">characters</see>;
        /// otherwise, the <c>⋅</c> character used in computer science.
        /// </remarks>
        protected static string SequenceSeparator { get; } = isChar ? "" : "⋅";

        static Generex()
        {
            if (!isChar)
                metaCharacters.Add('⋅');
        }

        /// <summary>
        /// Escapes any occurences of <see cref="metaCharacters">meta characters</see>
        /// in the string representation of a <paramref name="literal"/>.
        /// </summary>
        /// <remarks>
        /// Used by the <see cref="ToString()"/> implementations for some patterns.
        /// </remarks>
        /// <param name="literal">The string representation of a literal that should be escaped.</param>
        /// <returns>The escaped representation of the literal.</returns>
        [return: NotNullIfNotNull(nameof(literal))]
        public static string? EscapeLiteral(string? literal)
        {
            if (literal is null)
                return null;

            var chars = new List<char>(literal);
            var escapeIndices = new List<int>();

            for (var i = 0; i < chars.Count; ++i)
            {
                if (metaCharacters.Contains(chars[i]))
                    escapeIndices.Add(i);
            }

            for (var i = 0; i < escapeIndices.Count; ++i)
            {
                // Index of char to escape, offset by number of escaped ones before
                chars.Insert(escapeIndices[i] + i, '\\');
            }

            return new string([.. chars]);
        }

        /// <summary>
        /// Converts the given literal <paramref name="value"/> into a <see cref="Literal{T}"/> atom.
        /// </summary>
        /// <param name="value">The literal to convert into an atom.</param>
        public static implicit operator Generex<T>(T value) => new Literal<T>(value);

        /// <summary>
        /// Groups the given sequence of atoms into a <see cref="Atoms.Sequence{T}"/>.
        /// </summary>
        /// <param name="atoms">The atoms to group into a sequence.</param>
        public static implicit operator Generex<T>(Generex<T>[] atoms) => new Atoms.Sequence<T>((IEnumerable<Generex<T>>)atoms);

        /// <summary>
        /// Converts the given sequence of literals into a <see cref="Atoms.Sequence{T}"/> of <see cref="Literal{T}"/> atoms.
        /// </summary>
        /// <param name="values">The sequence of literals to convert into an atom.</param>
        public static implicit operator Generex<T>(T[] values) => values.Select(v => new Literal<T>(v)).ToArray();

        /// <summary>
        /// Joins the given atoms into a <see cref="Conjunction{T}"/>.
        /// </summary>
        /// <param name="leftAtom">The left atom.</param>
        /// <param name="rightAtom">The right atom.</param>
        /// <returns>The joined atoms.</returns>
        public static Generex<T> operator &(Generex<T> leftAtom, Generex<T> rightAtom)
        {
            if (leftAtom is Conjunction<T> leftConjunction)
            {
                if (rightAtom is Conjunction<T> rightConjunction)
                    return new Conjunction<T>(leftConjunction.Atoms.Concat(rightConjunction.Atoms));

                return new Conjunction<T>(leftConjunction.Atoms.Concat(rightAtom));
            }
            else
            {
                if (rightAtom is Conjunction<T> rightConjunction)
                    return new Conjunction<T>(leftAtom.Yield().Concat(rightConjunction.Atoms));

                return new Conjunction<T>(leftAtom, rightAtom);
            }
        }

        /// <summary>
        /// Joins the given atoms into a <see cref="Atoms.Sequence{T}"/>.
        /// </summary>
        /// <remarks>
        /// This references the <c>⋅</c> character used in computer science.
        /// </remarks>
        /// <param name="leftAtom">The left atom.</param>
        /// <param name="rightAtom">The right atom.</param>
        /// <returns>The joined atoms.</returns>
        public static Generex<T> operator *(Generex<T> leftAtom, Generex<T> rightAtom)
        {
            if (leftAtom is Atoms.Sequence<T> leftSequence)
            {
                if (rightAtom is Atoms.Sequence<T> rightSequence)
                    return new Atoms.Sequence<T>(leftSequence.Atoms.Concat(rightSequence.Atoms));

                return new Atoms.Sequence<T>(leftSequence.Atoms.Concat(rightAtom));
            }
            else
            {
                if (rightAtom is Atoms.Sequence<T> rightSequence)
                    return new Atoms.Sequence<T>(leftAtom.Yield().Concat(rightSequence.Atoms));

                return new Atoms.Sequence<T>(leftAtom, rightAtom);
            }
        }

        /// <summary>
        /// Joins the given atoms into a <see cref="Disjunction{T}"/>.
        /// </summary>
        /// <param name="leftAtom">The left atom.</param>
        /// <param name="rightAtom">The right atom.</param>
        /// <returns>The joined atoms.</returns>
        public static Generex<T> operator |(Generex<T> leftAtom, Generex<T> rightAtom)
        {
            if (leftAtom is Disjunction<T> leftDisjunction)
            {
                if (rightAtom is Disjunction<T> rightDisjunction)
                    return new Disjunction<T>(leftDisjunction.Atoms.Concat(rightDisjunction.Atoms));

                return new Disjunction<T>(leftDisjunction.Atoms.Concat(rightAtom));
            }
            else
            {
                if (rightAtom is Disjunction<T> rightDisjunction)
                    return new Disjunction<T>(leftAtom.Yield().Concat(rightDisjunction.Atoms));

                return new Disjunction<T>(leftAtom, rightAtom);
            }
        }

        /// <summary>
        /// Determines if this pattern has a match in the given <paramref name="inputSequence"/>.
        /// </summary>
        /// <param name="inputSequence">The sequence to match against this pattern.</param>
        /// <param name="match">The first match found; otherwise, <c>null</c>.</param>
        /// <param name="fromStartOnly"><c>true</c> if only matches starting at the first element in the sequence should be considered.</param>
        /// <returns></returns>
        public bool HasMatch(IEnumerable<T> inputSequence, [NotNullWhen(true)] out Match<T>? match, bool fromStartOnly = false)
        {
            match = MatchAll(inputSequence, false, true, fromStartOnly).FirstOrDefault();
            return match != null;
        }

        /// <inheritdoc cref="HasMatch(IEnumerable{T}, out Match{T}?, bool)"/>
        public bool HasMatch(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => HasMatch(inputSequence, out _, fromStartOnly);

        public IEnumerable<Match<T>> Match(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => MatchAll(inputSequence, false, true, fromStartOnly);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputSequence">The sequence to match against this pattern.</param>
        /// <param name="restartingBehavior">Defines which elements in the sequence matching will restart from.</param>
        /// <param name="returnEveryMatch"><c>true</c> if every match from every considered starting element in the sequence should be returned.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<Match<T>> MatchAll(IEnumerable<T> inputSequence, MatchRestartingBehavior restartingBehavior = MatchRestartingBehavior.FromEveryElement, bool returnEveryMatch = true)
        {
            var fromStartOnly = restartingBehavior == MatchRestartingBehavior.FromStartOnly;

            var startMatch = new MatchState<T>(inputSequence);
            bool wasEnd;

            do
            {
                wasEnd = startMatch.IsInputEnd;
                var hadSuccessfulMatch = false;

                var matches = ContinueMatchInternal(startMatch);

                if (returnEveryMatch)
                {
                    foreach (var match in matches)
                        yield return match.GetMatch();

                    startMatch = startMatch.Next().AsStart();
                }
                else
                {
                    var match = matches.FirstOrDefault();

                    if (match != null)
                    {
                        hadSuccessfulMatch = true;
                        yield return match.GetMatch();
                    }

                    startMatch = restartFromEveryValue || !hadSuccessfulMatch ?
                        startMatch.Next().AsStart() : match!.AsStart();
                }
            }
            while (!fromStartOnly && !wasEnd);
        }

        public IEnumerable<Match<T>> MatchAllFromStart(IEnumerable<T> inputSequence, bool returnEveryMatch = true, bool restartFromEveryValue = true)
            => MatchAll(inputSequence, returnEveryMatch, restartFromEveryValue, true);

        public IEnumerable<Match<T>> MatchSequential(IEnumerable<T> inputSequence, bool fromStartOnly = false)
            => MatchAll(inputSequence, false, false, fromStartOnly);

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