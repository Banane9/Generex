using System;
using System.Collections.Generic;
using System.Text;

namespace Generex
{
    public class CaptureReference<T>
    {
        public string? Name { get; }

        public CaptureReference(string? name = null)
        {
            Name = name;
        }

        public static bool operator !=(CaptureReference<T>? left, CaptureReference<T>? right)
            => !(left == right);

        public static bool operator ==(CaptureReference<T>? left, CaptureReference<T>? right)
            => ReferenceEquals(left, right) || left?.Name == right?.Name;

        public override bool Equals(object obj)
            => obj is CaptureReference<T> captureReference && captureReference == this;

        public override int GetHashCode()
            => Name?.GetHashCode() ?? base.GetHashCode();

        public override string ToString()
            => Name ?? "Unnamed Capture Reference";
    }
}