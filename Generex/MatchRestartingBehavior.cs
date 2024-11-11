namespace Generex
{
    /// <summary>
    /// Defines the possible restarting behavior for <see cref="Generex{T}"/>-matching.
    /// </summary>
    public enum MatchRestartingBehavior
    {
        /// <summary>
        /// Only attempts matching at the first element in the sequence.
        /// </summary>
        FromStartOnly,

        /// <summary>
        /// Attempt matching at each element in the sequence.
        /// </summary>
        /// <remarks>
        /// This is the fallback for every behavior other than
        /// <see cref="FromStartOnly">FromStartOnly</see>
        /// if no match was found from the current start.
        /// </remarks>
        FromEveryElement,

        /// <summary>
        /// Attemps matching after the shortest possible match from the last start.<br/>
        /// Starting point evaluation ends when reaching the minimum of
        /// <i>the next element in the sequence</i>.
        /// </summary>
        /// <remarks>
        /// This option may take a long time with certain patterns,
        /// particularly on potentially infinite sequences,
        /// where it may even get continue matching forever.<br/>
        /// Consider writing your pattern such that
        /// <see cref="AfterFirstMatch">AfterFirstMatch</see> delivers the desired result.
        /// </remarks>
        AfterShortestMatch,

        /// <summary>
        /// Attempts matching after the first found match from the last start.
        /// </summary>
        AfterFirstMatch,

        /// <summary>
        /// Attempts matching after the longest possible match from the last start.
        /// </summary>
        /// <inheritdoc cref="AfterShortestMatch"/>
        AfterLongestMatch
    }
}