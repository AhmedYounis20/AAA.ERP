using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Services;
using Shared.Domain.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.BaseServices;
using Shared.Application.ModuleRegistration;
using Shared.Application.Seeding;
using Inventory.Infrastructure.Seeding;

namespace Inventory.Infrastructure.ModuleRegistration;

public class InventoryModuleRegistration : IModuleRegistration
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register Repositories
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IAttributeDefinitionRepository, AttributeDefinitionRepository>();
        services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
        services.AddScoped<IInventoryTransferRepository, InventoryTransferRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IManufacturerCompanyRepository, ManufacturerCompanyRepository>();
        services.AddScoped<IPackingUnitRepository, PackingUnitRepository>();
        services.AddScoped<ISellingPriceRepository, SellingPriceRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IStockBalanceRepository, StockBalanceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        // Register Services
        services.AddScoped(typeof(IBaseSettingService<,,>), typeof(BaseSettingService<,,>));
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
        //services.AddScoped(typeof(IBaseQueryService<,>), typeof(BaseQueryService<,>));
        services.AddScoped<IAttributeDefinitionService, AttributeDefinitionService>();
        services.AddScoped<IAttributeValueService, AttributeValueService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
        services.AddScoped<IInventoryTransferService, InventoryTransferService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IManufacturerCompanyService, ManufacturerCompanyService>();
        services.AddScoped<IPackingUnitService, PackingUnitService>();
        services.AddScoped<ISellingPriceService, SellingPriceService>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IStockBalanceService, StockBalanceService>();
        services.AddScoped<IExportTransactionService, ExportTransactionService>();
        services.AddScoped<IImportTransactionService, ImportTransactionService>();
    }

    public void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
        {

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Inventory.Infrastructure"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddScoped<IInventoryDbContext,InventoryDbContext>();
    }

    public void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(InventoryModuleRegistration).Assembly);
        });
    }

    public void RegisterControllers(IServiceCollection services)
    {
        // Controllers will be registered automatically by ASP.NET Core
        // when they are in the same assembly or referenced assemblies
        services.AddScoped<IModuleSeeder, InventoryModuleSeeder>();
    }
}
