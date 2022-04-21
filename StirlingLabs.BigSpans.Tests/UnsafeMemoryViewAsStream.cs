using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace StirlingLabs.BigSpans.Tests;

[ExcludeFromCodeCoverage]
internal unsafe class UnsafeMemoryViewAsStream : Stream
{
    private byte* _p;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UnsafeMemoryViewAsStream(byte* p)
        => _p = p;

    public UnsafeMemoryViewAsStream()
        => _p = default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Flush() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int ReadByte()
    {
        return *_p++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void WriteByte(byte value)
        => *_p++ = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int Read(byte[] buffer, int offset, int count)
    {
        fixed (void* pBuffer = buffer)
            Buffer.MemoryCopy(_p, (byte*)pBuffer + offset, count, count);
        _p += count;
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override long Seek(long offset, SeekOrigin origin)
        => origin switch
        {
            SeekOrigin.Begin => (long)(_p = (byte*)offset),
            SeekOrigin.Current => (long)(_p += offset),
            SeekOrigin.End => (long)(_p = (byte*)checked(nuint.MaxValue - (ulong)offset)),
            _ => throw new NotImplementedException()
        };

    public override void SetLength(long value)
        => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Write(byte[] buffer, int offset, int count)
    {
        fixed (void* pBuffer = buffer)
            Buffer.MemoryCopy((byte*)pBuffer + offset, _p, count, count);
        _p += count;
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => nint.MaxValue;

    public override long Position
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (long)_p;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _p = (byte*)value;
    }

    public byte* Pointer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _p;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _p = value;
    }

    public nuint Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (nuint)_p;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _p = (byte*)value;
    }
}
