using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace StirlingLabs.Utilities
{
    [SuppressMessage("Microsoft.Design","CA1060", Justification = "A partial is good enough")]
    public static partial class UnmanagedMemory
    {
#if !NETSTANDARD
        [SuppressGCTransition]
#endif
        [DllImport("libc", EntryPoint = "malloc")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern unsafe void* C_Allocate(nuint size);

#if !NETSTANDARD
        [SuppressGCTransition]
#endif
        [DllImport("libc", EntryPoint = "free")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern unsafe void C_Free(void* size);

#if !NETSTANDARD
        [SuppressGCTransition]
#endif
        [DllImport("libc", EntryPoint = "memcmp")]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern unsafe int C_CompareMemory(void* a, void* b, nuint size);
    }
}
