using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;

namespace StirlingLabs.Utilities.Assertions;

[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class BigSpanAssert
{
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
}

[PublicAPI]
[ExcludeFromCodeCoverage]
public static class BigSpanAssert<T>
{
    public delegate void BigSpanAction(BigSpan<T> span);

    [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
    public static TException Throws<TException>(BigSpan<T> span, [InstantHandle] BigSpanAction action) where TException : Exception
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        Exception? exception;

        try
        {
            action(span);
            exception = null;
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        return exception switch
        {
            null => throw new AssertionException($"Did not throw {typeof(TException).FullName}"),
            TException ex when ex.GetType() == typeof(TException) => ex,
            _ => throw new AssertionException($"Did not throw {typeof(TException).FullName}")
        };
    }

    public delegate TResult BigSpanFunc<out TResult>(BigSpan<T> span);

    [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
    public static TException Throws<TException>(BigSpan<T> span, BigSpanFunc<T> action) where TException : Exception
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        Exception? exception;

        try
        {
            action(span);
            exception = null;
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        return exception switch
        {
            null => throw new AssertionException($"Did not throw {typeof(TException).FullName}"),
            TException ex when ex.GetType() == typeof(TException) => ex,
            _ => throw new AssertionException($"Did not throw {typeof(TException).FullName}")
        };
    }

    [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
    public static TException Throws<TException, TResult>(BigSpan<T> span, BigSpanFunc<TResult> action) where TException : Exception
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        Exception? exception;

        try
        {
            action(span);
            exception = null;
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        return exception switch
        {
            null => throw new AssertionException($"Did not throw {typeof(TException).FullName}"),
            TException ex when ex.GetType() == typeof(TException) => ex,
            _ => throw new AssertionException($"Did not throw {typeof(TException).FullName}")
        };
    }
}
