using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Repositories.BaseRepositories;
using Shared.BaseEntities.Identity;

namespace ERP.Infrastracture.Repositories.Inventory;

public class ItemRepository : BaseTreeSettingRepository<Item>, IItemRepository
{
    private readonly IApplicationDbContext _context;
    public ItemRepository(IApplicationDbContext context) : base(context) => _context = context;


    public async Task<IEnumerable<ItemDto>> GetDtos()
    {
        var items = await (from item in _context.Set<Item>()
                           select new ItemDto
                           {
                               Id = item.Id,
                               ParentId = item.ParentId,
                               ConditionalDiscount = item.ConditionalDiscount,
                               CreatedAt = item.CreatedAt,
                               CountryOfOrigin = item.CountryOfOrigin,
                               DefaultDiscount = item.DefaultDiscount,
                               ModifiedAt = item.ModifiedAt,
                               ItemCodes = _context.Set<ItemCode>().Where(e => e.ItemId == item.Id).ToList(),
                               DefaultDiscountType = item.DefaultDiscountType,
                               Model = item.Model,
                               Name = item.Name,
                               NameSecondLanguage = item.NameSecondLanguage,
                               IsDiscountBasedOnSellingPrice = item.IsDiscountBasedOnSellingPrice,
                               ItemType = item.ItemType,
                               MaxDiscount = item.MaxDiscount,
                               NodeType = item.NodeType,
                               Version = item.Version,
                               ApplyDomainChanges = item.ApplyDomainChanges
                           }).OrderBy(e=>e.CreatedAt).ToListAsync();

        return items;
    }
    

    public async Task<IEnumerable<ItemDto>> GetVariants()
    {
        var items = await (from item in _context.Set<Item>()
                           let hasSubDomains = _context.Set<Item>().Any(e => e.ParentId == item.Id && e.NodeType == NodeType.SubDomain)
                               let itemPackingUnits = _context.Set<ItemPackingUnit>()
                                .Include(e => e.ItemPackingUnitSellingPrices)
                                .Where(e => e.ItemId == item.Id)
                                .Select(e => new ItemPackingUnitDto
                                {
                                    PackingUnitId = e.PackingUnitId,
                                    Name = _context.Set<PackingUnit>().Where(pu => pu.Id == e.PackingUnitId).Select(pu => pu.Name).FirstOrDefault(),
                                    AverageCostPrice = e.AverageCostPrice,
                                    IsDefaultPackingUnit = e.IsDefaultPackingUnit,
                                    IsDefaultPurchases = e.IsDefaultPurchases,
                                    IsDefaultSales = e.IsDefaultSales,
                                    LastCostPrice = e.LastCostPrice,
                                    PartsCount = e.PartsCount,
                                    OrderNumber = e.OrderNumber,
                                    SellingPrices = e.ItemPackingUnitSellingPrices.Select(price => new ItemPackingUnitSellingPriceDto
                                    {
                                        SellingPriceId = price.SellingPriceId,
                                        Amount = price.Amount,
                                    }).ToList(),
                                }).OrderBy(e => e.OrderNumber).ToList()
                            let stockBalances = _context.Set<StockBalance>()
                                .Where(sb => sb.ItemId == item.Id)
                                .Select(sb => new ItemStockBalanceDto
                                {
                                    BranchId = sb.BranchId,
                                    PackingUnitId = sb.PackingUnitId,
                                    CurrentBalance = sb.CurrentBalance
                                }).ToList()

                           select new ItemDto
                           {
                               Id = item.Id,
                               ParentId = item.ParentId,
                               ConditionalDiscount = item.ConditionalDiscount,
                               CreatedAt = item.CreatedAt,
                               CountryOfOrigin = item.CountryOfOrigin,
                               DefaultDiscount = item.DefaultDiscount,
                               ModifiedAt = item.ModifiedAt,
                               ItemCodes = _context.Set<ItemCode>().Where(e => e.ItemId == item.Id).ToList(),
                               DefaultDiscountType = item.DefaultDiscountType,
                               Model = item.Model,
                               Name = item.Name,
                               NameSecondLanguage = item.NameSecondLanguage,
                               IsDiscountBasedOnSellingPrice = item.IsDiscountBasedOnSellingPrice,
                               ItemType = item.ItemType,
                               MaxDiscount = item.MaxDiscount,
                               NodeType = item.NodeType,
                               Version = item.Version,
                               ApplyDomainChanges = item.ApplyDomainChanges,
                               HasSubDomains = hasSubDomains,
                                PackingUnits = itemPackingUnits,
                                StockBalances = stockBalances,
                           }).Where(e=>e.NodeType == NodeType.SubDomain || (!e.HasSubDomains && e.NodeType == NodeType.Domain)).OrderBy(e=>e.CreatedAt).ToListAsync();

        return items;
    }

