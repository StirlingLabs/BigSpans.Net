using System;
using System.IO;

namespace StirlingLabs.Utilities;

public static class StreamExtensions
{
#if NETSTANDARD2_1_OR_GREATER || NET
    public static void Write(this Stream stream, ReadOnlyBigSpan<byte> bigSpan)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        bigSpan.AsSmallSlices(stream.Write);
    }
    public static ulong Read(this Stream stream, BigSpan<byte> bigSpan)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));

        ulong total = 0;

        bigSpan.AsSmallSlices(span => {
            var stride = stream.Read(span);
            var length = (uint)span.Length;
            if (stride < length)
                return false;
            total += checked((uint)stride);
            return true;
        });

        return total;
    }
#endif
}
