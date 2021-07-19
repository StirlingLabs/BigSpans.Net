using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using StirlingLabs.Utilities;
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
    internal unsafe class UnsafeEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        /// <summary>A byref or a native ptr.</summary>
        internal readonly void* Pointer;

        /// <summary>The number of elements this Span contains.</summary>
        /// <remarks>Due to _pointer being a hack, this must written to immediately after.</remarks>
        internal readonly nuint Length;

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
            if (_offset >= Length)
                return false;
            _offset++;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
            => _offset = ~default(nuint);

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
        void IDisposable.Dispose() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator()
            => this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetPinnableReference()
            => ref Current;
    }
}
