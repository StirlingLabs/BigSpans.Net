using JetBrains.Annotations;

namespace System.Runtime.CompilerServices
{
    [PublicAPI]
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Field,
        Inherited = false)]
    internal sealed class IntrinsicAttribute : Attribute { }
}
