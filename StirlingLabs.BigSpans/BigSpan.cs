using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using InlineIL;
using JetBrains.Annotations;
using StirlingLabs.Utilities.Magic;
using static InlineIL.IL;
using static InlineIL.IL.Emit;

// @formatter:off
#if NETSTANDARD
using StirlingLabs.Utilities.Compatibility;
#endif
// @formatter:on

#pragma warning disable 0809 //warning CS0809: Obsolete member 'Span<T>.Equals(object)' overrides non-obsolete member 'object.Equals(object)'

namespace StirlingLabs.Utilities
{
    [PublicAPI]
    public static class BigSpan
    {
        /// <summary>
        /// Creates a new read-only span over the target managed buffer.
        /// </summary>
        /// <param name="ptr">A managed reference to memory.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <typeparamref name="T"/> is reference type or contains pointers and hence cannot be stored in unmanaged memory.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1045", Justification = "Nope")]
        public static BigSpan<T> Create<T>(ref T ptr, nuint length)
        {
            if (BigSpanHelpers.IsReferenceOrContainsReferences<T>())
                throw new NotSupportedException("Invalid type with pointers.");

            return new(ref ptr, length);
        }

        /// <summary>
        /// Creates a new span over the entirety of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        [SuppressMessage("Microsoft.Design", "CA1045", Justification = "Nope")]
        public static BigSpan<T> Create<T>(T[] array)
        {
            if (BigSpanHelpers.IsReferenceOrContainsReferences<T>())
                throw new NotSupportedException("Invalid type with pointers.");

            return new(array);
        }

        /// <summary>
        /// Creates a new span over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the span.</param>
        /// <param name="length">The number of items in the span.</param>
        /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;Length).
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1045", Justification = "Nope")]
        public static BigSpan<T> Create<T>(T[] array, nuint start, nuint length)
        {
            if (BigSpanHelpers.IsReferenceOrContainsReferences<T>())
                throw new NotSupportedException("Invalid type with pointers.");

            return new(array, start, length);
        }

        [SuppressMessage("Reliability", "CA2000", Justification = "Not actually disposable")]
        public static void AsPinnedEnumerables<T>(ReadOnlyBigSpan<T> a, Action<IEnumerable<T>> f)
        {

            DeclareLocals(new LocalVar(typeof(T).MakeByRefType()).Pinned());

            if (f is null) throw new ArgumentNullException(nameof(f));

            PushInRef(a.GetPinnableReference());
            Stloc_0();

            var e = UnsafeEnumerator.Create(a);
            f(e);
        }

        [SuppressMessage("Reliability", "CA2000", Justification = "Not actually disposable")]
        public static void AsPinnedEnumerables<T1, T2>(ReadOnlyBigSpan<T1> a, ReadOnlyBigSpan<T2> b, Action<IEnumerable<T1>, IEnumerable<T2>> f)
        {
            DeclareLocals(
                new LocalVar(typeof(T1).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T2).MakeByRefType()).Pinned()
            );

            if (f is null) throw new ArgumentNullException(nameof(f));

            PushInRef(a.GetPinnableReference());
            Stloc_0();
            PushInRef(b.GetPinnableReference());
            Stloc_1();

