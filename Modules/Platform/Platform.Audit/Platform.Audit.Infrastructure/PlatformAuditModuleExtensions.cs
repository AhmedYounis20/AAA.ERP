using ERP.Application.Data;
using ERP.Application.Services.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Platform.Audit.Infrastructure.Persistence;
using Platform.Audit.Infrastructure.Services;

namespace Platform.Audit.Infrastructure;

public static class PlatformAuditModuleExtensions
{
    public static IServiceCollection AddPlatformAudit(this IServiceCollection services)
    {
        ModuleModelConfigurationRegistry.Register(ApplyPlatformAuditConfigurations);
        services.AddScoped<IAuditService, AuditService>();
        return services;
    }

    public static void ApplyPlatformAuditConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditLogDbConfig());
    }
}
