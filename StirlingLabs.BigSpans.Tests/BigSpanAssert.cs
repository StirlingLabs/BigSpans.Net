using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NUnit.Framework;
using StirlingLabs.Utilities;

namespace StirlingLabs.BigSpans.Tests
{
    [ExcludeFromCodeCoverage]
    internal static class BigSpanAssert<T>
    {
        public delegate void BigSpanAction(BigSpan<T> span);

        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
        public static TException Throws<TException>(BigSpan<T> span, [InstantHandle] BigSpanAction action) where TException : Exception
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

        public delegate TResult BigSpanFunc<out TResult>(BigSpan<T> span);
        
        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "Test case assertion helper.")]
        public static TException Throws<TException>(BigSpan<T> span, BigSpanFunc<T> action) where TException : Exception
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
        public static TException Throws<TException, TResult>(BigSpan<T> span, BigSpanFunc<TResult> action) where TException : Exception
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
}
