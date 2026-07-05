using Domain.Account.DBConfiguration.DbContext;
using ERP.BuildingBlocks.Persistence.Data;
using ERP.Application.Repositories.Inventory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace ERP.IntegrationTests.Infrastructure;

/// <summary>
/// Uses SQLite in-memory so <see cref="ERP.BuildingBlocks.Persistence.Repositories.IUnitOfWork"/> transactions
/// (rollback on GL failure) behave like production SQL Server.
/// </summary>
public sealed class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly SqliteConnection _connection;

    public IntegrationTestWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(DbContextOptions<IntegrationTestDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));
            services.RemoveAll(typeof(IntegrationTestDbContext));
            services.RemoveAll(typeof(IApplicationDbContext));

            services.AddDbContext<IntegrationTestDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            services.AddScoped<ApplicationDbContext>(sp => sp.GetRequiredService<IntegrationTestDbContext>());
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<IntegrationTestDbContext>());

            services.RemoveAll<IItemRepository>();
            services.AddScoped(_ => new Mock<IItemRepository>().Object);
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _connection.Dispose();

        base.Dispose(disposing);
    }
}
