#nullable enable
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using InlineIL;
using JetBrains.Annotations;
using static InlineIL.IL;
using static InlineIL.IL.Emit;

namespace StirlingLabs.Utilities;

[PublicAPI]
public static class BigSpanExtensions
{
    /// <summary>
    /// Returns a reference to the 0th element of the BigSpan. If the BigSpan is empty, returns a reference to the location where the 0th element
    /// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
    /// </summary>
    public static ref T GetReference<T>(this BigSpan<T> span)
        => ref span._pointer.Value;
    /// <summary>
    /// Returns a reference to the 0th element of the ReadOnlyBigSpan. If the ReadOnlyBigSpan is empty, returns a reference to the location where the 0th element
    /// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
    /// </summary>
    public static ref T GetReference<T>(this ReadOnlyBigSpan<T> span)
        => ref span._pointer.Value;

    public static void CopyTo<T>(this T[] srcArray, BigSpan<T> dst)
        => new BigSpan<T>(srcArray, false).CopyTo(dst);

    /// <summary>
    /// Copies the contents of this span into destination span. If the source
    /// and destinations overlap, this method behaves as if the original values in
    /// a temporary location before the destination is overwritten.
    /// </summary>
    /// <param name="src">The span to copy items from.</param>
    /// <param name="dst">The span to copy items into.</param>
    /// <exception cref="System.ArgumentException">
    /// Thrown when the destination Span is shorter than the source Span.
    /// </exception>
    public static unsafe void CopyTo<T>(this Span<T> src, BigSpan<T> dst)
    {
        DeclareLocals(
            new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                .Pinned(),
            new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                .Pinned()
        );

        var sizeOfT = (nuint)Unsafe.SizeOf<T>();
        var srcLen = (nuint)src.Length * sizeOfT;
        var dstLen = dst.Length * sizeOfT;
        if (srcLen > dstLen)
            throw new ArgumentException("Too short.", nameof(dst));

        var length = srcLen < dstLen ? srcLen : dstLen;
        if (length <= 0) return;

        Push(ref dst.GetPinnableReference()!);
        Stloc("rDst");
        Push(ref src.GetPinnableReference()!);
        Stloc("rSrc");
        Ldloc("rDst");
        Pop(out var pDst);
        Ldloc("rSrc");
        Pop(out var pSrc);
        if (pDst == default) throw new ArgumentNullException(nameof(dst));
        if (pSrc == default) throw new ArgumentNullException(nameof(src));

        BigSpanHelpers.Copy(pDst, pSrc, length);
    }

    /// <summary>
    /// Copies the contents of this span into destination span. If the source
    /// and destinations overlap, this method behaves as if the original values in
    /// a temporary location before the destination is overwritten.
    /// </summary>
    /// <param name="src">The span to copy items from.</param>
    /// <param name="dst">The span to copy items into.</param>
    /// <returns>If the destination span is shorter than the source span, this method
    /// return false and no data is written to the destination.</returns>
    public static unsafe bool TryCopyTo<T>(this Span<T> src, BigSpan<T> dst)
    {
        DeclareLocals(
            new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                .Pinned(),
            new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                .Pinned()
        );

        var sizeOfT = (nuint)Unsafe.SizeOf<T>();
        var srcLen = (nuint)src.Length * sizeOfT;
        var dstLen = dst.Length * sizeOfT;
        if (srcLen > dstLen)
            return false;

        var length = srcLen < dstLen ? srcLen : dstLen;
        if (length <= 0) return true;

        Push(ref dst.GetPinnableReference()!);
        Stloc("rDst");
        Push(ref src.GetPinnableReference()!);
        Stloc("rSrc");
        Ldloc("rDst");
        Pop(out var pDst);
        Ldloc("rSrc");
        Pop(out var pSrc);
        if (pDst == default) throw new ArgumentNullException(nameof(dst));
        if (pSrc == default) throw new ArgumentNullException(nameof(src));

        BigSpanHelpers.Copy(pDst, pSrc, length);
        return true;
    }

