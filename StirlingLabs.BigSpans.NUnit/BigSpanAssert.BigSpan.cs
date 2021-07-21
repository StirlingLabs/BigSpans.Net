using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace StirlingLabs.Utilities.Assertions
{
    public static partial class BigSpanAssert
    {
        #region AllItemsAreInstancesOfType

        /// <summary>
        /// Asserts that all items contained in collection are of the type specified by expectedType.
        /// </summary>
        /// <param name="collection">BigSpan containing objects to be considered</param>
        /// <param name="expectedType">System.Type that all objects in collection must be instances of</param>
        public static void AllItemsAreInstancesOfType<T>(BigSpan<T> collection, Type expectedType)
            => AllItemsAreInstancesOfType(collection, expectedType, string.Empty, null);

        /// <summary>
        /// Asserts that all items contained in collection are of the type specified by expectedType.
        /// </summary>
        /// <param name="collection">BigSpan containing objects to be considered</param>
        /// <param name="expectedType">System.Type that all objects in collection must be instances of</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AllItemsAreInstancesOfType<T>(BigSpan<T> collection, Type expectedType, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Is.All.InstanceOf(expectedType), message, args));

        #endregion

        #region AllItemsAreNotNull

        /// <summary>
        /// Asserts that all items contained in collection are not equal to null.
        /// </summary>
        /// <param name="collection">BigSpan containing objects to be considered</param>
        public static void AllItemsAreNotNull<T>(BigSpan<T> collection)
            => AllItemsAreNotNull(collection, string.Empty, null);

        /// <summary>
        /// Asserts that all items contained in collection are not equal to null.
        /// </summary>
        /// <param name="collection">BigSpan of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AllItemsAreNotNull<T>(BigSpan<T> collection, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Is.All.Not.Null, message, args));

        #endregion

        #region AllItemsAreUnique

        /// <summary>
        /// Ensures that every object contained in collection exists within the collection
        /// once and only once.
        /// </summary>
        /// <param name="collection">BigSpan of objects to be considered</param>
        public static void AllItemsAreUnique<T>(BigSpan<T> collection)
            => AllItemsAreUnique(collection, string.Empty, null);

        /// <summary>
        /// Ensures that every object contained in collection exists within the collection
        /// once and only once.
        /// </summary>
        /// <param name="collection">BigSpan of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AllItemsAreUnique<T>(BigSpan<T> collection, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Is.Unique, message, args));

        #endregion

        #region AreEqual

        /// <summary>
        /// Asserts that expected and actual are exactly equal.  The collections must have the same count,
        /// and contain the exact same objects in the same order.
        /// </summary>
        /// <param name="expected">The first BigSpan of objects to be considered</param>
        /// <param name="actual">The second BigSpan of objects to be considered</param>
        public static void AreEqual<T>(BigSpan<T> expected, BigSpan<T> actual)
            => AreEqual(expected, actual, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are exactly equal.  The collections must have the same count,
        /// and contain the exact same objects in the same order.
        /// If comparer is not null then it will be used to compare the objects.
        /// </summary>
        /// <param name="expected">The first BigSpan of objects to be considered</param>
        /// <param name="actual">The second BigSpan of objects to be considered</param>
        /// <param name="comparer">The IComparer to use in comparing objects from each BigSpan</param>
        public static void AreEqual<T>(BigSpan<T> expected, BigSpan<T> actual, IComparer comparer)
            => AreEqual(expected, actual, comparer, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are exactly equal.  The collections must have the same count,
        /// and contain the exact same objects in the same order.
        /// </summary>
        /// <param name="expected">The first BigSpan of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreEqual<T>(BigSpan<T> expected, BigSpan<T> actual, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.EqualTo(e).AsCollection, message, args));

        /// <summary>
        /// Asserts that expected and actual are exactly equal.  The collections must have the same count,
        /// and contain the exact same objects in the same order.
        /// If comparer is not null then it will be used to compare the objects.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="comparer">The IComparer to use in comparing objects from each BigSpan</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreEqual<T>(BigSpan<T> expected, BigSpan<T> actual, IComparer comparer, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.EqualTo(e).Using(comparer), message, args));

        #endregion

        #region AreEquivalent

        /// <summary>
        /// Asserts that expected and actual are equivalent, containing the same objects but the match may be in any order.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        public static void AreEquivalent<T>(BigSpan<T> expected, BigSpan<T> actual)
            => AreEquivalent(expected, actual, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are equivalent, containing the same objects but the match may be in any order.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreEquivalent<T>(BigSpan<T> expected, BigSpan<T> actual, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.EquivalentTo(e), message, args));

        #endregion

        #region AreNotEqual

        /// <summary>
        /// Asserts that expected and actual are not exactly equal.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        public static void AreNotEqual<T>(BigSpan<T> expected, BigSpan<T> actual)
            => AreNotEqual(expected, actual, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are not exactly equal.
        /// If comparer is not null then it will be used to compare the objects.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="comparer">The IComparer to use in comparing objects from each BigSpan</param>
        public static void AreNotEqual<T>(BigSpan<T> expected, BigSpan<T> actual, IComparer comparer)
            => AreNotEqual(expected, actual, comparer, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are not exactly equal.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreNotEqual<T>(BigSpan<T> expected, BigSpan<T> actual, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.Not.EqualTo(e).AsCollection, message, args));

        /// <summary>
        /// Asserts that expected and actual are not exactly equal.
        /// If comparer is not null then it will be used to compare the objects.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="comparer">The IComparer to use in comparing objects from each BigSpan</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreNotEqual<T>(BigSpan<T> expected, BigSpan<T> actual, IComparer comparer, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.Not.EqualTo(e).Using(comparer), message, args));

        #endregion

        #region AreNotEquivalent

        /// <summary>
        /// Asserts that expected and actual are not equivalent.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        public static void AreNotEquivalent<T>(BigSpan<T> expected, BigSpan<T> actual)
            => AreNotEquivalent(expected, actual, string.Empty, null);

        /// <summary>
        /// Asserts that expected and actual are not equivalent.
        /// </summary>
        /// <param name="expected">The first IEnumerable of objects to be considered</param>
        /// <param name="actual">The second IEnumerable of objects to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void AreNotEquivalent<T>(BigSpan<T> expected, BigSpan<T> actual, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(expected, actual, (e, a)
                => Assert.That(a, Is.Not.EquivalentTo(e), message, args));

        #endregion

        #region Contains

        /// <summary>
        /// Asserts that collection contains actual as an item.
        /// </summary>
        /// <param name="collection">BigSpan of objects to be considered</param>
        /// <param name="actual">Object to be found within collection</param>
        public static void Contains<T>(BigSpan<T> collection, Object actual)
            => Contains(collection, actual, string.Empty, null);

        /// <summary>
        /// Asserts that collection contains actual as an item.
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="actual">Object to be found within collection</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void Contains<T>(BigSpan<T> collection, Object actual, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Has.Member(actual), message, args));

        #endregion

        #region DoesNotContain

        /// <summary>
        /// Asserts that collection does not contain actual as an item.
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="actual">Object that cannot exist within collection</param>
        public static void DoesNotContain<T>(BigSpan<T> collection, Object actual)
            => DoesNotContain(collection, actual, string.Empty, null);

        /// <summary>
        /// Asserts that collection does not contain actual as an item.
        /// </summary>
        /// <param name="collection">IEnumerable of objects to be considered</param>
        /// <param name="actual">Object that cannot exist within collection</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void DoesNotContain<T>(BigSpan<T> collection, Object actual, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Has.No.Member(actual), message, args));

        #endregion

        #region IsNotSubsetOf

        /// <summary>
        /// Asserts that the superset does not contain the subset
        /// </summary>
        /// <param name="subset">The IEnumerable subset to be considered</param>
        /// <param name="superset">The BigSpan superset to be considered</param>
        public static void IsNotSubsetOf<T>(BigSpan<T> subset, BigSpan<T> superset)
            => IsNotSubsetOf(subset, superset, string.Empty, null);

        /// <summary>
        /// Asserts that the superset does not contain the subset
        /// </summary>
        /// <param name="subset">The IEnumerable subset to be considered</param>
        /// <param name="superset">The BigSpan superset to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsNotSubsetOf<T>(BigSpan<T> subset, BigSpan<T> superset, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(subset, superset, (sub, sup)
                => Assert.That(sub, Is.Not.SubsetOf(sup), message, args));

        #endregion

        #region IsSubsetOf

        /// <summary>
        /// Asserts that the superset contains the subset.
        /// </summary>
        /// <param name="subset">The IEnumerable subset to be considered</param>
        /// <param name="superset">The BigSpan superset to be considered</param>
        public static void IsSubsetOf<T>(BigSpan<T> subset, BigSpan<T> superset)
            => IsSubsetOf(subset, superset, string.Empty, null);

        /// <summary>
        /// Asserts that the superset contains the subset.
        /// </summary>
        /// <param name="subset">The IEnumerable subset to be considered</param>
        /// <param name="superset">The BigSpan superset to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsSubsetOf<T>(BigSpan<T> subset, BigSpan<T> superset, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(subset, superset, (sub, sup)
                => Assert.That(sub, Is.SubsetOf(sup), message, args));

        #endregion


        #region IsNotSupersetOf

        /// <summary>
        /// Asserts that the subset does not contain the superset
        /// </summary>
        /// <param name="superset">The IEnumerable superset to be considered</param>
        /// <param name="subset">The BigSpan subset to be considered</param>
        public static void IsNotSupersetOf<T>(BigSpan<T> superset, BigSpan<T> subset)
            => IsNotSupersetOf(superset, subset, string.Empty, null);

        /// <summary>
        /// Asserts that the subset does not contain the superset
        /// </summary>
        /// <param name="superset">The BigSpan superset to be considered</param>
        /// <param name="subset">The BigSpan subset to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsNotSupersetOf<T>(BigSpan<T> superset, BigSpan<T> subset, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(subset, superset, (sub, sup)
                => Assert.That(sup, Is.Not.SupersetOf(sub), message, args));

        #endregion

        #region IsSupersetOf

        /// <summary>
        /// Asserts that the subset contains the superset.
        /// </summary>
        /// <param name="superset">The BigSpan superset to be considered</param>
        /// <param name="subset">The BigSpan subset to be considered</param>
        public static void IsSupersetOf<T>(BigSpan<T> superset, BigSpan<T> subset)
            => IsSupersetOf(superset, subset, string.Empty, null);

        /// <summary>
        /// Asserts that the subset contains the superset.
        /// </summary>
        /// <param name="superset">The BigSpan superset to be considered</param>
        /// <param name="subset">The BigSpan subset to be considered</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsSupersetOf<T>(BigSpan<T> superset, BigSpan<T> subset, string message, params object[] args)
            => BigSpan.AsPinnedEnumerables<T, T>(subset, superset, (sub, sup)
                => Assert.That(sup, Is.SupersetOf(sub), message, args));

        #endregion


        #region IsEmpty

        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing BigSpan</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsEmpty<T>(BigSpan<T> collection, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, new EmptyCollectionConstraint(), message, args));

        /// <summary>
        /// Assert that an array,list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        public static void IsEmpty<T>(BigSpan<T> collection)
            => IsEmpty(collection, string.Empty, null);

        #endregion

        #region IsNotEmpty

        /// <summary>
        /// Assert that an array, list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsNotEmpty<T>(BigSpan<T> collection, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, new NotConstraint(new EmptyCollectionConstraint()), message, args));

        /// <summary>
        /// Assert that an array,list or other collection is empty
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        public static void IsNotEmpty<T>(BigSpan<T> collection)
            => IsNotEmpty(collection, string.Empty, null);

        #endregion

        #region IsOrdered

        /// <summary>
        /// Assert that an array, list or other collection is ordered
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsOrdered<T>(BigSpan<T> collection, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Is.Ordered, message, args));

        /// <summary>
        /// Assert that an array, list or other collection is ordered
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        public static void IsOrdered<T>(BigSpan<T> collection)
            => IsOrdered(collection, string.Empty, null);

        /// <summary>
        /// Assert that an array, list or other collection is ordered
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        /// <param name="comparer">A custom comparer to perform the comparisons</param>
        /// <param name="message">The message to be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void IsOrdered<T>(BigSpan<T> collection, IComparer comparer, string message, params object[] args)
            => collection.AsPinnedEnumerable(e => Assert.That(e, Is.Ordered.Using(comparer), message, args));

        /// <summary>
        /// Assert that an array, list or other collection is ordered
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing IEnumerable</param>
        /// <param name="comparer">A custom comparer to perform the comparisons</param>
        public static void IsOrdered<T>(BigSpan<T> collection, IComparer comparer)
            => IsOrdered(collection, comparer, string.Empty, null);

        #endregion
    }
}
