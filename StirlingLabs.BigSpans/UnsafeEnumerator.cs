using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using StirlingLabs.Utilities.Magic;

namespace StirlingLabs.Utilities
{
    [PublicAPI]
    internal static class UnsafeEnumerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeEnumerator<T> Create<T>(ref T pointer, nuint length)
            => new(ref pointer, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeEnumerator<T> Create<T>(Span<T> s)
            => new(s);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeEnumerator<T> Create<T>(ReadOnlySpan<T> s)
            => new(s);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeEnumerator<T> Create<T>(BigSpan<T> s)
            => new(s);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeEnumerator<T> Create<T>(ReadOnlyBigSpan<T> s)
            => new(s);
    }

    [PublicAPI]
    internal unsafe class UnsafeEnumerator<T> : IEnumerator<T>, IList<T>, IReadOnlyList<T>
    {
        /// <summary>A byref or a native ptr.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly void* Pointer;

        /// <summary>The number of elements this Span contains.</summary>
        /// <remarks>Due to _pointer being a hack, this must written to immediately after.</remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly nuint Length;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private nuint _offset;

        private UnsafeEnumerator() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnsafeEnumerator(ref T pointer, nuint length)
        {
            Pointer = Unsafe.AsPointer(ref pointer);
            Length = length;
            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnsafeEnumerator(Span<T> s)
        {
            Pointer = Unsafe.AsPointer(ref s.GetPinnableReference());
            Length = (nuint)s.Length;
            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnsafeEnumerator(ReadOnlySpan<T> s)
        {
            Pointer = Unsafe.AsPointer(ref Unsafe.AsRef(s.GetPinnableReference()));
            Length = (nuint)s.Length;
            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnsafeEnumerator(BigSpan<T> s)
        {
            Pointer = Unsafe.AsPointer(ref s.GetReference());
            Length = s.Length;
            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnsafeEnumerator(ReadOnlyBigSpan<T> s)
        {
            Pointer = Unsafe.AsPointer(ref s.GetReference());
            Length = s.Length;
            Reset();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _offset++;
            if (_offset < Length)
                return true;

            _offset--;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
            => _offset = ~default(nuint);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef<T>(Pointer), (nint)_offset);
        }

        T IEnumerator<T>.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current;
        }

        object? IEnumerator.Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IDisposable.Dispose()
        {
            /* no need */
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator()
            => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
            => ref Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex)
            => new BigSpan<byte>((byte*)Pointer + _offset * (uint)Unsafe.SizeOf<T>(), Length - _offset).CastAs<T>()
                .CopyTo(new BigSpan<T>(array).Slice((nuint)arrayIndex));

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)Length;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new BigSpan<byte>(Pointer, Length).CastAs<T>()[(nuint)index];
            set => throw new NotSupportedException();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
            => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
            => throw new NotSupportedException();
    }
}
