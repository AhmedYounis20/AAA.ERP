using ERP.Infrastructure.DBConfiguration.Config.Identity;
using Identity.Application;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shared.BaseEntities.Identity;
using Inventory.Domain.Models.Entities.Attributes;

namespace ERP.Infrastructure.DBConfiguration.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> , IApplicationDbContext
{
    private readonly ICurrentUser? _currentUser;

    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public ApplicationDbContext(DbContextOptions options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        IdentitySchemaDbConfig.ApplyIdentitySchema(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var configure in ModuleModelConfigurationRegistry.Configurators)
            configure(builder);
    }

    private Guid? GetCurrentUserId() => _currentUser?.UserId;

    protected void ApplyAuditInfo()
    {
        var currentUserId = GetCurrentUserId();
        var currentTime = DateTime.UtcNow;

        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = currentTime;
                entity.CreatedBy = currentUserId;
            }

            // Always update ModifiedAt and ModifiedBy on any change
            entity.ModifiedAt = currentTime;
            entity.ModifiedBy = currentUserId;
        }
    }
    public override int SaveChanges()
    {
        ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInfo();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    // Attribute system DbSets
    public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.Products.ProductAttributeDefinition> ProductAttributeDefinitions { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.Variants.VariantAttributeValue> VariantAttributeValues { get; set; }
    
    // Product template system DbSets
    public DbSet<Inventory.Domain.Models.Entities.Products.ProductSellingPriceDiscount> ProductSellingPriceDiscounts { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.Products.ProductPackingUnit> ProductPackingUnits { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.Products.ProductPackingUnitSellingPrice> ProductPackingUnitSellingPrices { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.InventoryThresholds.InventoryThreshold> InventoryThresholds { get; set; }
    public DbSet<Inventory.Domain.Models.Entities.ExpiryLevels.ExpiryLevel> ExpiryLevels { get; set; }
}