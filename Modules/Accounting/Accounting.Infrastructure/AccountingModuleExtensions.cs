using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Application.Services.Queries;
using Accounting.Contracts.Integration;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Persistence.Attachments;
using Accounting.Infrastructure.Repositories;
using Accounting.Infrastructure.Services;
using Accounting.Infrastructure.Services.Entries;
using Accounting.Infrastructure.Services.Queries;
using Accounting.Infrastructure.Workflows;
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
        services.AddScoped<ITaxQueries, TaxQueries>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ICurrencyQueries, CurrencyQueries>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IGLSettingQueries, GLSettingQueries>();
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<IAccountGuideQueries, AccountGuideQueries>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<ICollectionBookQueries, CollectionBookQueries>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IFinancialPeriodWorkflowService, FinancialPeriodWorkflowService>();
        services.AddScoped<IFinancialPeriodQueries, FinancialPeriodQueries>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        services.AddScoped<ICostCenterWorkflowService, CostCenterWorkflowService>();
        services.AddScoped<ICostCenterQueries, CostCenterQueries>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<IChartOfAccountQueries, ChartOfAccountQueries>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IBankQueries, BankQueries>();
        services.AddScoped<ICashInBoxRepository, CashInBoxRepository>();
        services.AddScoped<ICashInBoxQueries, CashInBoxQueries>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IBranchWorkflowService, BranchWorkflowService>();
        services.AddScoped<IBranchQueries, BranchQueries>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerQueries, CustomerQueries>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierQueries, SupplierQueries>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<IFixedAssetWorkflowService, FixedAssetWorkflowService>();
        services.AddScoped<IFixedAssetQueries, FixedAssetQueries>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleQueries, RoleQueries>();
        services.AddScoped<IJournalEntryQueries, JournalEntryQueries>();
        services.AddScoped<IPaymentEntryQueries, PaymentEntryQueries>();
        services.AddScoped<IReceiptEntryQueries, ReceiptEntryQueries>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryWorkflowService, EntryWorkflowService>();
        services.AddScoped<IComplexEntryWorkflowService, ComplexEntryWorkflowService>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IComplexEntryService, ComplexEntryService>();
        services.AddScoped<IComplexEntryQueries, ComplexEntryQueries>();
        services.AddScoped<IOpeningEntryQueries, OpeningEntryQueries>();
        services.AddScoped<ICombinedEntryQueries, CombinedEntryQueries>();
        services.AddScoped<IAccountingIntegrationService, AccountingIntegrationService>();
        services.AddScoped<IInventoryCatalogValidationQueries, InventoryCatalogValidationQueries>();
        services.AddScoped<IInventoryAccountingDisplayQueries, InventoryAccountingDisplayQueries>();
        services.AddScoped<IAttachmentCrossModulePort, AttachmentCrossModulePort>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
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
