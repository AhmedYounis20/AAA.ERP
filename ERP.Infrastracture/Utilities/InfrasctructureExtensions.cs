using AAA.ERP.Services.Impelementation;
using Domain.Account.Services.Identity;
using ERP.Application.Repositories.Account;
using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Repositories.Inventory;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.Entries;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Application.Services.Identity;
using ERP.Application.Services.Inventory;
using ERP.Infrastracture.Repositories.Account;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.BaseRepositories;
using ERP.Infrastracture.Repositories.Inventory;
using ERP.Infrastracture.Services.Account;
using ERP.Infrastracture.Services.Account.Entries;
using ERP.Infrastracture.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.Inventory;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastracture.Utilities;

public static class InfrasctructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseSettingService<,,>), typeof(BaseSettingService<,,>));
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
        services.AddScoped<ExportDataToSeed>();
        services.AddScoped<ImportDataToSeed>();

        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
        services.AddScoped<ICashInBoxService, CashInBoxService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IFixedAssetService, FixedAssetService>();
        services.AddScoped<ICostCenterService, CostCenterService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IComplexEntryService, ComplexEntryService>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IOpeningEntryService, OpeningEntryService>();
        services.AddScoped<IJournalEntryService, JournalEntryService>();
        services.AddScoped<IPaymentEntryService, PaymentEntryService>();
        services.AddScoped<IReceiptEntryService, ReceiptEntryService>();
        services.AddScoped<ICompinedEntryService, CompinedEntryService>();
        services.AddScoped<ICollectionBookService, CollectionBookService>();
        services.AddScoped<IPackingUnitService, PackingUnitService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISellingPriceService, SellingPriceService>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
        services.AddScoped<ICashInBoxRepository, CashInBoxRepository>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<IPackingUnitRepository, PackingUnitRepository>();
        services.AddScoped<ISellingPriceRepository, SellingPriceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHttpContextAccessor();
    }
}