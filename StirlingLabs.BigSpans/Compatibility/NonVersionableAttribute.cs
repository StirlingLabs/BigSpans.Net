using JetBrains.Annotations;

namespace System.Runtime.Versioning
{
    [PublicAPI]
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor,
        Inherited = false)]
    internal sealed class NonVersionableAttribute : Attribute { }
}
