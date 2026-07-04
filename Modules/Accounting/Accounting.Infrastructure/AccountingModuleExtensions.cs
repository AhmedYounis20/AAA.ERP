using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Repositories;
using Accounting.Infrastructure.Services;
using ERP.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure;

public static class AccountingModuleExtensions
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services)
    {
        ModuleModelConfigurationRegistry.Register(ApplyAccountingModuleConfigurations);

        services.AddScoped<ITaxRepository, TaxRepository>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ICurrencyService, CurrencyService>();

        return services;
    }

    public static void ApplyAccountingModuleConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaxDbConfig());
        modelBuilder.ApplyConfiguration(new CurrencyDbConfig());
    }
}
