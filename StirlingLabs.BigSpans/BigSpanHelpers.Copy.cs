using System;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities
{
    internal static partial class BigSpanHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Copy(byte* destinationPointer, byte* sourcePointer, nuint length)
            => Buffer.MemoryCopy(sourcePointer, destinationPointer, length, length);

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
