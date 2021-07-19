using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities
{
    [PublicAPI]
    public static partial class UnmanagedMemory
    {
        private static readonly Assembly ThisAssembly = typeof(UnmanagedMemory).Assembly;

#if !NETSTANDARD
        static UnmanagedMemory()
            => NativeLibrary.SetDllImportResolver(ThisAssembly, (name, assembly, path) => {
                if (name == "libc" && assembly == ThisAssembly)
                    return NativeLibrary.Load(
                        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                            // ReSharper disable once StringLiteralTypo
                            ? "msvcrt"
                            // ReSharper disable once StringLiteralTypo
                            : "libc"
                    );
                return default;
            });
#endif

        [SuppressMessage("Microsoft.Design","CA1021", Justification = "Not for general audience")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Allocate(nuint size, out nint pointer)
        {
            try
            {
                pointer = (nint)C_Allocate(size);
            }
            catch (OutOfMemoryException)
            {
                pointer = default;
                return false;
            }
            return pointer != default;
        }

        [SuppressMessage("Microsoft.Design","CA1021", Justification = "Not for general audience")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Allocate(nuint size, out void* pointer)
        {
            try
            {
                pointer = C_Allocate(size);
            }
            catch (OutOfMemoryException)
            {
                pointer = default;
                return false;
            }
            return pointer != default;
        }

        [SuppressMessage("Microsoft.Design","CA1021", Justification = "Not for general audience")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Allocate<T>(nuint count, out T* pointer)
            where T : unmanaged
        {
            var success = Allocate(count * (nuint)sizeof(T), out void* pointerValue);
            pointer = (T*)pointerValue;
            return success;
        }
        
        [SuppressMessage("Microsoft.Design","CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Free(ref nint nativePointer)
        {
            C_Free((void*)nativePointer);
            nativePointer = default;
        }
        
        [SuppressMessage("Microsoft.Design","CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Free(ref void* nativePointer)
        {
            C_Free(nativePointer);
            nativePointer = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe BigSpan<T> AllocateBigSpan<T>(nuint count)
            where T : unmanaged
        {
            if (Allocate(count * (nuint)sizeof(T), out void* pointerValue))
                return new(pointerValue, count);
            throw new OutOfMemoryException();
        }
        
        [SuppressMessage("Microsoft.Design","CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeBigSpan<T>(ref BigSpan<T> span)
            where T : unmanaged
        {
            C_Free(span.GetUnsafePointer());
            span = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<T> AllocateSpan<T>(int count)
            where T : unmanaged
        {
            if (Allocate((nuint)count * (nuint)sizeof(T), out void* pointerValue))
                return new(pointerValue, count);
            throw new OutOfMemoryException();
        }
        
        [SuppressMessage("Microsoft.Design","CA1045", Justification = "Nope")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FreeSpan<T>(ref Span<T> span)
            where T : unmanaged
        {
            C_Free(Unsafe.AsPointer(ref span.GetPinnableReference()));
            span = default;
        }
    }
}
