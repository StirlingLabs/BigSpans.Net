using System.Runtime.CompilerServices;

namespace StirlingLabs.Utilities.Compatibility
{
    internal static class NetStandard2Compatibility
    {
#if NETSTANDARD2_1 || NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareTo(this nuint a, nuint b)
            => a < b ? -1 : a > b ? 1 : 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareTo(this nuint a, uint b)
            => a < b ? -1 : a > b ? 1 : 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareTo(this nuint a, ulong b)
            => a < b ? -1 : a > b ? 1 : 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareTo(this nuint a, int b)
            => b < 0 ? 1 : a.CompareTo((uint)b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareTo(this nuint a, long b)
            => b < 0 ? 1 : a.CompareTo((ulong)b);
#endif
    }
}
