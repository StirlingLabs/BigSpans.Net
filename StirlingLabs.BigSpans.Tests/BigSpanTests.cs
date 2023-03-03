using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using StirlingLabs.Utilities;
using StirlingLabs.Utilities.Assertions;

namespace StirlingLabs.BigSpans.Tests;

[ExcludeFromCodeCoverage]
public static partial class BigSpanTests
{
    [Test]
    public static void BigSpanSize()
    {
        if (BigSpanHelpers.Is64Bit)
        {
#if NET7_0_OR_GREATER
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfByReference<byte>());
#else
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfByReference<byte>());
#endif

            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfBigSpan<byte>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<byte>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfBigSpan<int>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<int>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfBigSpan<double>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<double>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfBigSpan<object>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<object>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfBigSpan<string>());
            Assert.AreEqual(16, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<string>());
        }
        else
        {
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfByReference<byte>());

            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfBigSpan<byte>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<byte>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfBigSpan<int>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<int>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfBigSpan<double>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<double>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfBigSpan<object>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<object>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfBigSpan<string>());
            Assert.AreEqual(8, BigSpanHelpers.GetSizeOfReadOnlyBigSpan<string>());
        }
    }

    [Test]
    public static void IndexAccess()
    {
        var expected = new object();
        var span = new BigSpan<object>(new[] { expected });

        var actual = span[0u];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public static void OutOfBoundsIndexAccess()
    {
        var span = new BigSpan<object>();

        BigSpanAssert<object>
            .Throws<IndexOutOfRangeException>(span, s => s[0u]);
    }

    [Test]
    public static void IndexAccessByIndex()
    {
        var expected = new object();
        var span = new BigSpan<object>(new[] { expected });

        var actual = span[^1];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public static void RangeAccess()
    {
        var expected = new object();
        var span = new BigSpan<object>(new[] { expected });

        var actual = span[..^1][0u];

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public static void HoldsObjectReference()
    {

        var garbageCollected = 0uL;

        void OnGc() => ++garbageCollected;

        try
        {
            //var wrCtrl = CreateWeakRefObject();
            var bsObj = CreateObjectRefs(out var wrObj, out var sObj);

            GarbageCollectedNotifier.GarbageCollected += OnGc;

            var collected = false;

            for (var i = 0; i < 10000; ++i)
            {
                Unsafe.AreSame(ref sObj[0], ref bsObj[0u]);

                if (wrObj.IsAlive)
                {
                    // not yet collected
                }
                else
                {
                    collected = true;
                }
                GC.Collect(0, GCCollectionMode.Forced, true, true);

                if (collected) return;
            }

            Assert.IsFalse(collected);
        }
        finally
        {
            GarbageCollectedNotifier.GarbageCollected -= OnGc;
        }
    }

#if DEBUG
    [Ignore("Garbage collector will hold a reference to items expected to be collected under Debug mode.")]
#endif
    [Test]
    public static void ReleasesObjectReference()
    {

        var garbageCollected = 0uL;

        void OnGc() => ++garbageCollected;

        try
        {
            Weak<object> wrObj;
            bool collected;

            [MethodImpl(MethodImplOptions.NoInlining)]
            Weak<object> ActiveStackScope() {
                //var wrCtrl = CreateWeakRefObject();
                var bsObj = CreateObjectRefs(out wrObj, out var sObj);
                //var sObj = CreateObjectRefs(out var wrObj);

                GarbageCollectedNotifier.GarbageCollected += OnGc;

                collected = false;

                for (var i = 0; i < 10000; ++i) {
                    Unsafe.AreSame(ref sObj[0], ref bsObj[0u]);

                    if (!wrObj.IsAlive)
                        collected = true;
                    // else not yet collected

                    GC.Collect(2, GCCollectionMode.Forced, true, true);

                    if (collected) return wrObj;
                }

                Assert.IsFalse(collected);

                sObj = null;
                bsObj = default;
                return wrObj;
            }

            wrObj = ActiveStackScope();

            for (var i = 0; i < 10000; ++i)
            {
                if (!wrObj.IsAlive)
                    collected = true;

                GC.Collect(2, GCCollectionMode.Forced, true, true);

                if (collected) return;
            }

            Assert.IsTrue(collected);
        }
        finally
        {
            GarbageCollectedNotifier.GarbageCollected -= OnGc;
        }
    }

    [Test]
    public static unsafe void ImplicitSpanUpgradeTest2G()
    {
        var notTwoGigs = new Span<byte>((void*)0, int.MaxValue);
        BigSpan<byte> twoGigs = notTwoGigs;
        Assert.AreEqual((nuint)int.MaxValue, twoGigs.Length);
    }

    [Test]
    public static void ImplicitSpanUpgradeTest4G()
    {
        var notFourGigs = MemoryMarshal.CreateSpan(ref Unsafe.NullRef<byte>(), unchecked((int)uint.MaxValue));
        BigSpan<byte> fourGigs = notFourGigs;
        Assert.AreEqual((nuint)uint.MaxValue, fourGigs.Length);
    }

    [Test]
    public static void SpanStackCopyTest4G()
    {
        var fourGigs = new BigSpan<byte>(ref Unsafe.NullRef<byte>(), uint.MaxValue);

        Assert.AreEqual((nuint)uint.MaxValue, fourGigs.Length);

        var fourGigsCopy = fourGigs;

        Assert.AreEqual((nuint)uint.MaxValue, fourGigsCopy.Length);

        var embeddedFourGigsCopy = new EmbeddingExample { Bytes = fourGigs };

        Assert.AreEqual((nuint)uint.MaxValue, embeddedFourGigsCopy.Bytes.Length);
    }

    [Test]
    public static void SpanStackCopyTestMax()
    {

        var maxSpan = new BigSpan<byte>(ref Unsafe.NullRef<byte>(), nuint.MaxValue);

        Assert.AreEqual(nuint.MaxValue, maxSpan.Length);

        var maxSpanCopy = maxSpan;

        Assert.AreEqual(nuint.MaxValue, maxSpanCopy.Length);

        var embeddedMaxSpanCopy = new EmbeddingExample { Bytes = maxSpan };

        Assert.AreEqual(nuint.MaxValue, embeddedMaxSpanCopy.Bytes.Length);
    }

    internal ref struct EmbeddingExample
    {
        public BigSpan<byte> Bytes;
    }

    private static BigSpan<object> CreateObjectRefs(out Weak<object> wr, out Span<object> sp)
    {
        var o = new object();
        wr = new(o);
        sp = MemoryMarshal.CreateSpan(ref o, 1)!;
        return new(ref sp.GetPinnableReference(), 1);
    }

    private static Weak<object> CreateWeakRefObject()
        => new(new());

#if DEBUG
    [Ignore("Garbage collector will hold a reference to items expected to be collected under Debug mode.")]
#endif
    [Test]
    public static void ObjPinnedEnumForEachTest()
    {
        var garbageCollected = 0uL;

        void OnGc() => ++garbageCollected;

        try
        {
            GarbageCollectedNotifier.GarbageCollected += OnGc;

            var objects = new object[] { new(), new(), new(), new(), new() };

            var objBigSpan = new BigSpan<object>(objects);

            GC.Collect(2, GCCollectionMode.Forced, true, true);
            objBigSpan.AsPinnedEnumerable(e => {
                    for (var i = 0; i < 10; ++i)
                        GC.Collect(0, GCCollectionMode.Forced, true, true);
                    foreach (var (o, i) in e.Zip(Enumerable.Range(0, 5)))
                    {
                        for (var j = 0; j < 10; ++j)
                            GC.Collect(0, GCCollectionMode.Forced, true, true);

                        Assert.IsNotNull(o);

                        Assert.AreEqual(objects[i], o);
                    }
                }
            );

            Assert.Greater(garbageCollected, 0);
        }
        finally
        {
            GarbageCollectedNotifier.GarbageCollected -= OnGc;
        }
    }

#if DEBUG
    [Ignore("Garbage collector will hold a reference to items expected to be collected under Debug mode.")]
#endif
    [Test]
    public static void ObjPinnedEnumForEachTest2()
    {
        var garbageCollected = 0uL;

        void OnGc() => ++garbageCollected;

        BigSpan<object> CreateFiveObjects() => (BigSpan<object>)new object[] { new(), new(), new(), new(), new() };

        try
        {
            GarbageCollectedNotifier.GarbageCollected += OnGc;

            var objBigSpan = CreateFiveObjects();

            GC.Collect(2, GCCollectionMode.Forced, true, true);
            objBigSpan.AsPinnedEnumerable(e => {
                    for (var i = 0; i < 10; ++i)
                        GC.Collect(0, GCCollectionMode.Forced, true, true);
                    foreach (var (o, i) in e.Zip(Enumerable.Range(0, 5)))
                    {
                        for (var j = 0; j < 10; ++j)
                            GC.Collect(0, GCCollectionMode.Forced, true, true);

                        Assert.IsNotNull(o);
                    }
                }
            );

            Assert.Greater(garbageCollected, 0);
        }
        finally
        {
            GarbageCollectedNotifier.GarbageCollected -= OnGc;
        }
    }

    [Test]
    public static void ObjPinnedEnumAssertTest()
    {
        var objects = new object[] { new(), new(), new(), new(), new() };

        var objBigSpan = new BigSpan<object>(objects);

        BigSpanAssert.AllItemsAreNotNull(objBigSpan);
    }
}
