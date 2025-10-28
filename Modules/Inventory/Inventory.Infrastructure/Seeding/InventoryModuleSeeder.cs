using Inventory.Domain.Models.Entities.Colors;
using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.PackingUnits;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Models.Entities.StockBalances;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Application.Seeding;
using Shared.Domain.Attachments;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inventory.Infrastructure.Seeding;

public class InventoryModuleSeeder : IModuleSeeder
{
    private readonly InventoryDbContext _db;
    private readonly ILogger<InventoryModuleSeeder> _logger;
    public string Name => "Inventory";

    public InventoryModuleSeeder(InventoryDbContext db, ILogger<InventoryModuleSeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
        var basePath = Path.Combine(AppContext.BaseDirectory, "Seeding", "Data");

        await using var trx = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await UpsertTable<Color>(basePath, "Colors.json", cancellationToken);
            await UpsertTable<Size>(basePath, "Sizes.json", cancellationToken);
            await UpsertTable<PackingUnit>(basePath, "PackingUnits.json", cancellationToken);
            await UpsertTable<ManufacturerCompany>(basePath, "ManufacturerCompanies.json", cancellationToken);
            await UpsertTable<SellingPrice>(basePath, "SellingPrices.json", cancellationToken);
            await UpsertTable<Item>(basePath, "Items.json", cancellationToken);
            await UpsertTable<ItemCode>(basePath, "ItemCodes.json", cancellationToken);
            await UpsertTable<ItemSupplier>(basePath, "ItemSuppliers.json", cancellationToken);
            await UpsertTable<ItemManufacturerCompany>(basePath, "ItemManufacturerCompanies.json", cancellationToken);
            await UpsertTable<ItemPackingUnit>(basePath, "ItemPackingUnits.json", cancellationToken);
            await UpsertTable<ItemPackingUnitSellingPrice>(basePath, "ItemPackingUnitSellingPrices.json", cancellationToken);
            await UpsertTable<ItemSellingPriceDiscount>(basePath, "ItemSellingPriceDiscounts.json", cancellationToken);
            await UpsertTable<StockBalance>(basePath, "StockBalances.json", cancellationToken);
            await UpsertTable<Attachment>(basePath, "Attachments.json", cancellationToken);
            await _db.SaveChangesAsync();
            await trx.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Inventory module seeding failed");
            await trx.RollbackAsync(cancellationToken);
            throw;
        }
    }
    private async Task UpsertTable<TEntity>(string basePath, string jsonFile, CancellationToken ct) where TEntity : BaseEntity
    {
        var fullPath = Path.Combine(basePath, jsonFile);
        if (!File.Exists(fullPath))
            return;
        var json = await File.ReadAllTextAsync(fullPath);
        List<TEntity> entities = JsonConvert.DeserializeObject<List<TEntity>>(json) ?? new();

        try
        {
            foreach (TEntity entity in entities)
            {
                TEntity? dbEntity = _db.Set<TEntity>().Where(e => e.Id == entity.Id).FirstOrDefault();
                if (dbEntity != null)
                    _db.Update(entity);
                else
                    _db.Add(entity);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
    }
}


