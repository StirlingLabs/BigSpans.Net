using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities
{
    public static partial class BinaryPrimitives
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReverseEndianness(byte value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReverseEndianness(sbyte value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReverseEndianness(short value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReverseEndianness(ushort value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReverseEndianness(int value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReverseEndianness(uint value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReverseEndianness(long value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReverseEndianness(ulong value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
    }
}
