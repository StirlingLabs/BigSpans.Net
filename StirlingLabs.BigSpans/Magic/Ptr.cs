using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities.Magic
{
    // ReSharper disable UseNameofExpression, NotResolvedInText
    [DebuggerDisplay("{DebugString,nq}", Type = "{Type}")]
    // ReSharper restore UseNameofExpression, NotResolvedInText
    [SuppressMessage("Usage", "CA2225", Justification = "Limited use case")]
    public readonly struct Ptr<T> : IEquatable<Ptr<T>>, IEquatable<nuint>, IEquatable<nint>
    {
        // @formatter:off
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public unsafe string DebugString
            => Unsafe.IsNullRef(ref Value)
            ? "<null reference>"
            : Type<T>.IsPrimitive()
            ? Value!.ToString() ?? ""
            : sizeof(nuint) == 8
                ? $"@ 0x{(ulong)(nuint)Pointer:X16}"
                : $"@ 0x{(uint)(nuint)Pointer:X8}";
        public unsafe Ptr(void* pointer) => Pointer = pointer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Type Type => typeof(T);
        public readonly unsafe void* Pointer;
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public unsafe ref T Value => ref Unsafe.AsRef<T>(Pointer);
        public override bool Equals(object? obj)
            => obj is Ptr<T> other && Equals(other)
                || obj is nint ni && Equals(ni)
                || obj is nuint nu && Equals(nu);
        public override unsafe int GetHashCode() => ((nuint)Pointer).GetHashCode();
        public unsafe bool Equals(Ptr<T> other) => Pointer == other.Pointer;
        public unsafe bool Equals(nint other) => (nint)Pointer == other;
        public unsafe bool Equals(nuint other) => (nuint)Pointer == other;
        public static unsafe implicit operator void*(Ptr<T> p) => p.Pointer;
        public static unsafe implicit operator nuint(Ptr<T> p) => (nuint)p.Pointer;
        public static unsafe implicit operator nint(Ptr<T> p) => (nint)p.Pointer;
        public static bool operator ==(Ptr<T> left, Ptr<T> right) => left.Equals(right);
        public static bool operator !=(Ptr<T> left, Ptr<T> right) => !left.Equals(right);
        public static bool operator ==(Ptr<T> left, nuint right) => (nuint)left == right;
        public static bool operator !=(Ptr<T> left, nuint right) => (nuint)left != right;
        public static bool operator ==(nuint left, Ptr<T> right) => left == (nuint)right;
        public static bool operator !=(nuint left, Ptr<T> right) => left != (nuint)right;
        public static bool operator ==(Ptr<T> left, nint right) => (nint)left == right;
        public static bool operator !=(Ptr<T> left, nint right) => (nint)left != right;
        public static bool operator ==(nint left, Ptr<T> right) => left == (nint)right;
        public static bool operator !=(nint left, Ptr<T> right) => left != (nint)right;
        public static unsafe bool operator ==(Ptr<T> left, void* right) => (void*)left == right;
        public static unsafe bool operator !=(Ptr<T> left, void* right) => (void*)left != right;
        public static unsafe bool operator ==(void* left, Ptr<T> right) => left == (void*)right;
        public static unsafe bool operator !=(void* left, Ptr<T> right) => left != (void*)right;
        // @formatter:on
    }
}
