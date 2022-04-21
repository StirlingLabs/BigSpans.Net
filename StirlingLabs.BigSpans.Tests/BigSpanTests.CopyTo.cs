using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using StirlingLabs.Utilities;
using StirlingLabs.Utilities.Assertions;

namespace StirlingLabs.BigSpans.Tests
{
    public static partial class BigSpanTests
    {
        [Test]
        public static void TryCopyTo()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101 };

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)dst);
            Assert.True(success);
            Assert.AreEqual(src, dst);
        }

        [Test]
        public static void TryCopyToSingle()
        {
            int[] src = { 1 };
            int[] dst = { 99 };

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)dst);
            Assert.True(success);
            Assert.AreEqual(src, dst);
        }

        [Test]
        public static void TryCopyToArraySegmentImplicit()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 5, 99, 100, 101, 10 };
            var segment = new ArraySegment<int>(dst, 1, 3);

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)segment);
            Assert.True(success);
            Assert.AreEqual(src, segment);
        }

        [Test]
        public static void TryCopyToEmpty()
        {
            int[] src = { };
            int[] dst = { 99, 100, 101 };

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)dst);
            Assert.True(success);
            int[] expected = { 99, 100, 101 };
            Assert.AreEqual(expected, dst);
        }

        [Test]
        public static void TryCopyToLonger()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100, 101, 102 };

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)dst);
            Assert.True(success);
            int[] expected = { 1, 2, 3, 102 };
            Assert.AreEqual(expected, dst);
        }

        [Test]
        public static void TryCopyToLonger2()
        {
            byte[] src = new byte[9];
            byte[] dst = new byte[131];

            for (var i = 1; i <= 9; ++i)
                src[i - 1] = (byte)i;

            var srcSpan = new Span<byte>(src);
            srcSpan.CopyTo((BigSpan<byte>)dst);
            byte[] expected = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            Assert.AreEqual(expected, dst.Take(10));
        }

        [Test]
        public static void TryCopyToShorter()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100 };

            var srcSpan = new BigSpan<int>(src);
            var success = srcSpan.TryCopyTo((BigSpan<int>)dst);
            Assert.False(success);
            int[] expected = { 99, 100 };
            Assert.AreEqual(expected, dst); // TryCopyTo() checks for sufficient space before doing any copying.
        }

        [Test]
        public static void CopyToShorter()
        {
            int[] src = { 1, 2, 3 };
            int[] dst = { 99, 100 };

            var srcSpan = new BigSpan<int>(src);
            BigSpanAssert<int>.Throws<ArgumentException>(srcSpan, _srcSpan => _srcSpan.CopyTo((BigSpan<int>)dst));
            int[] expected = { 99, 100 };
            Assert.AreEqual(expected, dst); // CopyTo() checks for sufficient space before doing any copying.
        }

        [Test]
        public static void Overlapping1()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97 };

            var src = new BigSpan<int>(a, 1, 6);
            var dst = new BigSpan<int>(a, 2, 6);
            src.CopyTo(dst);

            int[] expected = { 90, 91, 91, 92, 93, 94, 95, 96 };
            Assert.AreEqual(expected, a);
        }

        [Test]
        public static void Overlapping2()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97 };

            var src = new BigSpan<int>(a, 2, 6);
            var dst = new BigSpan<int>(a, 1, 6);
            src.CopyTo(dst);

            int[] expected = { 90, 92, 93, 94, 95, 96, 97, 97 };
            Assert.AreEqual(expected, a);
        }

        [Test]
        public static void CopyToArray()
        {
            int[] src = { 1, 2, 3 };
            var dst = (BigSpan<int>)new int[3] { 99, 100, 101 };

            src.CopyTo(dst);
            Assert.AreEqual(src, dst.ToArray());
        }

        [Test]
        public static void CopyToSingleArray()
        {
            int[] src = { 1 };
            var dst = (BigSpan<int>)new int[1] { 99 };

            src.CopyTo(dst);
            Assert.AreEqual(src, dst.ToArray());
        }

        [Test]
        public static void CopyToEmptyArray()
        {
            int[] src = { };
            var dst = (BigSpan<int>)new int[3] { 99, 100, 101 };

            src.CopyTo(dst);
            int[] expected = { 99, 100, 101 };
            Assert.AreEqual(expected, dst.ToArray());

            var dstEmpty = (BigSpan<int>)new int[0] { };

            src.CopyTo(dstEmpty);
            int[] expectedEmpty = { };
            Assert.AreEqual(expectedEmpty, dstEmpty.ToArray());
        }

        [Test]
        public static void CopyToLongerArray()
        {
            int[] src = { 1, 2, 3 };
            var dst = (BigSpan<int>)new int[4] { 99, 100, 101, 102 };

            src.CopyTo(dst);
            int[] expected = { 1, 2, 3, 102 };
            Assert.AreEqual(expected, dst.ToArray());
        }

        [Test]
        public static void CopyToShorterArray()
        {
            var src = (BigSpan<int>)new[] { 1, 2, 3 };
            var dst = new int[2] { 99, 100 };

            BigSpanAssert<int>.Throws<ArgumentException>(src, _src => _src.CopyTo((BigSpan<int>)dst));
            int[] expected = { 99, 100 };
            Assert.AreEqual(expected, dst); // CopyTo() checks for sufficient space before doing any copying.
        }

        [Test]
        public static void CopyToCovariantArray()
        {
            var src = new[] { "Hello" };
            var dst = (BigSpan<object>)new object[] { "world" };

            src.CopyTo(dst);
            Assert.AreEqual("Hello", dst[0u]);
        }

        [Test]
        public static unsafe void FillWithRandomDataCoverage()
        {
            nuint bufferSize = 256;

            var allocated = false;
            IntPtr memBlock = default;

            try
            {
                Assert.True(allocated = UnmanagedMemory.Allocate(bufferSize, out memBlock));

                var memoryFirst = (byte*)memBlock.ToPointer();
                var spanFirst = new BigSpan<byte>(memoryFirst, bufferSize);

                spanFirst.AsSmallSlices(RandomNumberGenerator.Fill);

                // take a sample and ensure all of the sampled bytes are non-zero
                var nonZeroBytes = 0;

                for (nuint i = 0; i < bufferSize; ++i)
                    if (spanFirst[i] != 0)
                        nonZeroBytes++;

                Assert.NotZero(nonZeroBytes);
            }
            finally
            {
                if (allocated)
                    UnmanagedMemory.Free(ref memBlock);
            }
        }

        [Test]
        public static unsafe void FillWithRandomNonZeroData()
        {
            nuint bufferSize = 256;

            var allocated = false;
            IntPtr memBlock = default;

            try
            {
                Assert.True(allocated = UnmanagedMemory.Allocate(bufferSize, out memBlock));

                var memoryFirst = (byte*)memBlock.ToPointer();
                var spanFirst = new BigSpan<byte>(memoryFirst, bufferSize);

                using var rng = RandomNumberGenerator.Create();

                spanFirst.AsSmallSlices(rng.GetNonZeroBytes);

                for (nuint i = 0; i < bufferSize; ++i)
                    Assert.NotZero(spanFirst[i]);
            }
            finally
            {
                if (allocated)
                    UnmanagedMemory.Free(ref memBlock);
            }
        }

        [Theory]
        public static unsafe void CopyToSmallSizeTest([Values(1uL, 64uL, 384uL, 1024uL)] ulong longBufferSize)
        {
            var bufferSize = (nuint)longBufferSize;

            var allocatedFirst = false;
            var allocatedSecond = false;
            IntPtr memBlockFirst = default;
            IntPtr memBlockSecond = default;

            try
            {
                Assert.True(allocatedFirst = UnmanagedMemory.Allocate(bufferSize, out memBlockFirst));
                Assert.True(allocatedSecond = UnmanagedMemory.Allocate(bufferSize, out memBlockSecond));

                var memoryFirst = (byte*)memBlockFirst.ToPointer();
                var spanFirst = new BigSpan<byte>(memoryFirst, bufferSize);

                var memorySecond = (byte*)memBlockSecond.ToPointer();
                var spanSecond = new BigSpan<byte>(memorySecond, bufferSize);

                using var rng = RandomNumberGenerator.Create();

                spanFirst.AsSmallSlices(rng.GetNonZeroBytes);

                // take a sample and ensure all of the sampled bytes are non-zero
                for (nuint i = 0; i < Math.Min(bufferSize - 1, 63); ++i)
                    Assert.NotZero(spanFirst[i]);
                Assert.NotZero(spanFirst[^1]);

                spanFirst.CopyTo(spanSecond);

                Assert.Zero(spanFirst.CompareMemory(spanSecond));
            }
            finally
            {
                if (allocatedFirst)
                    UnmanagedMemory.Free(ref memBlockFirst);
                if (allocatedSecond)
                    UnmanagedMemory.Free(ref memBlockSecond);
            }
        }

        [Theory]
        [Explicit]
        public static unsafe void CopyToLargeSizeTest(
            [Values(uint.MaxValue / 32uL, 256uL + int.MaxValue, 256uL + uint.MaxValue)] ulong longBufferSize)
        {
            var bufferSize = (nuint)longBufferSize;

            if (bufferSize != longBufferSize)
                Assert.Inconclusive($"Can't address 0x{longBufferSize:X} within a nuint on this platform.");

            var allocatedFirst = false;
            var allocatedSecond = false;
            IntPtr memBlockFirst = default;
            IntPtr memBlockSecond = default;

            try
            {
                Assert.True(allocatedFirst = UnmanagedMemory.Allocate(bufferSize, out memBlockFirst));
                var memoryFirst = (byte*)memBlockFirst.ToPointer();
                Assert.NotZero((nint)memoryFirst);
                var spanFirst = new BigSpan<byte>(memoryFirst, bufferSize);

                Assert.True(allocatedSecond = UnmanagedMemory.Allocate(bufferSize, out memBlockSecond));
                var memorySecond = (byte*)memBlockSecond.ToPointer();
                Assert.NotZero((nint)memorySecond);
                var spanSecond = new BigSpan<byte>(memorySecond, bufferSize);

                spanFirst.AsSmallSlices(RandomNumberGenerator.Fill);

                spanFirst.CopyTo(spanSecond);

                Assert.Zero(spanFirst.CompareMemory(spanSecond));
            }
            finally
            {
                if (allocatedFirst)
                    UnmanagedMemory.Free(ref memBlockFirst);
                if (allocatedSecond)
                    UnmanagedMemory.Free(ref memBlockSecond);
            }
        }

        [Test]
        [SuppressMessage("Security", "CA5394", Justification = "Test case with no security requirement.")]
        public static void CopyToVaryingSizes()
        {
            const int MaxLength = 2048;

            var rng = new Random();
            var inputArray = new byte[MaxLength];
            var inputSpan = (BigSpan<byte>)inputArray;
            BigSpan<byte> outputSpan = (BigSpan<byte>)new byte[MaxLength];
            BigSpan<byte> allZerosSpan = (BigSpan<byte>)new byte[MaxLength];

            // Test all inputs from size 0 .. MaxLength (inclusive) to make sure we don't have
            // gaps in our Memmove logic.
            for (var i = 0u; i <= MaxLength; i++)
            {
                // Arrange

                rng.NextBytes(inputArray);
                outputSpan.Clear();

                // Act

                inputSpan.Slice(0, i).CopyTo(outputSpan);

                // Assert

                Assert.True(inputSpan.Slice(0, i).SequenceEqual(outputSpan.Slice(0, i))); // src successfully copied to dst
                Assert.True(outputSpan.Slice(i).SequenceEqual(allZerosSpan.Slice(i))); // no other part of dst was overwritten
            }
        }
    }
}
