using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

public class IdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure schema
        builder.HasDefaultSchema("Identity");
        
        // Apply configurations
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
