
# Big Spans
[![CodeFactor](https://www.codefactor.io/repository/github/stirlinglabs/bigspans.net/badge?s=eff9fc166f2e137f96ee77a5d51b8891904fdd92)](https://www.codefactor.io/repository/github/stirlinglabs/bigspans.net)
<img alt="bridge by Simon Child from the Noun Project" src="media/noun_bridge_4720.svg" width="636" />

**Spans for the whole memory range.**

With StirlingLabs.BigSpans, you have access to BigSpan and ReadOnlyBigSpan
in .NET Standard 2.0, 2.1 and .NET 5.0 packages.

You have full interoperability with .NET Standard's System.Memory's Spans
and the standard .NET Runtime's Spans.

BigSpans take up the same space on the stack as regular Spans, but use
up the padding at the end of their allocation to contain a full native
unsigned integer sized length. This padding is always copied around with
the BigSpan, so it's safe enough to use to store this information.

A host of useful extensions are provided.
 * Fast sequential memory equality and comparisons extensions.
 * A BinaryPrimitives work-alike that works on BigSpans.
 * A pinning `IEnumerable<T>` implementation.
 * Unmanaged memory allocation exposed as BigSpans.
 * Automatic slicing into regular Spans.
