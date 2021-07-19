using JetBrains.Annotations;

namespace System.Runtime.CompilerServices
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal sealed class IgnoresAccessChecksToAttribute : Attribute
    {
        private readonly string _assemblyName;

        // ReSharper disable once ConvertToAutoProperty
        public string AssemblyName => _assemblyName;
        public IgnoresAccessChecksToAttribute(string assemblyName)
            => _assemblyName = assemblyName;
    }
}
