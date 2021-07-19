namespace StirlingLabs.Utilities.Magic
{
    public static class BigSpanExtensions
    {
        /// <summary>
        /// Returns a reference to the 0th element of the BigSpan. If the BigSpan is empty, returns a reference to the location where the 0th element
        /// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
        /// </summary>
        public static ref T GetReference<T>(this BigSpan<T> span)
            => ref span._pointer.Value;
        /// <summary>
        /// Returns a reference to the 0th element of the ReadOnlyBigSpan. If the ReadOnlyBigSpan is empty, returns a reference to the location where the 0th element
        /// would have been stored. Such a reference may or may not be null. It can be used for pinning but must never be dereferenced.
        /// </summary>
        public static ref T GetReference<T>(this ReadOnlyBigSpan<T> span)
            => ref span._pointer.Value;
    }
}
