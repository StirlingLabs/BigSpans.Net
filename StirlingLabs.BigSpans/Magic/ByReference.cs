using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using static InlineIL.IL;
using static InlineIL.IL.Emit;

namespace StirlingLabs.Utilities.Magic
{
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ByReference<T>
    {
        private readonly ReadOnlySpan<T> _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(Span<T> span)
            => _span = span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(ReadOnlySpan<T> span)
            => _span = span;

#if NETSTANDARD2_0
        [SuppressMessage("Microsoft.Design","CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ByReference(ref T item)
            => _span = new(Unsafe.AsPointer(ref item), 1);
#else
        [SuppressMessage("Microsoft.Design", "CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(ref T item)
            => _span = MemoryMarshal.CreateReadOnlySpan(ref item, 1);
#endif

        public ref T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetReference(_span);
        }

        public ref nuint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Ldarg_0();
                Sizeof(typeof(void*));
                Add();
                return ref ReturnRef<nuint>();
            }
        }
    }
}
