using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a reference to a capture group.
    /// </summary>
    /// <inheritdoc/>
    public sealed class CaptureReference<T> : CaptureReferenceBase<T, MatchedSequence<T>, CaptureReference<T>, CaptureGroup<T>>
    {
        /// <inheritdoc/>
        public CaptureReference(string? name = null) : base(name)
        { }

        /// <inheritdoc/>
        public override CaptureGroup<T> GetCaptureGroup(IEnumerable<MatchedSequence<T>> captures)
            => new(this, captures);

        /// <inheritdoc/>
        public override bool TryGetCapture(IEnumerable<MatchState<T>> capturedMatch, [NotNullWhen(true)] out MatchedSequence<T>? capture)
        {
            capture = new(capturedMatch);
            return true;
        }
    }

    /// <summary>
    /// Represents the base class for references to capture groups.
    /// </summary>
    /// <typeparam name="TIn">The type of elements in the input sequence.</typeparam>
    /// <typeparam name="TCapture">The type of the captures referred to by this reference.</typeparam>
    /// <typeparam name="TCaptureReference">The type of this reference used to refer to the captures.</typeparam>
    /// <typeparam name="TCaptureGroup">The type of the capture groups created by this reference.</typeparam>
    public abstract class CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
        : ICaptureReference<TIn>, IEquatable<CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>?>
        where TCapture : MatchedSequence<TIn>
        where TCaptureReference : CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
        where TCaptureGroup : CaptureGroupBase<TIn, TCapture, TCaptureReference, TCaptureGroup>
    {
        /// <inheritdoc/>
        public string Name { get; internal set; }

        /// <summary>
        /// Creates a new capture reference with the given <paramref name="name"/> or a <see cref="Guid"/> by default.
        /// </summary>
        /// <param name="name">The name of the capture group. Defaults to a <see cref="Guid"/>.</param>
        public CaptureReferenceBase(string? name = null)
        {
            Name = name ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Determines whether two capture references are inequal.
        /// </summary>
        /// <param name="left">The first capture reference.</param>
        /// <param name="right">The second capture reference.</param>
        /// <returns><c>true</c> if they're different, otherwise <c>false</c>.</returns>
        public static bool operator !=(CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>? left, CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>? right)
            => !(left == right);

        /// <summary>
        /// Determines whether two capture references are equal.
        /// </summary>
        /// <param name="left">The first capture reference.</param>
        /// <param name="right">The second capture reference.</param>
        /// <returns><c>true</c> if they're equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>? left, CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>? right)
            => ReferenceEquals(left, right) || left?.Name == right?.Name;

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup> captureReference && Equals(captureReference);

        /// <inheritdoc/>
        bool IEquatable<CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>?>.Equals(CaptureReferenceBase<TIn, TCapture, TCaptureReference, TCaptureGroup>? other)
            => Equals(other);

        /// <inheritdoc/>
        public bool Equals(ICaptureReference<TIn>? other) => other?.Name == Name;

        /// <inheritdoc/>
        public abstract TCaptureGroup GetCaptureGroup(IEnumerable<TCapture> captures);

        ICaptureGroup<TIn> ICaptureReference<TIn>.GetCaptureGroup(IEnumerable<MatchedSequence<TIn>> captures)
            => GetCaptureGroup(captures.Cast<TCapture>());

        /// <inheritdoc/>
        public override int GetHashCode() => Name.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Name;

        bool ICaptureReference<TIn>.TryGetCapture(IEnumerable<MatchState<TIn>> capturedMatch, [NotNullWhen(true)] out MatchedSequence<TIn>? capture)
        {
            if (!TryGetCapture(capturedMatch, out var actualCapture))
            {
                capture = null;
                return false;
            }

            capture = actualCapture;
            return true;
        }

        /// <inheritdoc cref="ICaptureReference{T}.TryGetCapture"/>
        public abstract bool TryGetCapture(IEnumerable<MatchState<TIn>> capturedMatch, [NotNullWhen(true)] out TCapture? capture);
    }

    /// <summary>
    /// Defines the interface for references to capture groups.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public interface ICaptureReference<T> : IEquatable<ICaptureReference<T>?>
    {
        /// <summary>
        /// Gets the name of the capture group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a <see cref="ICaptureGroup{T}">capture group</see> using
        /// the sequence of <paramref name="captures"/> of the <see cref="Match{T}"/>.
        /// </summary>
        /// <param name="captures">The captured sequences to group.</param>
        /// <returns>The created <see cref="ICaptureGroup{T}"/>.</returns>
        public ICaptureGroup<T> GetCaptureGroup(IEnumerable<MatchedSequence<T>> captures);

        /// <summary>
        /// Tries to create a <paramref name="capture"/> from the captured sequence of matched states.
        /// </summary>
        /// <param name="capturedMatch">The captured sequence of matched states to create a <paramref name="capture"/> with.</param>
        /// <param name="capture">The created capture if successful; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the <paramref name="capture"/> is created successfully; otherwise, <c>false</c>.</returns>
        public bool TryGetCapture(IEnumerable<MatchState<T>> capturedMatch, [NotNullWhen(true)] out MatchedSequence<T>? capture);
    }
}