using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities;

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

    public unsafe BigSpanDebugView(ReadOnlyBigSpan<T> span)
        => View = span.Length != 0
            ? new(span.GetUnsafePointer(), span.Length)
            : default;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IEnumerable<UnsafePtr<T>> Items => View;
}
