using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities;

internal static partial class BigSpanHelpers
{
    [SuppressMessage("Maintainability", "CA1502", Justification = "Copy of official implementation")]
    [SuppressMessage("ReSharper", "CognitiveComplexity")]
    public static unsafe void ClearWithoutReferences(ref byte b, nuint byteLength)
    {
        if (byteLength == 0)
            return;

        if (Is64Bit)
        {
            ZeroMemory(ref b, byteLength);
            return;
        }

        // TODO: Optimize other platforms to be on par with AMD64 CoreCLR
        // Note: It's important that this switch handles lengths at least up to 22.
        // See notes below near the main loop for why.

        // The switch will be very fast since it can be implemented using a jump
        // table in assembly. See http://stackoverflow.com/a/449297/4077294 for more info.

        switch (byteLength)
        {
            case 1:
                b = 0;
                return;
            case 2:
                Unsafe.As<byte, short>(ref b) = 0;
                return;
            case 3:
                Unsafe.As<byte, short>(ref b) = 0;
                Unsafe.Add(ref b, 2) = 0;
                return;
            case 4:
                Unsafe.As<byte, int>(ref b) = 0;
                return;
            case 5:
                Unsafe.As<byte, int>(ref b) = 0;
                Unsafe.Add(ref b, 4) = 0;
                return;
            case 6:
                Unsafe.As<byte, int>(ref b) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 4)) = 0;
                return;
            case 7:
                Unsafe.As<byte, int>(ref b) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 4)) = 0;
                Unsafe.Add(ref b, 6) = 0;
                return;
            case 8:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                return;
            case 9:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.Add(ref b, 8) = 0;
                return;
            case 10:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 8)) = 0;
                return;
            case 11:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 8)) = 0;
                Unsafe.Add(ref b, 10) = 0;
                return;
            case 12:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                return;
            case 13:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                Unsafe.Add(ref b, 12) = 0;
                return;
            case 14:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 12)) = 0;
                return;
            case 15:
                if (Is64Bit)
                    Unsafe.As<byte, long>(ref b) = 0;
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 12)) = 0;
                Unsafe.Add(ref b, 14) = 0;
                return;
            case 16:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                return;
            case 17:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.Add(ref b, 16) = 0;
                return;
            case 18:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 16)) = 0;
                return;
            case 19:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 16)) = 0;
                Unsafe.Add(ref b, 18) = 0;
                return;
            case 20:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                return;
            case 21:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                Unsafe.Add(ref b, 20) = 0;
                return;
            case 22:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 20)) = 0;
                return;
            case 23:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 20)) = 0;
                Unsafe.Add(ref b, 22) = 0;
                return;
            case 24:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                return;
            case 25:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.Add(ref b, 24) = 0;
                return;
            case 26:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 24)) = 0;
                return;
            case 27:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 24)) = 0;
                Unsafe.Add(ref b, 26) = 0;
                return;
            case 28:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 24)) = 0;
                return;
            case 29:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 24)) = 0;
                Unsafe.Add(ref b, 28) = 0;
                return;
            case 30:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 24)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 28)) = 0;
                return;
            case 31:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                }
                Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 24)) = 0;
                Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 28)) = 0;
                Unsafe.Add(ref b, 30) = 0;
                return;
            case 32:
                if (Is64Bit)
                {
                    Unsafe.As<byte, long>(ref b) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 24)) = 0;
                }
                else
                {
                    Unsafe.As<byte, int>(ref b) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 4)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 12)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 20)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 24)) = 0;
                    Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 28)) = 0;
                }
                return;
        }

        // P/Invoke into the native version for large lengths
        if (byteLength >= 512)
        {
            ZeroMemory(ref b, byteLength);
            return;
        }

        nuint i = 0; // byte offset at which we're copying

        if (((nuint)Unsafe.AsPointer(ref b) & 3) != 0)
        {
            if (((nuint)Unsafe.AsPointer(ref b) & 1) != 0)
            {
                b = 0;
                i += 1;
                if (((nuint)Unsafe.AsPointer(ref b) & 2) != 0)
                    goto IntAligned;
            }
            Unsafe.As<byte, short>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
            i += 2;
        }

        IntAligned:

        // On 64-bit IntPtr.Size == 8, so we want to advance to the next 8-aligned address. If
        // (int)b % 8 is 0, 5, 6, or 7, we will already have advanced by 0, 3, 2, or 1
        // bytes to the next aligned address (respectively), so do nothing. On the other hand,
        // if it is 1, 2, 3, or 4 we will want to copy-and-advance another 4 bytes until
        // we're aligned.
        // The thing 1, 2, 3, and 4 have in common that the others don't is that if you
        // subtract one from them, their 3rd lsb will not be set. Hence, the below check.

        if ((((nuint)Unsafe.AsPointer(ref b) - 1) & 4) == 0)
        {
            Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
            i += 4;
        }

        var end = byteLength - 16;
        byteLength -= i; // lower 4 bits of byteLength represent how many bytes are left *after* the unrolled loop

        // We know due to the above switch-case that this loop will always run 1 iteration; max
        // bytes we clear before checking is 23 (7 to align the pointers, 16 for 1 iteration) so
        // the switch handles lengths 0-22.
        Debug.Assert(end >= 7 && i <= end);

        // This is separated out into a different variable, so the i + 16 addition can be
        // performed at the start of the pipeline and the loop condition does not have
        // a dependency on the writes.
        nuint counter;

        do
        {
            counter = i + 16;

            // This loop looks very costly since there appear to be a bunch of temporary values
            // being created with the adds, but the jit (for x86 anyways) will convert each of
            // these to use memory addressing operands.

            // So the only cost is a bit of code size, which is made up for by the fact that
            // we save on writes to b.

            if (Is64Bit)
            {
                Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
                Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, (nint)i + 8)) = 0;
            }
            else
            {
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i + 4)) = 0;
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i + 8)) = 0;
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i + 12)) = 0;
            }

            i = counter;

            // See notes above for why this wasn't used instead
            // i += 16;
        } while (counter <= end);

        if ((byteLength & 8) != 0)
        {
            if (Is64Bit)
                Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
            else
            {
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
                Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i + 4)) = 0;
            }
            i += 8;
        }
        if ((byteLength & 4) != 0)
        {
            Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
            i += 4;
        }
        if ((byteLength & 2) != 0)
        {
            Unsafe.As<byte, short>(ref Unsafe.AddByteOffset(ref b, (nint)i)) = 0;
            i += 2;
        }
        if ((byteLength & 1) != 0)
            Unsafe.AddByteOffset(ref b, (nint)i) = 0;
        // We're not using i after this, so not needed
        // i += 1;
    }
}
