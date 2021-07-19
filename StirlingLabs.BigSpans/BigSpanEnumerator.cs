using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities
{
    /// <summary>Enumerates the elements of a <see cref="BigSpan{T}"/>.</summary>
    [PublicAPI]
    public ref struct BigSpanEnumerator<T>
    {
        /// <summary>The span being enumerated.</summary>
        private readonly BigSpan<T> _bigSpan;
        /// <summary>The next index to yield.</summary>
        private nuint _index;

        /// <summary>Initialize the enumerator.</summary>
        /// <param name="bigSpan">The span to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpanEnumerator(BigSpan<T> bigSpan)
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
        public ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _bigSpan[_index];
        }
    }
}
