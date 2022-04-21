using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities;

internal static partial class BigSpanHelpers
{
    internal static void ZeroMemory(ref byte b, nuint byteLength)
    {
        if (Is64Bit)
        {
            ref var pb = ref b;
            var l = (ulong)byteLength;
            while (l >= uint.MaxValue)
            {
                Unsafe.InitBlockUnaligned(ref pb, 0, uint.MaxValue);
                // ReSharper disable once RedundantOverflowCheckingContext // CS8778
                pb = ref Unsafe.AddByteOffset(ref pb, unchecked((nint)uint.MaxValue));
                l -= uint.MaxValue;
            }
            if (l > 0)
                Unsafe.InitBlockUnaligned(ref pb, 0, (uint)l);
        }
        else
            Unsafe.InitBlockUnaligned(ref b, 0, (uint)byteLength);
    }
}
