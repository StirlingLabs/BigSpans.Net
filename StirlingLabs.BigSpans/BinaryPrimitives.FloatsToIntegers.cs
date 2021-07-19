using System.Runtime.CompilerServices;

// @formatter:off
#if !NETSTANDARD
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif
// @formatter:on

namespace StirlingLabs.Utilities
{
    public static partial class BinaryPrimitives
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once RedundantUnsafeContext
        public static unsafe int SingleToInt32Bits(float value)
        {
#if !NETSTANDARD
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (Sse2.IsSupported)
                return Sse2.ConvertToInt32(Vector128.CreateScalarUnsafe(value).AsInt32());
#endif
            return Unsafe.As<float, int>(ref value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once RedundantUnsafeContext
        public static unsafe long DoubleToInt64Bits(double value)
        {
#if !NETSTANDARD
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (Sse2.X64.IsSupported)
                return Sse2.X64.ConvertToInt64(Vector128.CreateScalarUnsafe(value).AsInt64());
#endif
            return Unsafe.As<double, long>(ref value);
        }

        /// <summary>
        /// Converts the specified 32-bit signed integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float Int32BitsToSingle(int value)
        {
#if !NETSTANDARD
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (Sse2.IsSupported)
                return Vector128.CreateScalarUnsafe(value).AsSingle().ToScalar();
#endif
            return Unsafe.As<int, float>(ref value);
        }

        /// <summary>
        /// Converts the specified 64-bit signed integer to a double-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A double-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double Int64BitsToDouble(long value)
        {
#if !NETSTANDARD
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (Sse2.X64.IsSupported)
                return Vector128.CreateScalarUnsafe(value).AsDouble().ToScalar();
#endif
            return Unsafe.As<long, double>(ref value);
        }
    }
}
