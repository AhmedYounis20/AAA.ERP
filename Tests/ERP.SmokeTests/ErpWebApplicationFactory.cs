using Domain.Account.DBConfiguration.DbContext;
using Inventory.Application.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace ERP.SmokeTests;

public class ErpWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase($"ErpSmokeTests_{Guid.NewGuid()}");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // ItemRepository implementation is commented out in Infrastructure; stub for host startup validation.
            services.RemoveAll<IItemRepository>();
            services.AddScoped(_ => new Mock<IItemRepository>().Object);
        });
    }
}
