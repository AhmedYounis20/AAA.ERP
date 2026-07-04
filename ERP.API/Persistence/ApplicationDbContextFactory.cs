using Accounting.Infrastructure;
using Domain.Account.DBConfiguration.DbContext;
using ERP.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ERP.API.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        ModuleModelConfigurationRegistry.Clear();
        ModuleModelConfigurationRegistry.Register(AccountingModuleExtensions.ApplyAccountingModuleConfigurations);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultDbConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultDbConnection' was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("ERP.API"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
