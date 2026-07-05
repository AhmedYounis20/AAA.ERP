using Inventory.Application.Repositories;
using ERP.Infrastructure.Repositories;
using ERP.Infrastructure.Repositories.BaseRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Infrastructure.Utilities;

public static class InfrastructureExtensions
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
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITransactionCoordinator, TransactionCoordinator>();
        services.AddHttpContextAccessor();
    }
}
