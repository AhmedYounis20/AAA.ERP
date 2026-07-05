using Domain.Account.Models.Dtos.Attachments;
using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class ProductRepository : BaseTreeSettingRepository<Product>, IProductRepository
{
    private readonly IApplicationDbContext _context;
    public ProductRepository(IApplicationDbContext context) : base(context) => _context = context;

    /// <summary>
    /// Full product list for tree/grid views. Uses batched includes + grouped variant counts (no per-row queries).
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetDtos()
    {
        var products = await _context.Set<Product>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.ProductCodes)
            .Include(e => e.ProductAttachments)
                .ThenInclude(e => e.Attachment)
            .ToListAsync();

        if (products.Count == 0)
            return Enumerable.Empty<ProductDto>();

        var variantCounts = await GetNonDefaultVariantCountsAsync();

        return products.Select(product => MapProductListDto(product, variantCounts));
    }

    public async Task<ProductDto?> GetDtoById(Guid id)
    {
        // Intentionally heavy: detail view loads full product graph (variants, packing units, taxes, etc.).
        var product = await _context.Set<Product>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(e => e.ProductCodes)
            .Include(e => e.ProductPackingUnits)
                .ThenInclude(e => e.ProductPackingUnitSellingPrices)
            .Include(e => e.ProductAttachments)
                .ThenInclude(e => e.Attachment)
            .Include(e => e.ProductAttributeDefinitions)
            .Include(e => e.ProductSuppliers)
            .Include(e => e.ProductSalesTaxes)
            .Include(e => e.ProductPurchaseTaxes)
            .Include(e => e.ProductCostCenters)
            .Include(e => e.ProductManufacturerCompanies)
            .Include(e => e.ProductSellingPriceDiscounts)
            .Include(e => e.InventoryThresholds)
            .Include(e => e.ExpiryLevels)
            .Include(e => e.Variants)
                .ThenInclude(v => v.VariantAttributeValues)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (product == null)
            return null;

        var variants = product.Variants
            .Where(v => !v.IsDefault && v.VariantAttributeValues.Any())
            .ToList();

        var dto = new ProductDto
        {
            Id = product.Id,
            ParentId = product.ParentId,
            InventoryThresholdScope = product.InventoryThresholdScope,
            InventoryThresholdBranchId = product.InventoryThresholdBranchId,
            ConditionalDiscount = product.ConditionalDiscount,
            CreatedAt = product.CreatedAt,
            ModifiedAt = product.ModifiedAt,
            CountryOfOrigin = product.CountryOfOrigin,
            DefaultDiscount = product.DefaultDiscount,
            DefaultDiscountType = product.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
            MaxDiscount = product.MaxDiscount,
            Model = product.Model,
            Name = product.Name,
            NameSecondLanguage = product.NameSecondLanguage,
            ProductType = product.ProductType,
            NodeType = product.NodeType,
            Version = product.Version,
            SellingPriceDiscounts = product.ProductSellingPriceDiscounts
                .Select(e => e.Adapt<ProductSellingPriceDiscountDto>())
                .ToList(),
            ProductCodes = product.ProductCodes,
            ProductAttachments = product.ProductAttachments
                .Select(pa => new ProductAttachmentDto
                {
                    Id = pa.Id,
                    AttachmentType = pa.AttachmentType,
                    Description = pa.Description,
                    IsPrimary = pa.IsPrimary,
                    DisplayOrder = pa.DisplayOrder,
                    Attachment = pa.Attachment != null ? new AttachmentDto()
                    {
                        AttachmentId = pa.AttachmentId,
                        FileContent = Convert.ToBase64String(pa.Attachment.FileData),
                        ContentType = pa.Attachment.FileContentType,
                        FileName = pa.Attachment.FileName,
                    } : null
                })
                .ToList(),
            SuppliersIds = product.ProductSuppliers
                .Select(ps => ps.SupplierId)
                .ToList(),
            SalesTaxIds = product.ProductSalesTaxes
                .Select(pst => pst.TaxId)
                .ToList(),
            PurchaseTaxIds = product.ProductPurchaseTaxes
                .Select(ppt => ppt.TaxId)
                .ToList(),
            CostCenters = product.ProductCostCenters
                .Select(e => new ProductCostCenterDto
                {
                    CostCenterId = e.CostCenterId,
                    Percent = e.Percent
                })
                .ToList(),
            ManufacturerCompaniesIds = product.ProductManufacturerCompanies
                .Select(pmc => pmc.ManufacturerCompanyId)
                .ToList(),
            ProductAttributeDefinitions = product.ProductAttributeDefinitions
                .Select(e => new ProductAttributeDefinitionDto()
                {
                    AttributeDefinitionId = e.AttributeDefinitionId,
                    IsVariant = e.IsVariant,
                })
                .ToList(),
            VariantCombinations = variants.Select(v => new VariantCombinationDto
            {
                Name = product.Name,
                Id=v.Id,
                ApplyProductChanges = v.ApplyProductChanges,
                AttributeValues = v.VariantAttributeValues
                    .ToDictionary(
                        av => av.AttributeDefinitionId,
                        av => av.AttributeValueId
                    )
            }).ToList(),
            HasVariants = variants.Any(),
            VariantsCount = variants.Count,
            PackingUnits = product.ProductPackingUnits.Select(e => new ProductPackingUnitDto()
            {
                PackingUnitId = e.PackingUnitId,
                LastCostPrice = e.LastCostPrice,
                AverageCostPrice = e.AverageCostPrice,
                IsDefaultPackingUnit = e.IsDefaultPackingUnit,
                IsDefaultPurchases = e.IsDefaultPurchases,
                IsDefaultSales = e.IsDefaultSales,
                Name = e.Name,
                OrderNumber = e.OrderNumber,
                PartsCount = e.PartsCount,
                SellingPrices = e.ProductPackingUnitSellingPrices.Select(s => new ProductPackingUnitSellingPriceDto()
                {
                    Amount = s.Amount,
                    SellingPriceId = s.SellingPriceId
                }).ToList()
            }).ToList(),
            InventoryThresholds = product.InventoryThresholds
                .OrderBy(t => t.Level)
                .Select(t => new InventoryThresholdDto
                {
                    Level = t.Level,
                    MinQuantity = t.MinQuantity,
                    MaxQuantity = t.MaxQuantity,
                    Scope = t.Scope,
                    BranchId = t.BranchId,
                    EnableNotification = t.EnableNotification
                })
                .ToList(),
            ExpiryLevels = product.ExpiryLevels
                .OrderBy(e => e.Level)
                .Select(e => new ExpiryLevelDto
                {
                    Level = e.Level,
                    DaysFrom = e.DaysFrom,
                    DaysTo = e.DaysTo,
                    EnableNotification = e.EnableNotification
                })
                .ToList()
        };

        return dto;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType)
    {
        var query = _context.Set<Product>().AsNoTracking();

        if (productType.HasValue)
            query = query.Where(p => p.ProductType == productType.Value);

        var products = await query
            .AsSplitQuery()
            .Include(p => p.ProductCodes)
            .ToListAsync();

        if (products.Count == 0)
            return Enumerable.Empty<ProductDto>();

        var productIds = products.Select(p => p.Id).ToList();
        var variantCounts = await GetNonDefaultVariantCountsAsync(productIds);

        return products.Select(product =>
        {
            variantCounts.TryGetValue(product.Id, out var count);
            return new ProductDto
            {
                Id = product.Id,
                ParentId = product.ParentId,
                ConditionalDiscount = product.ConditionalDiscount,
                CreatedAt = product.CreatedAt,
                CountryOfOrigin = product.CountryOfOrigin,
                DefaultDiscount = product.DefaultDiscount,
                ModifiedAt = product.ModifiedAt,
                ProductCodes = product.ProductCodes,
                DefaultDiscountType = product.DefaultDiscountType,
                Model = product.Model,
                Name = product.Name,
                NameSecondLanguage = product.NameSecondLanguage,
                IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
                ProductType = product.ProductType,
                MaxDiscount = product.MaxDiscount,
                NodeType = product.NodeType,
                Version = product.Version,
                HasVariants = count > 0,
                VariantsCount = count
            };
        });
    }

    private async Task<Dictionary<Guid, int>> GetNonDefaultVariantCountsAsync(IReadOnlyList<Guid>? productIds = null)
    {
        var query = _context.Set<Variant>()
            .AsNoTracking()
            .Where(v => !v.IsDefault && !v.IsArchived);

        if (productIds is { Count: > 0 })
            query = query.Where(v => productIds.Contains(v.ProductId));

        return await query
            .GroupBy(v => v.ProductId)
            .Select(g => new { ProductId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ProductId, x => x.Count);
    }

    private static ProductDto MapProductListDto(Product product, IReadOnlyDictionary<Guid, int> variantCounts)
    {
        variantCounts.TryGetValue(product.Id, out var variantsCount);

        var primaryPicture = product.ProductAttachments
            .FirstOrDefault(pa => pa.IsPrimary);

        return new ProductDto
        {
            Id = product.Id,
            ParentId = product.ParentId,
            CreatedAt = product.CreatedAt,
            ModifiedAt = product.ModifiedAt,
            NameSecondLanguage = product.NameSecondLanguage,
            Name = product.Name,
            ProductType = product.ProductType,
            NodeType = product.NodeType,
            Version = product.Version,
            HasVariants = variantsCount > 0,
            VariantsCount = variantsCount,
            ProductCodes = product.ProductCodes,
            ProductPicture = primaryPicture?.Attachment != null ? new AttachmentDto
            {
                FileContent = Convert.ToBase64String(primaryPicture.Attachment.FileData),
                ContentType = primaryPicture.Attachment.FileContentType,
                FileName = primaryPicture.Attachment.FileName,
            } : null,
        };
    }
}
