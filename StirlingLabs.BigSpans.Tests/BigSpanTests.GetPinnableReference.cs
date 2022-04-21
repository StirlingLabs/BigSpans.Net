using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using StirlingLabs.Utilities;

namespace StirlingLabs.BigSpans.Tests;

public static partial class BigSpanTests
{
    [Test]
    public static void GetPinnableReferenceArray()
    {
        int[] a = { 91, 92, 93, 94, 95 };
        var span = new BigSpan<int>(a, 1, 3);
        ref var pinnableReference = ref span.GetPinnableReference();
        Assert.True(Unsafe.AreSame(ref a[1], ref pinnableReference));
    }

    [Test]
    public static unsafe void UsingSpanInFixed()
    {
        byte[] a = { 91, 92, 93, 94, 95 };
        var span = (BigSpan<byte>)a;
        fixed (byte* ptr = span)
        {
            for (nuint i = 0; i < span.Length; i++)
            {
                Assert.AreEqual(a[i], ptr[i]);
            }
        }
    }

    [Test]
    [SuppressMessage("Maintainability", "CA1508", Justification = "Empty span usage test case.")]
    public static unsafe void UsingEmptySpanInFixed()
    {
        var span = BigSpan<int>.Empty;
        fixed (int* ptr = span)
        {
            Assert.True(ptr == null);
        }

        // ReSharper disable HeuristicUnreachableCode
        var spanFromEmptyArray = (BigSpan<int>)Array.Empty<int>();
        fixed (int* ptr = spanFromEmptyArray)
        {
            Assert.True(ptr == null);
        }
        // ReSharper restore HeuristicUnreachableCode
    }

    [Test]
    public static unsafe void GetPinnableReferenceArrayPastEnd()
    {
        // The only real difference between GetPinnableReference() and "ref span[0]" is that
        // GetPinnableReference() of a zero-length won't throw an IndexOutOfRange but instead return a null ref.

        int[] a = { 91, 92, 93, 94, 95 };
        var span = new BigSpan<int>(a, (nuint)a.LongLength, 0);
        ref var pinnableReference = ref span.GetPinnableReference();
        ref var expected = ref Unsafe.AsRef<int>(null);
        Assert.True(Unsafe.AreSame(ref expected, ref pinnableReference));
    }

    [Test]
    public static unsafe void GetPinnableReferencePointer()
    {
        var i = 42;
        var span = new BigSpan<int>(&i, 1);
        ref var pinnableReference = ref span.GetPinnableReference();
        Assert.True(Unsafe.AreSame(ref i, ref pinnableReference));
    }

    [Test]
    public static unsafe void GetPinnableReferenceEmpty()
    {
        var span = BigSpan<int>.Empty;
        ref var pinnableReference = ref span.GetPinnableReference();
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<int>(null), ref pinnableReference));

        span = (BigSpan<int>)Array.Empty<int>();
        pinnableReference = ref span.GetPinnableReference();
        Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<int>(null), ref pinnableReference));
    }
}
