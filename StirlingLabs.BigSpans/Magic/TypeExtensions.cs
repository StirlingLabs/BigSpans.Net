using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static InlineIL.IL;
using static InlineIL.IL.Emit;

namespace StirlingLabs.Utilities.Magic
{
    [PublicAPI]
    internal static class TypeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TypeIs<T1, T2>(this T1? a, T2? b)
            where T1 : class where T2 : class
            => a is not null
                ? b is not null
                && NotNullTypeIs(a, b)
                : b is null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool NotNullTypeIs<T1, T2>(T1 a, T2 b)
            where T1 : class where T2 : class
            => a.GetMethodTable() == b.GetMethodTable();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe nint GetMethodTable<T>(this T o)
            where T : class
            => **(nint**)Unsafe.AsPointer(ref o);
    }

    [PublicAPI]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class Type<T1>
    {
        public static TypeInfo Info
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Ldtoken<T1>();
                return Return<TypeInfo>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe nint GetMethodTable()
        {
            Ldtoken<T1>();
            Pop(out nint* info);
            return info[3];
        }

        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        public static readonly nint MethodTable = GetMethodTable();

        private static class _Is<T2>
        {
            public static bool True
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => typeof(T1) == typeof(T2);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<T2>() => _Is<T2>.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNot<T2>() => !_Is<T2>.True;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Matches<T2>(in T2 o)
            where T2 : class
            => MethodTable == o.GetMethodTable();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DoesNotMatch<T2>(in T2 o)
            where T2 : class
            => !Matches(o);

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
