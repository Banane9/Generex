using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    /// <summary>
    /// Represents a reference to a capture group.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
    public class CaptureReference<T>
    {
        /// <summary>
        /// Gets the name of the capture group.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Creates a new capture reference with the given name or a <see cref="Guid"/> by default.
        /// </summary>
        /// <param name="name">The name of the capture group. Defaults to a <see cref="Guid"/>.</param>
        public CaptureReference(string? name = null)
        {
            Name = name ?? Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Determines whether two capture references are inequal.
        /// </summary>
        /// <param name="left">The first capture reference.</param>
        /// <param name="right">The second capture reference.</param>
        /// <returns><c>true</c> if they're different, otherwise <c>false</c>.</returns>
        public static bool operator !=(CaptureReference<T>? left, CaptureReference<T>? right)
            => !(left == right);

        /// <summary>
        /// Determines whether two capture references are equal.
        /// </summary>
        /// <param name="left">The first capture reference.</param>
        /// <param name="right">The second capture reference.</param>
        /// <returns><c>true</c> if they're equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(CaptureReference<T>? left, CaptureReference<T>? right)
            => ReferenceEquals(left, right) || left?.Name == right?.Name;

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is CaptureReference<T> captureReference && captureReference == this;

        /// <inheritdoc/>
        public override int GetHashCode() => Name.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}