using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Domain.Models.Entities.Colors;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.PackingUnits;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Models.Entities.StockBalances;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Attachments;
using Shared.Domain.BaseEntities;

namespace Inventory.Infrastructure.Data;

public class InventoryDbContext : DbContext, IInventoryDbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure schema
        builder.HasDefaultSchema("Inventory");
        
        // Apply configurations
        builder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }

    // Inventory Entities
    public DbSet<AttributeDefinition> AttributeDefinitions { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<PackingUnit> PackingUnits { get; set; }
    public DbSet<ManufacturerCompany> ManufacturerCompanies { get; set; }
    public DbSet<SellingPrice> SellingPrices { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemCode> ItemCodes { get; set; }
    public DbSet<ItemSupplier> ItemSuppliers { get; set; }
    public DbSet<ItemManufacturerCompany> ItemManufacturerCompanies { get; set; }
    public DbSet<ItemSellingPriceDiscount> ItemSellingPriceDiscounts { get; set; }
    public DbSet<ItemPackingUnit> ItemPackingUnits { get; set; }
    public DbSet<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPrices { get; set; }
    public DbSet<StockBalance> StockBalances { get; set; }
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
    public DbSet<InventoryTransactionItem> InventoryTransactionItems { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
}
