using ERP.BuildingBlocks.Application.Attachments;
using ERP.BuildingBlocks.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Platform.Audit.Infrastructure;
using Platform.Caching.Infrastructure;
using Platform.Infrastructure.Persistence.Attachments;
using Platform.Infrastructure.Services;
using Platform.Localization;

namespace Platform.Infrastructure;

public static class PlatformModuleExtensions
{
    public static IServiceCollection AddPlatformModules(this IServiceCollection services)
    {
        ModuleModelConfigurationRegistry.Register(ApplyPlatformAttachmentConfigurations);
        services.AddScoped<IAttachmentStore, DatabaseAttachmentStore>();
        services.AddPlatformAudit();
        services.AddPlatformCaching();
        services.AddPlatformLocalization();
        return services;
    }

    public static void ApplyPlatformAttachmentConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AttachmentDbConfig());
    }
}
