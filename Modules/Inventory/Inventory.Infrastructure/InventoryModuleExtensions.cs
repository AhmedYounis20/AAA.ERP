using ERP.Application.Data;
using ERP.Application.Repositories.Inventory;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class InventoryModuleExtensions
{
    public static IServiceCollection AddInventoryModule(this IServiceCollection services)
    {
        ModuleModelConfigurationRegistry.Register(ApplyInventoryModuleConfigurations);

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IPackingUnitRepository, PackingUnitRepository>();
        services.AddScoped<IPackingUnitService, PackingUnitService>();
        services.AddScoped<IManufacturerCompanyRepository, ManufacturerCompanyRepository>();
        services.AddScoped<IManufacturerCompanyService, ManufacturerCompanyService>();

        return services;
    }

    public static void ApplyInventoryModuleConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ColorDbConfig());
        modelBuilder.ApplyConfiguration(new SizeDbConfig());
        modelBuilder.ApplyConfiguration(new PackingUnitDbConfig());
        modelBuilder.ApplyConfiguration(new ManufacturerCompanyDbConfig());
    }
}
