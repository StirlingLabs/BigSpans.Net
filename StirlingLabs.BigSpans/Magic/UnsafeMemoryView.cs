using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities.Magic
{
    // ReSharper disable UseNameofExpression, NotResolvedInText
    [DebuggerDisplay("Pointer = {Pointer,X}, Length = {Length}", Type = "{Type}")]
    // ReSharper restore UseNameofExpression, NotResolvedInText
    internal readonly unsafe struct UnsafeMemoryView<T> : IEnumerable<Ptr<T>>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Type Type => typeof(T);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly void* Pointer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal readonly nuint Length;

        public UnsafeMemoryView(void* pointer, nuint length)
        {
            Pointer = pointer;
            Length = length;
        }

        public UnsafeMemoryView(ReadOnlyBigSpan<T> span)
        {
            Pointer = span.GetUnsafePointer();
            Length = span.Length;
        }

        public IEnumerator<Ptr<T>> GetEnumerator()
        {
            for (var i = (nuint)0; i < Length; ++i)
                yield return this[(nint)i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private Ptr<T> this[nint index] => new((void*)((nuint)Pointer + (nuint)Unsafe.SizeOf<T>() * (nuint)index));
    }
}
