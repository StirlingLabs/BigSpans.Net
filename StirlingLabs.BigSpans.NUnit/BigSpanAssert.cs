using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace StirlingLabs.Utilities.Assertions
{
    [PublicAPI]
    public static partial class BigSpanAssert
    {
        #region Equals and ReferenceEquals

        /// <summary>
        /// DO NOT USE! Use CollectionAssert.AreEqual(...) instead.
        /// The Equals method throws an InvalidOperationException. This is done
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Obsolete("CollectionAssert.Equals should not be used. Use CollectionAssert.AreEqual instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new static bool Equals(object a, object b)
            => throw new InvalidOperationException("CollectionAssert.Equals should not be used. Use CollectionAssert.AreEqual instead.");

        /// <summary>
        /// DO NOT USE!
        /// The ReferenceEquals method throws an InvalidOperationException. This is done
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [Obsolete("CollectionAssert.Equals should not be used. Use CollectionAssert.AreEqual instead.", true)]
        public new static void ReferenceEquals(object a, object b)
            => throw new InvalidOperationException("CollectionAssert.ReferenceEquals should not be used.");

        #endregion
    }
}