            var e1 = new UnsafeEnumerator<T1>(a);
            var e2 = new UnsafeEnumerator<T2>(b);
            f(e1, e2);
        }

        [SuppressMessage("Reliability", "CA2000", Justification = "Not actually disposable")]
        public static void AsPinnedEnumerables<T1, T2, T3>(ReadOnlyBigSpan<T1> a, ReadOnlyBigSpan<T2> b, ReadOnlyBigSpan<T3> c,
            Action<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> f)
        {
            DeclareLocals(
                new LocalVar(typeof(T1).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T2).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T3).MakeByRefType()).Pinned()
            );

            if (f is null) throw new ArgumentNullException(nameof(f));

            PushInRef(a.GetPinnableReference());
            Stloc_0();
            PushInRef(b.GetPinnableReference());
            Stloc_1();
            PushInRef(c.GetPinnableReference());
            Stloc_2();

            var e1 = new UnsafeEnumerator<T1>(a);
            var e2 = new UnsafeEnumerator<T2>(b);
            var e3 = new UnsafeEnumerator<T3>(c);
            f(e1, e2, e3);
        }

        [SuppressMessage("Reliability", "CA2000", Justification = "Not actually disposable")]
        public static void AsPinnedEnumerables<T1, T2, T3, T4>(ReadOnlyBigSpan<T1> a, ReadOnlyBigSpan<T2> b, ReadOnlyBigSpan<T3> c,
            ReadOnlyBigSpan<T4> d, Action<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> f)
        {
            DeclareLocals(
                new LocalVar(typeof(T1).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T2).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T3).MakeByRefType()).Pinned(),
                new LocalVar(typeof(T4).MakeByRefType()).Pinned()
            );

            if (f is null) throw new ArgumentNullException(nameof(f));

            PushInRef(a.GetPinnableReference());
            Stloc_0();
            PushInRef(b.GetPinnableReference());
            Stloc_1();
            PushInRef(c.GetPinnableReference());
            Stloc_2();
            PushInRef(d.GetPinnableReference());
            Stloc_3();

            var e1 = new UnsafeEnumerator<T1>(a);
            var e2 = new UnsafeEnumerator<T2>(b);
            var e3 = new UnsafeEnumerator<T3>(c);
            var e4 = new UnsafeEnumerator<T4>(d);
            f(e1, e2, e3, e4);
        }
    }

    /// <summary>
    /// Span represents a contiguous region of arbitrary memory. Unlike arrays, it can point to either managed
    /// or native memory, or to memory allocated on the stack. It is type- and memory-safe.
    /// </summary>
    [PublicAPI]
    [NonVersionable]
    [DebuggerDisplay("{ToString(),raw}")]
    [DebuggerTypeProxy(typeof(BigSpanDebugView<>))]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct BigSpan<T>
    {
        /// <summary>A byref or a native ptr.</summary>
        internal readonly ByReference<T> _pointer;

        /// <summary>The number of elements this Span contains.</summary>
        /// <remarks>Due to _pointer being a hack, this must written to immediately after.</remarks>
        // ReSharper disable once InconsistentNaming // when the hack is no longer needed, move back to a field
        internal readonly ref nuint _length => ref _pointer.Length;

        /// <summary>
        /// Creates a new span over the entirety of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(T[]? array)
        {
            if (array == null)
            {
                this = default;
                return; // returns default
            }

            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
                throw new ArrayTypeMismatchException();

#if NETSTANDARD
            _pointer = new(ref array[0]);
#else
            _pointer = new(ref MemoryMarshal.GetArrayDataReference(array));
#endif
            _length = BigSpanHelpers.Is64Bit ? (nuint)array.LongLength : (nuint)array.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpan(T[]? array, [UsedImplicitly] bool _)
        {
            if (array == null)
            {
                this = default;
                return; // returns default
            }

            /*
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
                throw new ArrayTypeMismatchException();
            */

#if NETSTANDARD
            _pointer = new(ref array[0]);
#else
            _pointer = new(ref MemoryMarshal.GetArrayDataReference(array));
#endif
            _length = BigSpanHelpers.Is64Bit ? (nuint)array.LongLength : (nuint)array.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpan(ByReference<T> byRef)
            => _pointer = byRef;

        /// <summary>
        /// Creates a new span over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the span.</param>
        /// <param name="length">The number of items in the span.</param>
        /// <remarks>Returns default when <paramref name="array"/> is null.</remarks>
        /// <exception cref="System.ArrayTypeMismatchException">Thrown when <paramref name="array"/> is covariant and array's type is not exactly T[].</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in the range (&lt;0 or &gt;Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(T[]? array, nuint start, nuint length)
        {
            if (array == null)
            {
                if (start != 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                if (length != 0)
                    throw new ArgumentOutOfRangeException(nameof(length));
                this = default;
                return; // returns default
            }

            /*
             if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
                throw new ArrayTypeMismatchException();
            */

            // See comment in Span<T>.Slice for how this works.
            if (start + length > (nuint)array.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

#if NETSTANDARD
            _pointer = new(ref Unsafe.Add(ref array[0],
                (nint)(uint)start /* force zero-extension */));
#else
            _pointer = new(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array),
                (nint)(uint)start /* force zero-extension */));
#endif
            _length = length;
        }

        /// <summary>
        /// Creates a new span over the target unmanaged buffer.  Clearly this
        /// is quite dangerous, because we are creating arbitrarily typed T's
        /// out of a void*-typed block of memory.  And the length is not checked.
        /// But if this creation is correct, then all subsequent uses are correct.
        /// </summary>
        /// <param name="pointer">An unmanaged pointer to memory.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <typeparamref name="T"/> is reference type or contains pointers and hence cannot be stored in unmanaged memory.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="length"/> is negative.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe BigSpan(void* pointer, nuint length)
        {
            if (BigSpanHelpers.IsReferenceOrContainsReferences<T>())
                throw new NotSupportedException("Invalid type with pointers.");

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            _pointer = new(ref Unsafe.As<byte, T>(ref *(byte*)pointer));
            _length = length;
        }

        // Constructor for internal use only.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpan(ref T ptr, nuint length)
        {
            Debug.Assert(length >= 0);

            _pointer = new(ref ptr);
            _length = length;
        }

        /// <summary>
        /// Returns a reference to specified element of the Span.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Thrown when index less than 0 or index greater than or equal to Length
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1043", Justification = "Intentional")]
        [SuppressMessage("Microsoft.Design", "CA1065", Justification = "Patterned after System.Span")]
        public ref T this[nuint index]
        {
            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [NonVersionable]
            get {
                if (index >= _length)
                    throw new IndexOutOfRangeException();

                return ref Unsafe.Add(ref _pointer.Value, (nint)index);
            }
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Returns a reference to specified element of the Span.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Thrown when index less than 0 or index greater than or equal to Length
        /// </exception>
        public ref T this[Index index]
        {
            get {
                var actualIndex = (nuint)index.Value;
                if (index.IsFromEnd) actualIndex = _length - actualIndex;
                return ref this[actualIndex];
            }
        }

        public BigSpan<T> this[Range range]
        {
            get {
                var start = (nuint)range.Start.Value;
                if (range.Start.IsFromEnd) start = _length - start;
                var end = (nuint)range.End.Value;
                if (range.Start.IsFromEnd) end = _length - end;
                var length = end - start;
                return new(ref this[start], length);
            }
        }
#endif

        /// <summary>
        /// The number of items in the span.
        /// </summary>
        public nuint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [NonVersionable]
            get => _length;
        }

        /// <summary>
        /// The number of items in the span.
        /// </summary>
        public ulong LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [NonVersionable]
            get => _length;
        }

        /// <summary>
        /// Returns true if Length is 0.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [NonVersionable]
            get => default(nuint) >= _length; // Workaround for https://github.com/dotnet/runtime/issues/10950
        }

        /// <summary>
        /// Returns false if left and right point at the same memory and have the same length.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BigSpan<T> left, BigSpan<T> right) => !(left == right);

        /// <summary>
        /// This method is not supported as spans cannot be boxed. To compare two spans, use operator==.
        /// <exception cref="System.NotSupportedException">
        /// Always thrown by this method.
        /// </exception>
        /// </summary>
        [Obsolete("Equals() on Span will always throw an exception. Use == instead.", true)]
        [SuppressMessage("Microsoft.Design", "CA1065", Justification = "Patterned after System.Span")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) =>
            throw new NotSupportedException();

        /// <summary>
        /// This method is not supported as spans cannot be boxed.
        /// <exception cref="System.NotSupportedException">
        /// Always thrown by this method.
        /// </exception>
        /// </summary>
        [Obsolete("GetHashCode() on Span will always throw an exception.", true)]
        [SuppressMessage("Microsoft.Design", "CA1065", Justification = "Patterned after System.Span")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
            throw new NotSupportedException();

        /// <summary>
        /// Defines an explicit conversion of an array to a <see cref="BigSpan{T}"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator BigSpan<T>(T[]? array) => new(array);

        /// <summary>
        /// Defines an explicit conversion of an array to a <see cref="BigSpan{T}"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyBigSpan<T> From(T[]? array) => new(array);

        /// <summary>
        /// Defines an explicit conversion of a <see cref="ArraySegment{T}"/> to a <see cref="BigSpan{T}"/>
        /// </summary>
        [SuppressMessage("Usage", "CA2225", Justification = "See From(ArraySegment<T>)")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator BigSpan<T>(ArraySegment<T> segment) =>
            new(segment.Array, (nuint)segment.Offset, (nuint)segment.Count);

        /// <summary>
        /// Defines an explicit conversion of a <see cref="ArraySegment{T}"/> to a <see cref="BigSpan{T}"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyBigSpan<T> From(ArraySegment<T> segment) => (ReadOnlyBigSpan<T>)segment;

        /// <summary>
        /// Returns an empty <see cref="BigSpan{T}"/>
        /// </summary>
        public static BigSpan<T> Empty => default;

        /// <summary>Gets an enumerator for this span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpanEnumerator<T> GetEnumerator() => new(this);

        /// <summary>
        /// Returns a reference to the 0th element of the Span. If the Span is empty, returns null reference.
        /// It can be used for pinning and is required to support the use of span within a fixed statement.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference()
        {
            // Ensure that the native code has just one forward branch that is predicted-not-taken.
            ref var ret = ref Unsafe.NullRef<T>();
            if (_length != 0) ret = ref _pointer.Value;
            return ref ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe void* GetUnsafePointer() => Unsafe.AsPointer(ref GetPinnableReference());

        /// <summary>
        /// Clears the contents of this span.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Clear()
        {
            if (BigSpanHelpers.IsReferenceOrContainsReferences<T>())
                BigSpanHelpers.ClearWithReferences(ref Unsafe.As<T, nint>(ref _pointer.Value),
                    (uint)_length * (nuint)(Unsafe.SizeOf<T>() / sizeof(nuint)));
            else
                BigSpanHelpers.ClearWithoutReferences(ref Unsafe.As<T, byte>(ref _pointer.Value),
                    (uint)_length * (nuint)Unsafe.SizeOf<T>());
        }

        /// <summary>
        /// Fills the contents of this span with the given value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fill(T value)
        {
            if (Unsafe.SizeOf<T>() == 1)
            {
                // Mono runtime's implementation of initblk performs a null check on the address.
                // We'll perform a length check here to avoid passing a null address in the empty span case.
                if (_length != 0)
                    // Special-case single-byte types like byte / sbyte / bool.
                    // The runtime eventually calls memset, which can efficiently support large buffers.
                    // We don't need to check IsReferenceOrContainsReferences because no references
                    // can ever be stored in types this small.
                    Unsafe.InitBlockUnaligned(ref Unsafe.As<T, byte>(ref _pointer.Value), Unsafe.As<T, byte>(ref value),
                        (uint)_length);
            }
            else
                // Call our optimized workhorse method for all other types.
                BigSpanHelpers.Fill(ref _pointer.Value, (uint)_length, value);
        }

        /// <summary>
        /// Copies the contents of this span into destination span. If the source
        /// and destinations overlap, this method behaves as if the original values in
        /// a temporary location before the destination is overwritten.
        /// </summary>
        /// <param name="destination">The span to copy items into.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the destination Span is shorter than the source Span.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void CopyTo(BigSpan<T> destination)
        {
            // Using "if (!TryCopyTo(...))" results in two branches: one for the length
            // check, and one for the result of TryCopyTo. Since these checks are equivalent,
            // we can optimize by performing the check once ourselves then calling Memmove directly.

            var sizeOfT = (nuint)Unsafe.SizeOf<T>();
            var srcLen = _length * sizeOfT;
            var dstLen = destination._length * sizeOfT;
            if (srcLen > dstLen)
                throw new ArgumentException("Too short.", nameof(destination));

            var length = srcLen < dstLen ? srcLen : dstLen;
            if (length == default) return;

            DeclareLocals(
                new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                    .Pinned(),
                new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                    .Pinned()
            );

            Push(ref destination.GetPinnableReference()!);
            Stloc("rDst");
            Push(ref GetPinnableReference()!);
            Stloc("rSrc");
            Ldloc("rDst");
            Pop(out var pDst);
            Ldloc("rSrc");
            Pop(out var pSrc);
            if (pDst == default) throw new ArgumentNullException(nameof(destination));
            if (pSrc == default) throw new NullReferenceException();
            if (length <= 0)
                return;
            BigSpanHelpers.Copy(pDst, pSrc, length);
        }

        /// <summary>
        /// Copies the contents of this span into destination span. If the source
        /// and destinations overlap, this method behaves as if the original values in
        /// a temporary location before the destination is overwritten.
        /// </summary>
        /// <param name="destination">The span to copy items into.</param>
        /// <returns>If the destination span is shorter than the source span, this method
        /// return false and no data is written to the destination.</returns>
        public unsafe bool TryCopyTo(BigSpan<T> destination)
        {
            var sizeOfT = (nuint)Unsafe.SizeOf<T>();
            var srcLen = _length * sizeOfT;
            var dstLen = destination._length * sizeOfT;
            if (srcLen > dstLen)
                return false;

            var length = srcLen < dstLen ? srcLen : dstLen;
            if (length == default) return true;

            DeclareLocals(
                new LocalVar("rDst", TypeRef.Type<T>().MakeByRefType())
                    .Pinned(),
                new LocalVar("rSrc", TypeRef.Type<T>().MakeByRefType())
                    .Pinned()
            );

            Push(ref destination.GetPinnableReference()!);
            Stloc("rDst");
            Push(ref GetPinnableReference()!);
            Stloc("rSrc");
            Ldloc("rDst");
            Pop(out var pDst);
            Ldloc("rSrc");
            Pop(out var pSrc);
            if (pDst == default) throw new ArgumentNullException(nameof(destination));
            if (pSrc == default) throw new NullReferenceException();
            if (length <= 0)
                return true;
            BigSpanHelpers.Copy(pDst, pSrc, length);
            return true;
        }

        /// <summary>
        /// Returns true if left and right point at the same memory and have the same length.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BigSpan<T> left, BigSpan<T> right) =>
            left._length == right._length &&
            Unsafe.AreSame(ref left._pointer.Value, ref right._pointer.Value);

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Span{T}"/> to a <see cref="BigSpan{T}"/>
        /// </summary>
        [SuppressMessage("Usage", "CA2225", Justification = "Pollution of scope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigSpan<T>(Span<T> span)
            => new(new ByReference<T>(span));

        /// <summary>
        /// Defines an explicit conversion of a <see cref="BigSpan{T}"/> to a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator ReadOnlySpan<T>(BigSpan<T> bigSpan) =>
            bigSpan._length <= int.MaxValue
                ? new ReadOnlySpan<T>(bigSpan.GetUnsafePointer(), (int)bigSpan._length)
                : throw new NotSupportedException(
                    $"Not possible to create ReadOnlySpans longer than {int.MaxValue} (maximum 32-bit integer value)");
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlySpan<T>(BigSpan<T> bigSpan) =>
            bigSpan._length <= int.MaxValue
                ? MemoryMarshal.CreateReadOnlySpan(ref bigSpan._pointer.Value, (int)bigSpan._length)
                : throw new NotSupportedException(
                    $"Not possible to create ReadOnlySpans longer than {int.MaxValue} (maximum 32-bit integer value)");
#endif

        /// <summary>
        /// Defines an explicit conversion of a <see cref="BigSpan{T}"/> to a <see cref="Span{T}"/>
        /// </summary>
#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe explicit operator Span<T>(BigSpan<T> bigSpan) =>
            bigSpan._length <= int.MaxValue
                ? new Span<T>(bigSpan.GetUnsafePointer(), (int)bigSpan._length)
                : throw new NotSupportedException(
                    $"Not possible to create ReadOnlySpans longer than {int.MaxValue} (maximum 32-bit integer value)");
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(BigSpan<T> bigSpan) =>
            bigSpan._length <= int.MaxValue
                ? MemoryMarshal.CreateSpan(ref bigSpan._pointer.Value, (int)bigSpan._length)
                : throw new NotSupportedException(
                    $"Not possible to create ReadOnlySpans longer than {int.MaxValue} (maximum 32-bit integer value)");
#endif

        /// <summary>
        /// Defines an explicit conversion of a <see cref="BigSpan{T}"/> to a <see cref="Span{T}"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> ToSpan() => (Span<T>)this;

        /// <summary>
        /// Defines an explicit conversion of a <see cref="BigSpan{T}"/> to a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> ToReadOnlySpan() => (ReadOnlySpan<T>)this;

        /// <summary>
        /// For <see cref="BigSpan{T}"/>, returns a new instance of string that represents the characters pointed to by the span.
        /// Otherwise, returns a <see cref="string"/> with the name of the type and the number of elements.
        /// </summary>
#if NETSTANDARD2_0
        public override unsafe string ToString()
        {
            if (IfType<T>.Is<char>() && _length <= int.MaxValue)
                return new((char*)GetUnsafePointer(), 0, (int)_length);
            return $"BigSpan<{typeof(T).Name}>[{_length}]";
        }
#else
        public override string ToString()
        {
            if (IfType<T>.Is<char>() && _length <= int.MaxValue)
                return new(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, char>(ref _pointer.Value), (int)_length));
            return $"BigSpan<{typeof(T).Name}>[{_length}]";
        }
#endif

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> index is not in range (&lt;0 or &gt;Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T> Slice(nuint start)
        {
            if ((uint)start > (uint)_length)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new(ref Unsafe.Add(ref _pointer.Value, (nint)(uint)start /* force zero-extension */), _length - start);
        }

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start', of given length
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice (exclusive).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in range (&lt;0 or &gt;Length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T> Slice(nuint start, nuint length)
        {
            // Since start and length are both 32-bit, their sum can be computed across a 64-bit domain
            // without loss of fidelity. The cast to uint before the cast to ulong ensures that the
            // extension from 32- to 64-bit is zero-extending rather than sign-extending. The end result
            // of this is that if either input is negative or if the input sum overflows past Int32.MaxValue,
            // that information is captured correctly in the comparison against the backing _length field.
            // We don't use this same mechanism in a 32-bit process due to the overhead of 64-bit arithmetic.
            if (start + length > _length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new(ref Unsafe.Add(ref _pointer.Value, (nint)(uint)start /* force zero-extension */), length);
        }

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start', of given length
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice (exclusive).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in range (&lt;0 or &gt;Length).
        /// </exception>
#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> Slice(nuint start, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"Length was less than zero. ({length})");
            if (start + (nuint)length > _length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new(GetUnsafePointer(), length);
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(nuint start, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"Length was less than zero. ({length})");
            if (start + (nuint)length > _length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return MemoryMarshal.CreateSpan(ref this[start], length);
        }
#endif

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start', of given length
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice (exclusive).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="start"/> or end index is not in range (&lt;0 or &gt;Length).
        /// </exception>
#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span<T> Slice(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if ((nuint)start + (nuint)length > _length)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new((byte*)GetUnsafePointer() + start * Unsafe.SizeOf<T>(), length);
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if ((nuint)start + (nuint)length > _length)
                throw new ArgumentOutOfRangeException(nameof(length));
            return MemoryMarshal.CreateSpan(ref this[(nuint)start], length);
        }
#endif

        /// <summary>
        /// Copies the contents of this span into a new array.  This heap
        /// allocates, so should generally be avoided, however it is sometimes
        /// necessary to bridge the gap with APIs written in terms of arrays.
        /// </summary>
#pragma warning disable 652 // not assuming pointer can be at most 64-bit here 
        public T[] ToArray()
        {
            if (_length == 0)
                return Array.Empty<T>();

            T[] destination = BigSpanHelpers.Is64Bit
                // not assuming pointer can be at most 64-bit here
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                ? _length > ulong.MaxValue
                    ? throw new NotSupportedException(
                        $"Arrays larger than {ulong.MaxValue} (maximum signed 64-bit integer) are not possible at this time.")
                    : new T[_length]
                : _length switch
                { // 32-bit
                    <= int.MaxValue => new T[(int)_length],
                    <= uint.MaxValue => new T[(uint)_length],
                    // not assuming pointer can be at most 64-bit here
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    _ => _length > ulong.MaxValue
                        ? throw new NotSupportedException(
                            $"Arrays larger than {ulong.MaxValue} (maximum signed 64-bit integer) are not possible at this time.")
                        : new T[_length]
                };
            CopyTo((BigSpan<T>)destination);
            return destination;
        }
#pragma warning restore 652

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<byte> AsBytes()
            => new(ref Unsafe.As<T, byte>(ref _pointer.Value), _length * (nuint)Unsafe.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T2> CastAs<T2>()
            => new(ref Unsafe.As<T, T2>(ref _pointer.Value), _length * (nuint)Unsafe.SizeOf<T>() / (nuint)Unsafe.SizeOf<T2>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe int CompareMemoryInternal(BigSpan<T> other, nuint length)
            => UnmanagedMemory.C_CompareMemory(GetUnsafePointer(), other.GetUnsafePointer(), length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe int CompareMemoryInternal(ReadOnlyBigSpan<T> other, nuint length)
            => UnmanagedMemory.C_CompareMemory(GetUnsafePointer(), other.GetUnsafePointer(), length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareMemory(BigSpan<T> other, nuint length)
        {
            if (length > _length || length > other._length)
                throw new ArgumentOutOfRangeException(nameof(length));
            return CompareMemoryInternal(other, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareMemory(BigSpan<T> other)
        {
            var myLength = _length;
            var otherLength = other._length;

            var lengthComparison = myLength.CompareTo(otherLength);
            if (lengthComparison == 0)
                return CompareMemoryInternal(other, myLength * (nuint)Unsafe.SizeOf<T>());

            var length = (nuint)Math.Min(myLength, otherLength) * (nuint)Unsafe.SizeOf<T>();
            var result = CompareMemoryInternal(other, length);
            return result == 0 ? lengthComparison : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareMemory(ReadOnlyBigSpan<T> other)
        {
            var myLength = _length;
            var otherLength = other._length;

            var lengthComparison = myLength.CompareTo(otherLength);
            if (lengthComparison == 0)
                return CompareMemoryInternal(other, myLength * (nuint)Unsafe.SizeOf<T>());

            var length = (nuint)Math.Min(myLength, otherLength) * (nuint)Unsafe.SizeOf<T>();
            var result = CompareMemoryInternal(other, length);
            return result == 0 ? lengthComparison : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareMemory(Span<T> other)
        {
            var myLength = _length;
            var otherLength = (uint)other.Length;

            var lengthComparison = myLength.CompareTo(otherLength);
            if (lengthComparison == 0)
                return CompareMemoryInternal(other, myLength * (nuint)Unsafe.SizeOf<T>());

            var length = (nuint)Math.Min(myLength, otherLength) * (nuint)Unsafe.SizeOf<T>();
            var result = CompareMemoryInternal(other, length);
            return result == 0 ? lengthComparison : result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareMemory(ReadOnlySpan<T> other)
        {
            var myLength = _length;
            var otherLength = (uint)other.Length;

            var lengthComparison = myLength.CompareTo(otherLength);
            if (lengthComparison == 0)
                return CompareMemoryInternal(other, myLength * (nuint)Unsafe.SizeOf<T>());

            var length = (nuint)Math.Min(myLength, otherLength) * (nuint)Unsafe.SizeOf<T>();
            var result = CompareMemoryInternal(other, length);
            return result == 0 ? lengthComparison : result;
        }
    }
}
