using System;
using System.Diagnostics.CodeAnalysis;

namespace StirlingLabs;

[ExcludeFromCodeCoverage]
[SuppressMessage("	Usage", "CA1806", Justification = "Intentional")]
internal class GarbageCollectedNotifier
{
    static GarbageCollectedNotifier()
        // ReSharper disable once ObjectCreationAsStatement
        => new GarbageCollectedNotifier();

    private GarbageCollectedNotifier() { }

    public static event Action? GarbageCollected;

    ~GarbageCollectedNotifier()
    {
        if (Environment.HasShutdownStarted
            || AppDomain.CurrentDomain.IsFinalizingForUnload())
            return;

        GarbageCollected?.Invoke();

        GC.KeepAlive(new GarbageCollectedNotifier());
    }
}
