using Accounting.Infrastructure;
using Inventory.Infrastructure;
using ERP.Infrastructure.DBConfiguration.DbContext;
using ERP.BuildingBlocks.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Platform.Audit.Infrastructure;

namespace ERP.Host.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        ModuleModelConfigurationRegistry.Clear();
        ModuleModelConfigurationRegistry.Register(AccountingModuleExtensions.ApplyAccountingModuleConfigurations);
        ModuleModelConfigurationRegistry.Register(InventoryModuleExtensions.ApplyInventoryModuleConfigurations);
        ModuleModelConfigurationRegistry.Register(PlatformAuditModuleExtensions.ApplyPlatformAuditConfigurations);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultDbConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultDbConnection' was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("ERP.Host"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
