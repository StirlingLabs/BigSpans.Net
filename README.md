# Big Spans

![Øresund Bridge](https://raw.githubusercontent.com/StirlingLabs/BigSpans.Net/663eb219eeb126bd2df585840a1a490de9420376/media/Øresund_Bridge_wide_small.jpg)

![coverage badge](https://raw.githubusercontent.com/StirlingLabs/BigSpans.Net/coverage/coverage/badge_combined.svg)
[![CodeFactor](https://www.codefactor.io/repository/github/stirlinglabs/bigspans.net/badge?s=eff9fc166f2e137f96ee77a5d51b8891904fdd92)](https://www.codefactor.io/repository/github/stirlinglabs/bigspans.net)
[![Codacy](https://app.codacy.com/project/badge/Grade/895756ab1b8646bdaac016dd7eaefa26)](https://www.codacy.com/gh/StirlingLabs/BigSpans.Net/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=StirlingLabs/BigSpans.Net&amp;utm_campaign=Badge_Grade)
[![Test Status](https://badgen.net/github/checks/StirlingLabs/BigSpans.Net?icon=github)](https://github.com/StirlingLabs/BigSpans.Net/actions)
[![Latest Release](https://badgen.net/github/release/StirlingLabs/BigSpans.Net/stable:color?icon=github)](https://github.com/StirlingLabs/BigSpans.Net/releases/latest)
[![NuGet](https://badgen.net/github/tag/StirlingLabs/BigSpans.Net?icon=nuget)](https://github.com/orgs/StirlingLabs/packages?repo_name=BigSpans.Net)

## Spans for the whole memory range

With StirlingLabs.BigSpans, you have access to 64-bit `BigSpan` and `ReadOnlyBigSpan` in .NET Standard 2.0, 2.1 and .NET 5.0 packages.

BigSpans have full interoperability with .NET Standard's System.Memory's Spans and the standard .NET Runtime's Spans.

BigSpans take up the same space on the stack as regular Spans, but use
up the padding at the end of their allocation to contain a full native
unsigned integer sized length. This padding is always copied around with
the BigSpan, so it's safe enough to use to store this information.

A host of useful extensions are provided:
- Fast sequential memory equality and comparisons extensions.
- A BinaryPrimitives work-alike that works on BigSpans.
- A pinning `IEnumerable<T>` implementation.
- Unmanaged memory allocation exposed as BigSpans.
- Automatic slicing into regular Spans.
