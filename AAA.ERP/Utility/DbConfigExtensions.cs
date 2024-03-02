using AAA.ERP.DBConfiguration.Config.BaseConfig;
using System.Reflection;

namespace AAA.ERP.Utility;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
    {
        var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces()
            .Any(gi => gi.IsGenericType
            && ((gi.GetGenericTypeDefinition() == typeof(BaseEntityDbConfig<>))
                || (gi.GetGenericTypeDefinition() == typeof(BaseSettingEntityDbConfig<>))
                ))).ToList();

        foreach (var type in typesToRegister)
        {
            dynamic? configurationInstance = Activator.CreateInstance(type);
            if (configurationInstance != null)
                modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}