    public async Task<ItemDto?> GetDtoById(Guid id)
    {
        var result = await (from item in _context.Set<Item>()
                            .Include(e => e.ItemSuppliers)
                            .Include(e => e.ItemManufacturerCompanies)
                            let itemPackingUnits = _context.Set<ItemPackingUnit>()
                                .Include(e => e.ItemPackingUnitSellingPrices)
                                .Where(e => e.ItemId == item.Id)
                                .Select(e => new ItemPackingUnitDto
                                {
                                    PackingUnitId = e.PackingUnitId,
                                    Name = _context.Set<PackingUnit>().Where(pu => pu.Id == e.PackingUnitId).Select(pu => pu.Name).FirstOrDefault(),
                                    AverageCostPrice = e.AverageCostPrice,
                                    IsDefaultPackingUnit = e.IsDefaultPackingUnit,
                                    IsDefaultPurchases = e.IsDefaultPurchases,
                                    IsDefaultSales = e.IsDefaultSales,
                                    LastCostPrice = e.LastCostPrice,
                                    PartsCount = e.PartsCount,
                                    OrderNumber = e.OrderNumber,
                                    SellingPrices = e.ItemPackingUnitSellingPrices.Select(price => new ItemPackingUnitSellingPriceDto
                                    {
                                        SellingPriceId = price.SellingPriceId,
                                        Amount = price.Amount,
                                    }).ToList(),
                                }).OrderBy(e => e.OrderNumber).ToList()
                            let stockBalances = _context.Set<StockBalance>()
                                .Where(sb => sb.ItemId == item.Id)
                                .Select(sb => new ItemStockBalanceDto
                                {
                                    BranchId = sb.BranchId,
                                    PackingUnitId = sb.PackingUnitId,
                                    CurrentBalance = sb.CurrentBalance
                                }).ToList()

                            select new ItemDto
                            {
                                Id = item.Id,
                                ParentId = item.ParentId,
                                ConditionalDiscount = item.ConditionalDiscount,
                                CreatedAt = item.CreatedAt,
                                CountryOfOrigin = item.CountryOfOrigin,
                                DefaultDiscount = item.DefaultDiscount,
                                ModifiedAt = item.ModifiedAt,
                                ItemCodes = _context.Set<ItemCode>().Where(e => e.ItemId == item.Id).ToList(),
                                DefaultDiscountType = item.DefaultDiscountType,
                                Model = item.Model,
                                Name = item.Name,
                                NameSecondLanguage = item.NameSecondLanguage,
                                IsDiscountBasedOnSellingPrice = item.IsDiscountBasedOnSellingPrice,
                                ItemType = item.ItemType,
                                MaxDiscount = item.MaxDiscount,
                                NodeType = item.NodeType,
                                Version = item.Version,
                                SuppliersIds = item.ItemSuppliers.Select(e => e.SupplierId).ToList(),
                                ManufacturerCompaniesIds = item.ItemManufacturerCompanies.Select(e => e.ManufacturerCompanyId).ToList(),
                                SellingPriceDiscounts = _context.Set<ItemSellingPriceDiscount>()
                                .Where(e => e.ItemId == item.Id).Select(e => new ItemSellingPriceDiscountDto
                                {
                                    SellingPriceId = e.SellingPriceId,
                                    Discount = e.Discount,
                                    DiscountType = e.DiscountType
                                }).ToList(),
                                PackingUnits = itemPackingUnits,
                                StockBalances = stockBalances,
                                ApplyDomainChanges = item.ApplyDomainChanges

                            }).FirstOrDefaultAsync(e => e.Id == id);

        return result;
    }



}