using AAA.ERP.DBConfiguration.Config.Currencies;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Models.Data.Currencies;
using AAA.ERP.Models.Data.Identity;
using AAA.ERP.Utility;

namespace AAA.ERP.DBConfiguration.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new AccountGuideDbConfig())
               .ApplyConfiguration(new CurrencyDbConfig());
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = SD.Role_Admin,
                NormalizedName = SD.Role_Admin.ToUpper(),
            }
            );
    }


    protected void ApplyCreateUpdateTime()
    {
        var entries = ChangeTracker
        .Entries()
        .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
                ((BaseEntity)entityEntry.Entity).ModifiedAt = DateTime.Now;
            else if (entityEntry.State == EntityState.Added)
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
        }
    }
    public override int SaveChanges()
    {
        ApplyCreateUpdateTime();
        return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyCreateUpdateTime();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyCreateUpdateTime();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyCreateUpdateTime();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}