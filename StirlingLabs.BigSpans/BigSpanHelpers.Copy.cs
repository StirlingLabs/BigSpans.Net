using System;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities
{
    internal static partial class BigSpanHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(byte* destinationPointer, byte* sourcePointer, nuint length)
        {
            Buffer.MemoryCopy(sourcePointer, destinationPointer, length, length);
            /*
            var dist = (nuint)(destinationPointer - sourcePointer);
            if (dist < length)
            {
                Buffer.MemoryCopy(sourcePointer, destinationPointer, length, length);
                return;
            }

            var l = length;
            ref var dstRef = ref Unsafe.AsRef<byte>(destinationPointer);
            ref var srcRef = ref Unsafe.AsRef<byte>(sourcePointer);
            while (l >= uint.MaxValue)
            {
                Unsafe.CopyBlockUnaligned(ref dstRef, ref srcRef, uint.MaxValue);
                // ReSharper disable RedundantOverflowCheckingContext // CS8778
                dstRef = ref Unsafe.AddByteOffset(ref dstRef, unchecked((nint)uint.MaxValue));
                srcRef = ref Unsafe.AddByteOffset(ref srcRef, unchecked((nint)uint.MaxValue));
                // ReSharper restore RedundantOverflowCheckingContext
                l -= uint.MaxValue;
            }
            if (l > 0)
                Unsafe.CopyBlockUnaligned(ref dstRef, ref srcRef, (uint)l);
            */
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(void* destinationPointer, void* sourcePointer, nuint length)
            => Copy((byte*)destinationPointer, (byte*)sourcePointer, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy<T>(T* destinationPointer, T* sourcePointer, nuint count) where T : unmanaged
        {
            var l = checked((nuint)((ulong)sizeof(T) * count));
            Copy((void*)destinationPointer, sourcePointer, l);
        }
    }
}
