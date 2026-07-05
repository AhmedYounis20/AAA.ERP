using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Services;
using Accounting.Application.Services.Queries;
using Accounting.Contracts.Integration;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Persistence.Attachments;
using Accounting.Infrastructure.Repositories;
using Accounting.Infrastructure.Services;
using Accounting.Infrastructure.Services.Queries;
using ERP.BuildingBlocks.Persistence.Data;
using Accounting.Application.Repositories.Account.SubLeadgers;
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
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<ICashInBoxRepository, CashInBoxRepository>();
        services.AddScoped<ICashInBoxService, CashInBoxService>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<IFixedAssetService, FixedAssetService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IJournalEntryService, JournalEntryService>();
        services.AddScoped<IPaymentEntryService, PaymentEntryService>();
        services.AddScoped<IReceiptEntryService, ReceiptEntryService>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IComplexEntryService, ComplexEntryService>();
        services.AddScoped<IOpeningEntryService, OpeningEntryService>();
        services.AddScoped<ICombinedEntryService, CombinedEntryService>();
        services.AddScoped<IAccountingIntegrationService, AccountingIntegrationService>();
        services.AddScoped<IInventoryCatalogValidationQueries, InventoryCatalogValidationQueries>();
        services.AddScoped<IInventoryAccountingDisplayQueries, InventoryAccountingDisplayQueries>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IFinancialPeriodUsageQueries, FinancialPeriodUsageQueries>();
        services.AddScoped(typeof(IBaseQueryService<,>), typeof(BaseQueryService<,>));

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
        modelBuilder.ApplyConfiguration(new BankDbConfig());
        modelBuilder.ApplyConfiguration(new CashInBoxDbConfig());
        modelBuilder.ApplyConfiguration(new BranchDbConfig());
        modelBuilder.ApplyConfiguration(new CustomerDbConfig());
        modelBuilder.ApplyConfiguration(new SupplierDbConfig());
        modelBuilder.ApplyConfiguration(new FixedAssetDbConfig());
        modelBuilder.ApplyConfiguration(new RoleDbConfig());
        modelBuilder.ApplyConfiguration(new EntryDbConfig());
        modelBuilder.ApplyConfiguration(new FinancialTransactionDbConfig());
        modelBuilder.ApplyConfiguration(new EntryCostCenterDbConfig());
        modelBuilder.ApplyConfiguration(new EntryAttachmentDbConfig());
        modelBuilder.ApplyConfiguration(new AttachmentDbConfig());
    }
}
