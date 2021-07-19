using System;
using System.Runtime.CompilerServices;
using StirlingLabs.Utilities.Magic;

namespace StirlingLabs.Utilities
{
    public partial class BinaryPrimitives
    {
        /// <summary>
        /// Reads a <see cref="double" /> from the beginning of a read-only span of bytes, as little endian.
        /// </summary>
        /// <param name="source">The read-only span to read.</param>
        /// <returns>The little endian value.</returns>
        /// <remarks>Reads exactly 8 bytes from the beginning of the span.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> is too small to contain a <see cref="double" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDoubleLittleEndian(ReadOnlyBigSpan<byte> source)
            => !BitConverter.IsLittleEndian
                ? BitConverter.Int64BitsToDouble(ReverseEndianness(source.Read<long>()))
                : Unsafe.As<byte, double>(ref source.GetReference());

        /// <summary>
        /// Reads an Int16 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<short>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads an Int32 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<int>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads an Int64 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<long>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads a <see cref="float" /> from the beginning of a read-only span of bytes, as little endian.
        /// </summary>
        /// <param name="source">The read-only span to read.</param>
        /// <returns>The little endian value.</returns>
        /// <remarks>Reads exactly 4 bytes from the beginning of the span.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> is too small to contain a <see cref="float" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadSingleLittleEndian(ReadOnlyBigSpan<byte> source)
            => !BitConverter.IsLittleEndian ? Int32BitsToSingle(ReverseEndianness(source.Read<int>())) : source.Read<float>();

        /// <summary>
        /// Reads a UInt16 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<ushort>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads a UInt32 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<uint>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads a UInt64 out of a read-only span of bytes as little endian.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64LittleEndian(ReadOnlyBigSpan<byte> source)
        {
            var result = source.Read<ulong>();
            return BitConverter.IsLittleEndian ? result : ReverseEndianness(result);
        }

        /// <summary>
        /// Reads a <see cref="double" /> from the beginning of a read-only span of bytes, as little endian.
        /// </summary>
        /// <param name="source">The read-only span of bytes to read.</param>
        /// <param name="value">When this method returns, the value read out of the read-only span of bytes, as little endian.</param>
        /// <returns>
        /// <see langword="true" /> if the span is large enough to contain a <see cref="double" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>Reads exactly 8 bytes from the beginning of the span.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadDoubleLittleEndian(ReadOnlyBigSpan<byte> source, out double value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out long tmp);
            value = BitConverter.Int64BitsToDouble(ReverseEndianness(tmp));
            return success;
        }

        /// <summary>
        /// Reads an Int16 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain an Int16, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt16LittleEndian(ReadOnlyBigSpan<byte> source, out short value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out short tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }

        /// <summary>
        /// Reads an Int32 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain an Int32, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt32LittleEndian(ReadOnlyBigSpan<byte> source, out int value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out int tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }

        /// <summary>
        /// Reads an Int64 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain an Int64, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadInt64LittleEndian(ReadOnlyBigSpan<byte> source, out long value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out long tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }

        /// <summary>
        /// Reads a <see cref="float" /> from the beginning of a read-only span of bytes, as little endian.
        /// </summary>
        /// <param name="source">The read-only span of bytes to read.</param>
        /// <param name="value">When this method returns, the value read out of the read-only span of bytes, as little endian.</param>
        /// <returns>
        /// <see langword="true" /> if the span is large enough to contain a <see cref="float" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>Reads exactly 4 bytes from the beginning of the span.</remarks>
        public static bool TryReadSingleLittleEndian(ReadOnlyBigSpan<byte> source, out float value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);
            var success = source.TryRead(out int tmp);
            value = Int32BitsToSingle(ReverseEndianness(tmp));
            return success;
        }

        /// <summary>
        /// Reads a UInt16 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain a UInt16, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt16LittleEndian(ReadOnlyBigSpan<byte> source, out ushort value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out ushort tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }

        /// <summary>
        /// Reads a UInt32 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain a UInt32, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt32LittleEndian(ReadOnlyBigSpan<byte> source, out uint value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out uint tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }

        /// <summary>
        /// Reads a UInt64 out of a read-only span of bytes as little endian.
        /// </summary>
        /// <returns>If the span is too small to contain a UInt64, return false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryReadUInt64LittleEndian(ReadOnlyBigSpan<byte> source, out ulong value)
        {
            if (BitConverter.IsLittleEndian)
                return source.TryRead(out value);

            var success = source.TryRead(out ulong tmp);
            if (success)
                value = ReverseEndianness(tmp);
            else
                Unsafe.SkipInit(out value);
            return success;
        }
    }
}