    /// <summary>
    /// Copies the contents of this span into destination span. If the source
    /// and destinations overlap, this method behaves as if the original values in
    /// a temporary location before the destination is overwritten.
    /// </summary>
    /// <param name="src">The span to copy items from.</param>
    /// <param name="dst">The span to copy items into.</param>
    /// <exception cref="System.ArgumentException">
    /// Thrown when the destination Span is shorter than the source Span.
    /// </exception>
    public static unsafe void CopyTo<T>(this ReadOnlySpan<T> src, BigSpan<T> dst)
    {
        DeclareLocals(
            new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                .Pinned(),
            new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                .Pinned()
        );

        var sizeOfT = (nuint)Unsafe.SizeOf<T>();
        var srcLen = (nuint)src.Length * sizeOfT;
        var dstLen = dst.Length * sizeOfT;
        if (srcLen > dstLen)
            throw new ArgumentException("Too short.", nameof(dst));

        var length = srcLen < dstLen ? srcLen : dstLen;
        if (length <= 0) return;

        Push(ref dst.GetPinnableReference()!);
        Stloc("rDst");
        Push(ref Unsafe.AsRef(src.GetPinnableReference())!);
        Stloc("rSrc");
        Ldloc("rDst");
        Pop(out var pDst);
        Ldloc("rSrc");
        Pop(out var pSrc);
        if (pDst == default) throw new ArgumentNullException(nameof(dst));
        if (pSrc == default) throw new ArgumentNullException(nameof(src));

        BigSpanHelpers.Copy(pDst, pSrc, length);
    }

