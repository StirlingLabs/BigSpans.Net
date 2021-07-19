using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities
{
    /// <summary>Enumerates the elements of a <see cref="ReadOnlyBigSpan{T}"/>.</summary>
    [PublicAPI]
    public ref struct ReadOnlyBigSpanEnumerator<T>
    {
        /// <summary>The span being enumerated.</summary>
        private readonly ReadOnlyBigSpan<T> _bigSpan;
        /// <summary>The next index to yield.</summary>
        private nuint _index;

        /// <summary>Initialize the enumerator.</summary>
        /// <param name="bigSpan">The span to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyBigSpanEnumerator(ReadOnlyBigSpan<T> bigSpan)
        {
            _bigSpan = bigSpan;
#if NETSTANDARD
            _index = ~(nuint)0;
#else
            _index = nuint.MaxValue;
#endif
        }

        /// <summary>Advances the enumerator to the next element of the span.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var index = _index + 1;
            if (index >= _bigSpan.Length)
                return false;

            _index = index;
            return true;

        }

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        public ref readonly T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _bigSpan[_index];
        }
    }
}
