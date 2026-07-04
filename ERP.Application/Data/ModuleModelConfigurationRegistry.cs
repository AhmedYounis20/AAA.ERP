using Microsoft.EntityFrameworkCore;

namespace ERP.Application.Data;

public static class ModuleModelConfigurationRegistry
{
    public static IReadOnlyList<Action<ModelBuilder>> Configurators => _configurators;

    private static readonly List<Action<ModelBuilder>> _configurators = [];

    public static void Register(Action<ModelBuilder> configure)
    {
        _configurators.Add(configure);
    }

    public static void Clear() => _configurators.Clear();
}
