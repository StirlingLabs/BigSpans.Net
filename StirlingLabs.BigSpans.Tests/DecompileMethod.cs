using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StirlingLabs.BigSpans.Tests
{
    public readonly struct DecompileMethod
    {
        public readonly nuint Pointer;
        public readonly string Description;

        public DecompileMethod(nuint pointer, string description)
        {
            Pointer = pointer;
            Description = description;
        }

        public unsafe DecompileMethod(void* p, string description)
        {
            Pointer = (nuint)p;
            Description = description;
        }

        public static implicit operator DecompileMethod(in (nuint, string) tuple)
            => Unsafe.As<(nuint, string), DecompileMethod>(ref Unsafe.AsRef(tuple));

        public static implicit operator DecompileMethod(MethodInfo mi)
        {
            var name = $"{mi.ReflectedType}.{mi.Name}";
            try
            {
                if (mi.IsGenericMethodDefinition)
                    mi = mi.MakeGenericMethod(mi.GetGenericArguments().Select(g => typeof(byte)).ToArray());
                RuntimeHelpers.PrepareMethod(mi.MethodHandle);
                return new((nuint)(nint)mi.MethodHandle.GetFunctionPointer(), name);
            }
            catch
            {
                return new(0, name);
            }
        }

        public override string ToString()
            => Description;
    }
}
