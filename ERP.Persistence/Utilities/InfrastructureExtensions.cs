using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.Infrastructure.Repositories;
using ERP.BuildingBlocks.Persistence.Repositories;
using ERP.BuildingBlocks.Persistence.Services.Queries;
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
        services.AddScoped<ExportDataToSeed>();
        services.AddScoped<ImportDataToSeed>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped(typeof(IBaseQueryService<,>), typeof(BaseQueryService<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITransactionCoordinator, TransactionCoordinator>();
        services.AddHttpContextAccessor();
    }
}
