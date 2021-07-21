using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StirlingLabs.BigSpans.Tests
{
    public readonly struct DecompileMethod : IEquatable<DecompileMethod>
    {
        public readonly nuint Pointer;
        public readonly string Description;

        public DecompileMethod(nuint pointer, string description)
        {
            Pointer = pointer;
            Description = description;
        }

        public unsafe DecompileMethod(void* p, string description)
        {
            Pointer = (nuint)p;
            Description = description;
        }

        [SuppressMessage("Usage", "CA2225", Justification = "Limited use case")]
        public static implicit operator DecompileMethod(in (nuint, string) tuple)
            => Unsafe.As<(nuint, string), DecompileMethod>(ref Unsafe.AsRef(tuple));

        [SuppressMessage("Microsoft.Design", "CA1062", Justification = "Should never be null")]
        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Intentional disregard of exception type")]
        [SuppressMessage("Usage", "CA2225", Justification = "Limited use case")]
        public static implicit operator DecompileMethod(MethodInfo mi)
        {
            Debug.Assert(mi != null, nameof(mi) + " != null");
            var name = $"{mi.ReflectedType}.{mi.Name}";
            try
            {
                if (mi.IsGenericMethodDefinition)
                    mi = mi.MakeGenericMethod(mi.GetGenericArguments().Select(g => typeof(byte)).ToArray());
                RuntimeHelpers.PrepareMethod(mi.MethodHandle);
                return new((nuint)(nint)mi.MethodHandle.GetFunctionPointer(), name);
            }
            catch
            {
                return new(0, name);
            }
        }

        public override string ToString()
            => Description;
        public bool Equals(DecompileMethod other)
            => Pointer.Equals(other.Pointer);

        public override bool Equals(object? obj)
            => obj is DecompileMethod other && Equals(other);

        public override int GetHashCode()
            => Pointer.GetHashCode();

        public static bool operator ==(DecompileMethod left, DecompileMethod right)
            => left.Equals(right);

        public static bool operator !=(DecompileMethod left, DecompileMethod right)
            => !left.Equals(right);
    }
}
