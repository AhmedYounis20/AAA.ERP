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
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<ICollectionBookService, CollectionBookService>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        services.AddScoped<ICostCenterService, CostCenterService>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();

        return services;
    }

    public static void ApplyAccountingModuleConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaxDbConfig());
        modelBuilder.ApplyConfiguration(new CurrencyDbConfig());
        modelBuilder.ApplyConfiguration(new GLSettingDbConfig());
        modelBuilder.ApplyConfiguration(new AccountGuideDbConfig());
        modelBuilder.ApplyConfiguration(new CollectionBookDbConfig());
        modelBuilder.ApplyConfiguration(new FinancialPeriodDbConfig());
        modelBuilder.ApplyConfiguration(new CostCenterDbConfig());
        modelBuilder.ApplyConfiguration(new CostCenterChartOfAccountDbConfig());
        modelBuilder.ApplyConfiguration(new ChartOfAccountDbConfig());
    }
}
