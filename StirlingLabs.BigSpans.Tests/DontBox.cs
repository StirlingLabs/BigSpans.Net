using System;
using System.Diagnostics.CodeAnalysis;
using StirlingLabs.Utilities;

namespace StirlingLabs.BigSpans.Tests;

[ExcludeFromCodeCoverage]
public static class DontBoxExtensions
{
    /// <summary>
    /// Prevents generation of boxing instructions in certain situations.
    /// </summary>
    /// <remarks>
    /// The innocent looking construct:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new Span&lt;T&gt;() );
    /// </code>
    /// generates a hidden box of the Span as the return value of the lambda. This makes the IL illegal and unloadable on
    /// runtimes that enforce the actual Span rules (never mind that we expect never to reach the box instruction...)
    ///
    /// The workaround is to code it like this:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new Span&lt;T&gt;().DontBox() );
    /// </code>
    /// which turns the lambda return type back to "void" and eliminates the troublesome box instruction.
    /// </remarks>
    public static void DontBox<T>(this Span<T> span)
    {
        // This space intentionally left blank.
    }

    /// <summary>
    /// Prevents generation of boxing instructions in certain situations.
    /// </summary>
    /// <remarks>
    /// The innocent looking construct:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new ReadOnlySpan&lt;T&gt;() );
    /// </code>
    /// generates a hidden box of the Span as the return value of the lambda. This makes the IL illegal and unloadable on
    /// runtimes that enforce the actual Span rules (never mind that we expect never to reach the box instruction...)
    ///
    /// The workaround is to code it like this:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new ReadOnlySpan&lt;T&gt;().DontBox() );
    /// </code>
    /// which turns the lambda return type back to "void" and eliminates the troublesome box instruction.
    /// </remarks>
    public static void DontBox<T>(this ReadOnlySpan<T> span)
    {
        // This space intentionally left blank.
    }

    /// <summary>
    /// Prevents generation of boxing instructions in certain situations.
    /// </summary>
    /// <remarks>
    /// The innocent looking construct:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new Span&lt;T&gt;() );
    /// </code>
    /// generates a hidden box of the Span as the return value of the lambda. This makes the IL illegal and unloadable on
    /// runtimes that enforce the actual Span rules (never mind that we expect never to reach the box instruction...)
    ///
    /// The workaround is to code it like this:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new Span&lt;T&gt;().DontBox() );
    /// </code>
    /// which turns the lambda return type back to "void" and eliminates the troublesome box instruction.
    /// </remarks>
    public static void DontBox<T>(this BigSpan<T> span)
    {
        // This space intentionally left blank.
    }

    /// <summary>
    /// Prevents generation of boxing instructions in certain situations.
    /// </summary>
    /// <remarks>
    /// The innocent looking construct:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new ReadOnlySpan&lt;T&gt;() );
    /// </code>
    /// generates a hidden box of the Span as the return value of the lambda. This makes the IL illegal and unloadable on
    /// runtimes that enforce the actual Span rules (never mind that we expect never to reach the box instruction...)
    ///
    /// The workaround is to code it like this:
    /// <code>
    ///    Assert.Throws&lt;E&gt;( () =&gt; new ReadOnlySpan&lt;T&gt;().DontBox() );
    /// </code>
    /// which turns the lambda return type back to "void" and eliminates the troublesome box instruction.
    /// </remarks>
    public static void DontBox<T>(this ReadOnlyBigSpan<T> span)
    {
        // This space intentionally left blank.
    }
}
