using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using StirlingLabs.Utilities;
using StirlingLabs.Utilities.Magic;

namespace StirlingLabs.BigSpans.Tests
{
    public partial class BigSpanTests
    {
        [Test]
        public static void SliceInt()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new BigSpan<int>(a).Slice(6);
            Assert.AreEqual((nuint)4, span.Length);
            Assert.True(Unsafe.AreSame(ref a[6], ref span.GetReference()));
        }

        [Test]
        public static void SliceIntPastEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new BigSpan<int>(a).Slice((nuint)a.LongLength);
            Assert.AreEqual((nuint)0, span.Length);
            Assert.True(Unsafe.AreSame(ref a[^1], ref Unsafe.Subtract(ref span.GetReference(), 1)));
        }

        [Test]
        public static void SliceIntInt()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new BigSpan<int>(a).Slice(3, (nuint)5);
            Assert.AreEqual((nuint)5, span.Length);
            Assert.True(Unsafe.AreSame(ref a[3], ref span.GetReference()));
        }

        [Test]
        public static void SliceIntIntUpToEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new BigSpan<int>(a).Slice(4, (nuint)6);
            Assert.AreEqual((nuint)6, span.Length);
            Assert.True(Unsafe.AreSame(ref a[4], ref span.GetReference()));
        }

        [Test]
        public static void SliceIntIntPastEnd()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            var span = new BigSpan<int>(a).Slice((nuint)a.LongLength, (nuint)0);
            Assert.AreEqual((nuint)0, span.Length);
            Assert.True(Unsafe.AreSame(ref a[^1], ref Unsafe.Subtract<int>(ref span.GetReference(), 1)));
        }

        [Test]
        public static void SliceIntRangeChecks()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
            //Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(-1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice((nuint)(a.LongLength + 1u)).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(-1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(0, -1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(0, a.Length + 1).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(2, a.Length + 1 - 2).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(a.Length + 1, 0).DontBox());
            Assert.Throws<ArgumentOutOfRangeException>(() => new BigSpan<int>(a).Slice(a.Length, 1).DontBox());
        }
    }
}
