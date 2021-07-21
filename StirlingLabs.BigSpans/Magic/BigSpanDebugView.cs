using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities.Magic
{
    [PublicAPI]
    // ReSharper disable UseNameofExpression, NotResolvedInText
    [DebuggerDisplay("Pointer = {View._pointer,X}, Length = {View._length}", Type = "{Type}")]
    // ReSharper restore UseNameofExpression, NotResolvedInText
    public sealed class BigSpanDebugView<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Type Type => typeof(T);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal UnsafeMemoryView<T> View;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal unsafe void* Pointer => View.Pointer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal nuint Length => View.Length;

        public BigSpanDebugView(BigSpan<T> span)
            : this((ReadOnlyBigSpan<T>)span) { }

        public BigSpanDebugView(ReadOnlyBigSpan<T> span)
            => View = span.Length != 0 ? new(span) : default;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IEnumerable<Ptr<T>> Items => View;
    }
}
