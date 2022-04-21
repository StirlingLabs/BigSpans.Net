using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using StirlingLabs.Utilities;

namespace StirlingLabs.Utilities.Assertions;

[ExcludeFromCodeCoverage]
internal static class ReadOnlyBigSpanAssert<T>
{
    public delegate void Action(ReadOnlyBigSpan<T> span);

    [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
    public static TException Throws<TException>(ReadOnlyBigSpan<T> span, Action action) where TException : Exception
    {
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

    public delegate TResult Func<out TResult>(ReadOnlyBigSpan<T> span);

    [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
    public static TException Throws<TException>(ReadOnlyBigSpan<T> span, Func<T> action)
        where TException : Exception
    {
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
    public static TException Throws<TException, TResult>(ReadOnlyBigSpan<T> span, Func<T> action)
        where TException : Exception
    {
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
