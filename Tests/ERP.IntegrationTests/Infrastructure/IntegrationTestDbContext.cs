using Domain.Account.DBConfiguration.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ERP.IntegrationTests.Infrastructure;

/// <summary>
/// Strips SQL Server column types so <see cref="DbContext.Database.EnsureCreated"/> works on SQLite.
/// </summary>
public sealed class IntegrationTestDbContext : ApplicationDbContext
{
    public IntegrationTestDbContext(DbContextOptions<IntegrationTestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        StripSqlServerColumnTypes(builder);
    }

    private static void StripSqlServerColumnTypes(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var columnType = property.GetColumnType();
                if (columnType is null)
                    continue;

                if (columnType.StartsWith("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                    columnType.StartsWith("decimal", StringComparison.OrdinalIgnoreCase) ||
                    columnType.StartsWith("datetime2", StringComparison.OrdinalIgnoreCase))
                {
                    property.SetColumnType(null);
                }
            }
        }
    }
}
