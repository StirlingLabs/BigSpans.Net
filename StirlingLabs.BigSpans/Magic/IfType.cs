using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities.Magic
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IfType<T1>
    {
        private static class _Is<T2>
        {
            public static readonly bool True = typeof(T1) == typeof(T2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<T2>() => _Is<T2>.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNot<T2>() => !_Is<T2>.True;

        private static class _IsPrimitive
        {
            public static readonly bool True = typeof(T1).IsPrimitive;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrimitive() => _IsPrimitive.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotPrimitive() => !_IsPrimitive.True;

        private static class _IsAssignableTo<T2>
        {
            public static readonly bool True = typeof(T2).IsAssignableFrom(typeof(T1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssignableTo<T2>() => _IsAssignableTo<T2>.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotAssignableTo<T2>() => !_IsAssignableTo<T2>.True;

        private static class _IsAssignableFrom<T2>
        {
            public static readonly bool True = typeof(T1).IsAssignableFrom(typeof(T2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAssignableFrom<T2>() => _IsAssignableFrom<T2>.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotAssignableFrom<T2>() => !_IsAssignableFrom<T2>.True;
    }
}
