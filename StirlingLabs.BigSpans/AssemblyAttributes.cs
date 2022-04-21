using System.Runtime.CompilerServices;

#if !NET6_0_OR_GREATER
[assembly: DisablePrivateReflection]
#endif

[assembly: InternalsVisibleTo("StirlingLabs.BigSpans.Tests")]
[assembly: IgnoresAccessChecksTo("mscorlib")]
[assembly: IgnoresAccessChecksTo("netstandard")]
[assembly: IgnoresAccessChecksTo("System.Runtime")]
[assembly: IgnoresAccessChecksTo("System.Private.CoreLib")]
