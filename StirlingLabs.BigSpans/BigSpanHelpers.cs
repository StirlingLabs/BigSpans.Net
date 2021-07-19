using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using InlineIL;
using StirlingLabs.Utilities.Magic;

// @formatter:off
#if NETSTANDARD2_0
using System.Linq;
using System.Reflection;
#endif
// @formatter:on

namespace StirlingLabs.Utilities
{
    internal static partial class BigSpanHelpers
    {
        public static readonly unsafe bool Is64Bit = sizeof(nint) == 8;

        public static int GetSizeOfByReference<T>()
        {
            IL.Emit.Sizeof(typeof(ByReference<T>));
            return IL.Return<int>();
        }
        public static int GetSizeOfBigSpan<T>()
        {
            IL.Emit.Sizeof(typeof(BigSpan<T>));
            return IL.Return<int>();
        }
        public static int GetSizeOfReadOnlyBigSpan<T>()
        {
            IL.Emit.Sizeof(typeof(ReadOnlyBigSpan<T>));
            return IL.Return<int>();
        }
        
        /// <summary>
        /// Evaluate whether a given integral value is a power of 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(int value) => (value & (value - 1)) == 0 && value > 0;

        /// <summary>
        /// Evaluate whether a given integral value is a power of 2.
        /// </summary>
        /// <param name="value">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(uint value) => (value & (value - 1)) == 0 && value != 0;

        private static readonly ConcurrentDictionary<Type, bool> _IsReferenceOrContainsReferencesCache
            = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReferenceOrContainsReferences(Type t)
            => _IsReferenceOrContainsReferencesCache.GetOrAdd(t, IsReferenceOrContainsReferencesInternal);

#if NETSTANDARD2_0
        public static bool IsReferenceOrContainsReferences<T>()
            => IsReferenceOrContainsReferences(typeof(T));

        private static bool IsReferenceOrContainsReferencesInternal(Type t)
        {
            if (t.IsClass) return true;

            var fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return fields.Any(f => IsReferenceOrContainsReferences(f.FieldType));
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReferenceOrContainsReferences<T>()
            => RuntimeHelpers.IsReferenceOrContainsReferences<T>();


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool IsReferenceOrContainsReferencesInternal(Type t)
        {
            if (t.IsClass) return true;

            return ((Func<bool>)typeof(RuntimeHelpers).GetMethod(nameof(RuntimeHelpers.IsReferenceOrContainsReferences))!
                    .MakeGenericMethod(t)
                    .CreateDelegate(typeof(Func<bool>)))
                .Invoke();
        }
#endif
    }
}
