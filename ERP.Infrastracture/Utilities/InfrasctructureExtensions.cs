using AAA.ERP.Services.Impelementation;
using Domain.Account.Services.Identity;
using ERP.Application.Repositories.Account;
using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Repositories.Inventory;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.Entries;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Application.Services.Audit;
using ERP.Application.Services.Caching;
using ERP.Application.Services.Identity;
using ERP.Application.Services.Inventory;
using ERP.Infrastracture.Repositories;
using ERP.Infrastracture.Repositories.Account;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.BaseRepositories;
using ERP.Infrastracture.Repositories.Inventory;
using ERP.Infrastracture.Services.Account;
using ERP.Infrastracture.Services.Account.Entries;
using ERP.Infrastracture.Services.Account.Queries;
using ERP.Infrastracture.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.Audit;
using ERP.Infrastracture.Services.Caching;
using ERP.Infrastracture.Services.Inventory;
using ERP.Domain.Models.Entities.Inventory.ExpiryLevels;
using ERP.Domain.Models.Entities.Inventory.InventoryThresholds;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Products;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastracture.Utilities;

public static class InfrasctructureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InfrasctructureExtensions).Assembly));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseSettingService<,,>), typeof(BaseSettingService<,,>));
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
        services.AddScoped<ExportDataToSeed>();
        services.AddScoped<ImportDataToSeed>();

        services.AddScoped( typeof(IBaseQueryService<,>), typeof(BaseQueryService<,>));
        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
        services.AddScoped<ICashInBoxService, CashInBoxService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<ITaxService, TaxService>();
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
        services.AddScoped<IManufacturerCompanyService, ManufacturerCompanyService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IVariantService, VariantService>();
        services.AddScoped<IAttributeValueService, AttributeValueService>();
        services.AddScoped<IAttributeDefinitionService, AttributeDefinitionService>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
        services.AddScoped<IImportTransactionService, ImportTransactionService>();
        services.AddScoped<IExportTransactionService, ExportTransactionService>();
        services.AddScoped<IStockBalanceService, StockBalanceService>();
        services.AddScoped<IInventoryTransferService, InventoryTransferService>();
        // services.AddScoped<IProductVariantReportService, ProductVariantReportService>();
        // services.AddScoped<IInventoryTransferRepository, InventoryTransferRepository>();
        services.AddScoped<IAttributeDefinitionService, AttributeDefinitionService>();
        services.AddScoped<IAttributeValueService, AttributeValueService>();
        services.AddScoped<IInventoryTransferService, InventoryTransferService>();

        // Audit Service
        services.AddScoped<IAuditService, AuditService>();

        // Caching Service
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

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
        services.AddScoped<ITaxRepository, TaxRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<IPackingUnitRepository, PackingUnitRepository>();
        services.AddScoped<ISellingPriceRepository, SellingPriceRepository>();
        services.AddScoped<IManufacturerCompanyRepository, ManufacturerCompanyRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();
        services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
        services.AddScoped<IAttributeDefinitionRepository, AttributeDefinitionRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        
        // New Product/Variant related repositories
        services.AddScoped<IBaseRepository<ProductCode>, BaseRepository<ProductCode>>();
        services.AddScoped<IBaseRepository<ProductSupplier>, BaseRepository<ProductSupplier>>();
        services.AddScoped<IBaseRepository<ProductSalesTax>, BaseRepository<ProductSalesTax>>();
        services.AddScoped<IBaseRepository<ProductPurchaseTax>, BaseRepository<ProductPurchaseTax>>();
        services.AddScoped<IBaseRepository<ProductCostCenter>, BaseRepository<ProductCostCenter>>();
        services.AddScoped<IBaseRepository<ProductManufacturerCompany>, BaseRepository<ProductManufacturerCompany>>();
        services.AddScoped<IBaseRepository<ProductAttachment>, BaseRepository<ProductAttachment>>();
        services.AddScoped<IProductAttributeDefinitionRepository, ProductAttributeDefinitionRepository>();
        services.AddScoped<IBaseRepository<VariantCode>, BaseRepository<VariantCode>>();
        services.AddScoped<IBaseRepository<VariantSupplier>, BaseRepository<VariantSupplier>>();
        services.AddScoped<IBaseRepository<VariantSalesTax>, BaseRepository<VariantSalesTax>>();
        services.AddScoped<IBaseRepository<VariantPurchaseTax>, BaseRepository<VariantPurchaseTax>>();
        services.AddScoped<IBaseRepository<VariantCostCenter>, BaseRepository<VariantCostCenter>>();
        services.AddScoped<IBaseRepository<VariantManufacturerCompany>, BaseRepository<VariantManufacturerCompany>>();
        services.AddScoped<IBaseRepository<VariantPackingUnit>, BaseRepository<VariantPackingUnit>>();
        services.AddScoped<IBaseRepository<VariantPackingUnitSellingPrice>, BaseRepository<VariantPackingUnitSellingPrice>>();
        services.AddScoped<IBaseRepository<VariantSellingPriceDiscount>, BaseRepository<VariantSellingPriceDiscount>>();
        services.AddScoped<IBaseRepository<VariantAttachment>, BaseRepository<VariantAttachment>>();
        services.AddScoped<IVariantAttributeValueRepository, VariantAttributeValueRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
        // services.AddScoped<IBaseRepository<InventoryTransactionVariant>, BaseRepository<InventoryTransactionVariant>>();
        services.AddScoped<IStockBalanceRepository, StockBalanceRepository>();
        services.AddScoped<IBaseRepository<InventoryThreshold>, BaseRepository<InventoryThreshold>>();
        services.AddScoped<IBaseRepository<ExpiryLevel>, BaseRepository<ExpiryLevel>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IInventoryTransferRepository, InventoryTransferRepository>();
        services.AddScoped<IAttributeDefinitionRepository, AttributeDefinitionRepository>();
        services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
        services.AddHttpContextAccessor();
    }
}