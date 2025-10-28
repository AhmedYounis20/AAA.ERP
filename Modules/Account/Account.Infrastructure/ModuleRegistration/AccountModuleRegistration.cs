using Account.Application.Repositories;
using Account.Application.Services;
using Account.Application.Services.Queries;
using Account.Infrastructure.Data;
using Account.Infrastructure.Repositories;
using Account.Infrastructure.Repositories.SubLeadgers;
using Account.Infrastructure.Services;
using Account.Infrastructure.Services.Entries;
using Account.Infrastructure.Services.Queries;
using Account.Infrastructure.Services.SubLeadgers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.BaseServices;
using Shared.Application.ModuleRegistration;
using Shared.Application.Seeding;
using Account.Infrastructure.Seeding;

namespace Account.Infrastructure.ModuleRegistration;

public class AccountModuleRegistration : IModuleRegistration
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register Repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        
        // Register SubLedger Repositories
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<ICashInBoxRepository, CashInBoxRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Services
        services.AddScoped(typeof(IBaseSettingService<,,>), typeof(BaseSettingService<,,>));
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
        services.AddScoped(typeof(IBaseQueryService<,>), typeof(BaseQueryService<,>));

        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ICollectionBookService, CollectionBookService>();
        services.AddScoped<ICostCenterService, CostCenterService>();
        
        // Register Entry Services
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IComplexEntryService, ComplexEntryService>();
        services.AddScoped<ICompinedEntryService, CompinedEntryService>();
        services.AddScoped<IJournalEntryService, JournalEntryService>();
        services.AddScoped<IOpeningEntryService, OpeningEntryService>();
        services.AddScoped<IPaymentEntryService, PaymentEntryService>();
        services.AddScoped<IReceiptEntryService, ReceiptEntryService>();
        
        // Register SubLedger Services
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICashInBoxService, CashInBoxService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IFixedAssetService, FixedAssetService>();
        services.AddScoped<ISupplierService, SupplierService>();
    }

    public void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AccountDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Account.Infrastructure"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IAccountDbContext, AccountDbContext>();
    }

    public void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AccountModuleRegistration).Assembly);
        });
    }

    public void RegisterControllers(IServiceCollection services)
    {
        // Controllers will be registered automatically by ASP.NET Core
        // when they are in the same assembly or referenced assemblies
        services.AddScoped<IModuleSeeder, AccountModuleSeeder>();
    }
}