    /// <summary>
    /// Copies the contents of this span into destination span. If the source
    /// and destinations overlap, this method behaves as if the original values in
    /// a temporary location before the destination is overwritten.
    /// </summary>
    /// <param name="src">The span to copy items from.</param>
    /// <param name="dst">The span to copy items into.</param>
    /// <returns>If the destination span is shorter than the source span, this method
    /// return false and no data is written to the destination.</returns>
    public static unsafe bool TryCopyTo<T>(this ReadOnlySpan<T> src, BigSpan<T> dst)
    {
        DeclareLocals(
            new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                .Pinned(),
            new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                .Pinned()
        );

        var sizeOfT = (nuint)Unsafe.SizeOf<T>();
        var srcLen = (nuint)src.Length * sizeOfT;
        var dstLen = dst.Length * sizeOfT;
        if (srcLen > dstLen)
            return false;

        var length = srcLen < dstLen ? srcLen : dstLen;
        if (length <= 0) return true;

        Push(ref dst.GetPinnableReference()!);
        Stloc("rDst");
        Push(ref Unsafe.AsRef(src.GetPinnableReference())!);
        Stloc("rSrc");
        Ldloc("rDst");
        Pop(out var pDst);
        Ldloc("rSrc");
        Pop(out var pSrc);
        if (pDst == default) throw new ArgumentNullException(nameof(dst));
        if (pSrc == default) throw new ArgumentNullException(nameof(src));

        BigSpanHelpers.Copy(pDst, pSrc, length);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this BigSpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this ReadOnlyBigSpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this BigSpan<T> a, ReadOnlyBigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this ReadOnlyBigSpan<T> a, ReadOnlyBigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this Span<T> a, BigSpan<T> b)
        where T : unmanaged
        => b.CompareMemory(a) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this ReadOnlySpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => b.CompareMemory(a) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this BigSpan<T> a, ReadOnlySpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequenceEqual<T>(this ReadOnlyBigSpan<T> a, ReadOnlySpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this BigSpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this ReadOnlyBigSpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this BigSpan<T> a, ReadOnlyBigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this ReadOnlyBigSpan<T> a, ReadOnlyBigSpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this Span<T> a, BigSpan<T> b)
        where T : unmanaged
        => -b.CompareMemory(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this ReadOnlySpan<T> a, BigSpan<T> b)
        where T : unmanaged
        => -b.CompareMemory(a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this BigSpan<T> a, ReadOnlySpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SequenceCompare<T>(this ReadOnlyBigSpan<T> a, ReadOnlySpan<T> b)
        where T : unmanaged
        => a.CompareMemory(b);

    /// <summary>
    /// Writes a structure of type T into a span of bytes.
    /// </summary>
    /// <returns>If the span is too small to contain the type T, return false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryWrite<T>(this BigSpan<byte> destination, in T value)
        where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() > (uint)destination.Length)
            return false;
        Unsafe.WriteUnaligned(ref destination.GetReference(), value);
        return true;
    }

    /// <summary>
    /// Writes a structure of type T into a span of bytes.
    /// </summary>
    /// <returns>If the span is too small to contain the type T, return false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryWrite<T>(this BigSpan<byte> destination, in T value, nuint offset)
        where T : unmanaged
    {
        if (offset + (nuint)Unsafe.SizeOf<T>() > destination.Length)
            return false;
        Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref destination.GetReference(), (nint)offset), value);
        return true;
    }

    /// <summary>
    /// Writes a structure of type T into a span of bytes.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(this BigSpan<byte> destination, in T value)
        where T : unmanaged
    {
        if (!TryWrite(destination, value))
            throw new ArgumentOutOfRangeException(nameof(destination));
    }

    /// <summary>
    /// Writes a structure of type T into a span of bytes.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(this BigSpan<byte> destination, in T value, nuint offset)
        where T : unmanaged
    {
        if (!TryWrite(destination, value, offset))
            throw new ArgumentOutOfRangeException(nameof(destination));
    }

    /// <summary>
    /// Reads a structure of type T from a span of bytes.
    /// </summary>
    /// <returns>If the span is too small to contain the type T, return false.</returns>
    public static bool TryRead<T>(this ReadOnlyBigSpan<byte> source, out T value)
    {
        if (source.Length < (nuint)Unsafe.SizeOf<T>())
        {
            Unsafe.SkipInit(out value);
            return false;
        }
        value = source.Read<T>();
        return true;
    }

    /// <summary>
    /// Reads a structure of type T from a span of bytes.
    /// </summary>
    public static ref readonly T Read<T>(this ReadOnlyBigSpan<byte> source)
        => ref Unsafe.As<byte, T>(ref source.GetReference());

    /// <summary>
    /// Defines a conversion of a <see cref="BigSpan{T}"/> to a <see cref="ReadOnlyBigSpan{T}"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly ReadOnlyBigSpan<T> AsReadOnlyBigSpan<T>(in this BigSpan<T> span)
    {
        Ldarg_0();
        Ret();
        throw Unreachable();
    }

    public static void AsSmallSlices<T>(in this BigSpan<T> span, [InstantHandle] SpanAction<T> action)
        => span.AsSmallSlices(int.MaxValue, action);

    public static void AsSmallSlices<T>(in this BigSpan<T> span, int size, [InstantHandle] SpanAction<T> action)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (action is null) throw new ArgumentNullException(nameof(action));
        var uSize = (uint)size;

        nuint start = 0;

        for (;;)
        {
            var maxSize = span.Length - start;
            if (maxSize == 0) return;
            var sliceSize = (int)Math.Min(uSize, maxSize);
            var slice = span.Slice(start, sliceSize);
            start += (uint)sliceSize;
            action(slice);
        }
    }

    public static void AsSmallSlices<T>(in this BigSpan<T> span, [InstantHandle] SpanFunc<T, bool> func)
        => span.AsSmallSlices(int.MaxValue, func);

    public static void AsSmallSlices<T>(in this BigSpan<T> span, int size, [InstantHandle] SpanFunc<T, bool> func)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (func is null) throw new ArgumentNullException(nameof(func));
        var uSize = (uint)size;

        nuint start = 0;

        for (;;)
        {
            var maxSize = span.Length - start;
            if (maxSize == 0) return;
            var sliceSize = (int)Math.Min(uSize, maxSize);
            var slice = span.Slice(start, sliceSize);
            start += (uint)sliceSize;
            if (!func(slice)) break;
        }
    }

    public static void AsSmallSlices<T>(in this ReadOnlyBigSpan<T> span, ReadOnlySpanAction<T> action)
        => span.AsSmallSlices(int.MaxValue, action);

    public static void AsSmallSlices<T>(in this ReadOnlyBigSpan<T> span, int size, ReadOnlySpanAction<T> action)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (action is null) throw new ArgumentNullException(nameof(action));
        var uSize = (uint)size;

        nuint start = 0;

        for (;;)
        {
            var maxSize = span.Length - start;
            if (maxSize == 0) return;
            var sliceSize = (int)Math.Min(uSize, maxSize);
            var slice = span.Slice(start, sliceSize);
            start += (uint)sliceSize;
            action(slice);
        }
    }

    public static TResult[] SelectSmallSlices<T, TResult>(in this BigSpan<T> span, SpanFunc<T, TResult> fn)
        => span.SelectSmallSlices(int.MaxValue, fn);

    public static TResult[] SelectSmallSlices<T, TResult>(this BigSpan<T> span, int size, SpanFunc<T, TResult> fn)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (fn is null) throw new ArgumentNullException(nameof(fn));
        var uSize = (uint)size;

        nuint start = 0;

        var sliceCount = span.Length / uSize;

        if (sliceCount == 0) return Array.Empty<TResult>();

        var resultIndex = 0;
        var results = new TResult[sliceCount];

        for (;;)
        {
            var maxSize = span.Length - start;
            if (maxSize == 0) return results;
            var sliceSize = (int)Math.Min(uSize, maxSize);
            var slice = span.Slice(start, sliceSize);
            start += (uint)sliceSize;
            results[resultIndex++] = fn(slice);
        }
    }

    public static TResult[] SelectSmallSlices<T, TResult>(in this ReadOnlyBigSpan<T> span, ReadOnlySpanFunc<T, TResult> fn)
        => span.SelectSmallSlices(int.MaxValue, fn);

    public static TResult[] SelectSmallSlices<T, TResult>(this ReadOnlyBigSpan<T> span, int size, ReadOnlySpanFunc<T, TResult> fn)
    {
        if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));
        if (fn is null) throw new ArgumentNullException(nameof(fn));
        var uSize = (uint)size;

        nuint start = 0;

        var sliceCount = span.Length / uSize;

        if (sliceCount == 0) return Array.Empty<TResult>();

        var resultIndex = 0;
        var results = new TResult[sliceCount];

        for (;;)
        {
            var maxSize = span.Length - start;
            if (maxSize == 0) return results;
            var sliceSize = (int)Math.Min(uSize, maxSize);
            var slice = span.Slice(start, sliceSize);
            start += (uint)sliceSize;
            results[resultIndex++] = fn(slice);
        }
    }
}

[PublicAPI]
public delegate void SpanAction<T>(Span<T> span);

[PublicAPI]
public delegate void ReadOnlySpanAction<T>(ReadOnlySpan<T> span);

[PublicAPI]
public delegate TResult SpanFunc<T, out TResult>(Span<T> span);

[PublicAPI]
public delegate TResult ReadOnlySpanFunc<T, out TResult>(ReadOnlySpan<T> span);

[PublicAPI]
public delegate void BigSpanAction<T>(BigSpan<T> span);

[PublicAPI]
public delegate void ReadOnlyBigSpanAction<T>(ReadOnlyBigSpan<T> span);

[PublicAPI]
public delegate TResult BigSpanFunc<T, out TResult>(BigSpan<T> span);

[PublicAPI]
public delegate TResult ReadOnlyBigSpanFunc<T, out TResult>(ReadOnlyBigSpan<T> span);
