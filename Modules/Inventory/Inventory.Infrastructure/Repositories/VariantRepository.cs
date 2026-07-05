using Domain.Account.Models.Dtos.Attachments;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Entities.Products;
using ERP.Infrastructure.Repositories.BaseRepositories;
using Shared.DTOs;
using System.Linq.Expressions;

namespace Inventory.Infrastructure.Repositories;

public class VariantRepository : BaseRepository<Variant>, IVariantRepository
{
    private readonly IApplicationDbContext _context;
    public VariantRepository(IApplicationDbContext context) : base(context) => _context = context;

    /// <summary>
    /// Full variant list — intentionally heavy; delegates to paginated query with max page size.
    /// Prefer GET /api/Variants/paginated for large catalogs.
    /// </summary>
    public async Task<IEnumerable<VariantDto>> GetDtos()
    {
        var paginated = await GetPaginatedDtos(
            pageNumber: 1,
            pageSize: int.MaxValue,
            filter: e => !e.IsArchived);

        return paginated.Items;
    }

    public async Task<PaginatedResult<VariantDto>> GetPaginatedDtos(
        int pageNumber,
        int pageSize,
        Expression<Func<Variant, bool>>? filter = null,
        Expression<Func<Variant, object>>? orderBy = null,
        bool descending = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Variant> query = _context.Set<Variant>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(v => v.Product)
                .ThenInclude(p => p!.ProductAttachments)
                    .ThenInclude(pa => pa.Attachment)
            .Include(v => v.VariantCodes)
            .Include(v => v.VariantAttachments)
                .ThenInclude(e => e.Attachment)
            .Include(v => v.VariantAttributeValues)
                .ThenInclude(vav => vav.AttributeDefinition)
            .Include(v => v.VariantAttributeValues)
                .ThenInclude(vav => vav.AttributeValue);

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        else
            query = query.OrderByDescending(e => e.CreatedAt);

        query = query.Where(v => !v.IsArchived);

        var paginatedVariants = await query.ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);

        return paginatedVariants.Map(MapVariantListDto);
    }

    public async Task<VariantDto?> GetDtoById(Guid id)
    {
        // Intentionally heavy: detail view loads full variant graph (packing units, taxes, attachments, etc.).
        var variant = await _context.Set<Variant>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(v => v.Product)
            .Include(v => v.VariantCodes)
            .Include(v => v.VariantPackingUnitPrices)
                .ThenInclude(e => e.VariantPackingUnitSellingPrices)
            .Include(v => v.VariantAttachments)
                .ThenInclude(e => e.Attachment)
            .Include(v => v.VariantAttributeValues)
                .ThenInclude(vav => vav.AttributeDefinition)
            .Include(v => v.VariantAttributeValues)
                .ThenInclude(vav => vav.AttributeValue)
            .Include(v => v.VariantSuppliers)
            .Include(v => v.VariantSalesTaxes)
            .Include(v => v.VariantPurchaseTaxes)
            .Include(v => v.VariantCostCenters)
            .Include(v => v.VariantManufacturerCompanies)
            .Include(v => v.VariantSellingPriceDiscounts)
            .Include(v => v.InventoryThresholds)
            .Include(v => v.ExpiryLevels)
            .Where(v => v.Id == id)
            .FirstOrDefaultAsync();

        if (variant == null)
            return null;

        var product = variant.Product;

        return new VariantDto
        {
            Id = variant.Id,
            Name = product?.Name ?? string.Empty,
            NameSecondLanguage = product?.NameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            ProductName = product?.Name ?? string.Empty,
            InventoryThresholdScope = variant.InventoryThresholdScope,
            InventoryThresholdBranchId = variant.InventoryThresholdBranchId,
            MaxDiscount = variant.MaxDiscount,
            ConditionalDiscount = variant.ConditionalDiscount,
            DefaultDiscount = variant.DefaultDiscount,
            DefaultDiscountType = variant.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = variant.IsDiscountBasedOnSellingPrice,
            Model = variant.Model,
            Version = variant.Version,
            CountryOfOrigin = variant.CountryOfOrigin,
            CreatedAt = variant.CreatedAt,
            ModifiedAt = variant.ModifiedAt,
            ProductType = product?.ProductType,
            VariantCodes = variant.VariantCodes,
            SuppliersIds = variant.VariantSuppliers
                .Select(vs => vs.SupplierId)
                .ToList(),
            SalesTaxIds = variant.VariantSalesTaxes
                .Select(vst => vst.TaxId)
                .ToList(),
            PurchaseTaxIds = variant.VariantPurchaseTaxes
                .Select(vpt => vpt.TaxId)
                .ToList(),
            CostCenters = variant.VariantCostCenters
                .Select(vcc => new VariantCostCenterDto
                {
                    CostCenterId = vcc.CostCenterId,
                    Percent = vcc.Percent
                })
                .ToList(),
            ManufacturerCompaniesIds = variant.VariantManufacturerCompanies
                .Select(vmc => vmc.ManufacturerCompanyId)
                .ToList(),
            SellingPriceDiscounts = variant.VariantSellingPriceDiscounts
                .Select(vspd => new VariantSellingPriceDiscountDto
                {
                    Id = vspd.Id,
                    VariantId = vspd.VariantId,
                    SellingPriceId = vspd.SellingPriceId,
                    Discount = vspd.Discount,
                    DiscountType = vspd.DiscountType,
                    CreatedAt = vspd.CreatedAt,
                    ModifiedAt = vspd.ModifiedAt
                })
                .ToList(),
            PackingUnits = variant.VariantPackingUnitPrices.Select(e => new VariantPackingUnitDto()
            {
                PackingUnitId = e.PackingUnitId,
                LastCostPrice = e.LastCostPrice,
                AverageCostPrice = e.AverageCostPrice,
                IsDefaultPackingUnit = e.IsDefaultPackingUnit,
                IsDefaultPurchases = e.IsDefaultPurchases,
                IsDefaultSales = e.IsDefaultSales,
                OrderNumber = e.OrderNumber,
                PartsCount = e.PartsCount,
                SellingPrices = e.VariantPackingUnitSellingPrices.Select(s => new VariantPackingUnitSellingPriceDto()
                {
                    Amount = s.Amount,
                    SellingPriceId = s.SellingPriceId
                }).ToList(),
            }).ToList(),
            VariantAttachments = variant.VariantAttachments
                .Select(va => new VariantAttachmentDto
                {
                    Id = va.Id,
                    VariantId = va.VariantId,
                    AttachmentId = va.AttachmentId,
                    AttachmentType = va.AttachmentType,
                    Description = va.Description,
                    IsPrimary = va.IsPrimary,
                    DisplayOrder = va.DisplayOrder,
                    CreatedAt = va.CreatedAt,
                    ModifiedAt = va.ModifiedAt,
                    Attachment = va.Attachment == null ? null : new AttachmentDto()
                    {
                        ContentType = va.Attachment.FileContentType,
                        AttachmentId = va.AttachmentId,
                        FileContent = Convert.ToBase64String(va.Attachment.FileData),
                        FileName = va.Attachment.FileName
                    }
                })
                .ToList(),
            VariantAttributeValues = variant.VariantAttributeValues
                .Select(MapVariantAttributeValueDto)
                .ToList(),
            InventoryThresholds = variant.InventoryThresholds
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
            ExpiryLevels = variant.ExpiryLevels
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
    }

    public async Task<IEnumerable<VariantDto>> GetVariantsByProductId(Guid productId)
    {
        var product = await _context.Set<Product>()
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .Select(p => new { p.Name, p.NameSecondLanguage })
            .FirstOrDefaultAsync();

        if (product == null)
            return Enumerable.Empty<VariantDto>();

        var variants = await _context.Set<Variant>()
            .AsNoTracking()
            .Include(v => v.VariantCodes)
            .Where(v => v.ProductId == productId)
            .ToListAsync();

        return variants.Select(variant => MapVariantSummaryDto(variant, product.Name, product.NameSecondLanguage));
    }

    /// <summary>
    /// Loads all variants with attribute values — intentionally heavy; prefer paginated list for UI grids.
    /// </summary>
    public async Task<IEnumerable<VariantDto>> GetVariantsWithAttributes()
    {
        var variants = await _context.Set<Variant>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(v => v.Product)
            .Include(v => v.VariantAttributeValues)
            .ToListAsync();

        return variants.Select(variant => new VariantDto
        {
            Id = variant.Id,
            Name = variant.Product?.Name ?? string.Empty,
            NameSecondLanguage = variant.Product?.NameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            ProductName = variant.Product?.Name ?? string.Empty,
            MaxDiscount = variant.MaxDiscount,
            ConditionalDiscount = variant.ConditionalDiscount,
            DefaultDiscount = variant.DefaultDiscount,
            DefaultDiscountType = variant.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = variant.IsDiscountBasedOnSellingPrice,
            Model = variant.Model,
            Version = variant.Version,
            CountryOfOrigin = variant.CountryOfOrigin,
            CreatedAt = variant.CreatedAt,
            ModifiedAt = variant.ModifiedAt,
            VariantAttributeValues = variant.VariantAttributeValues
                .Select(va => new VariantAttributeValueDto
                {
                    Id = va.Id,
                    VariantId = va.VariantId,
                    AttributeDefinitionId = va.AttributeDefinitionId,
                    AttributeValueId = va.AttributeValueId,
                    CreatedAt = va.CreatedAt,
                    ModifiedAt = va.ModifiedAt
                })
                .ToList()
        });
    }

    public async Task<VariantDto?> GetVariantWithAttributes(Guid id)
    {
        var variant = await _context.Set<Variant>()
            .AsNoTracking()
            .Include(v => v.Product)
            .Include(v => v.VariantAttributeValues)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (variant == null)
            return null;

        return new VariantDto
        {
            Id = variant.Id,
            Name = variant.Product?.Name ?? string.Empty,
            NameSecondLanguage = variant.Product?.NameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            ProductName = variant.Product?.Name ?? string.Empty,
            MaxDiscount = variant.MaxDiscount,
            ConditionalDiscount = variant.ConditionalDiscount,
            DefaultDiscount = variant.DefaultDiscount,
            DefaultDiscountType = variant.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = variant.IsDiscountBasedOnSellingPrice,
            Model = variant.Model,
            Version = variant.Version,
            CountryOfOrigin = variant.CountryOfOrigin,
            CreatedAt = variant.CreatedAt,
            ModifiedAt = variant.ModifiedAt,
            VariantAttributeValues = variant.VariantAttributeValues
                .Select(va => new VariantAttributeValueDto
                {
                    Id = va.Id,
                    VariantId = va.VariantId,
                    AttributeValueId = va.AttributeValueId,
                    CreatedAt = va.CreatedAt,
                    ModifiedAt = va.ModifiedAt
                })
                .ToList()
        };
    }

    public async Task<IEnumerable<VariantDto>> SearchVariants(string searchTerm)
    {
        var variants = await _context.Set<Variant>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(v => v.Product)
            .Include(v => v.VariantCodes)
            .Where(v =>
                v.Product != null &&
                (v.Product.Name.Contains(searchTerm) ||
                 (v.Product.NameSecondLanguage != null && v.Product.NameSecondLanguage.Contains(searchTerm))))
            .ToListAsync();

        return variants.Select(variant =>
            MapVariantSummaryDto(
                variant,
                variant.Product?.Name ?? string.Empty,
                variant.Product?.NameSecondLanguage));
    }

    public async Task<List<Variant>> GetByProductIdAsync(Guid productId)
    {
        return await _dbSet
            .Include(e => e.VariantCodes)
            .Where(v => v.ProductId == productId)
            .ToListAsync();
    }

    private static VariantDto MapVariantListDto(Variant variant)
    {
        var product = variant.Product;

        var variantPicture = variant.VariantAttachments
            .FirstOrDefault(va => va.IsPrimary);

        var productPicture = product?.ProductAttachments
            .FirstOrDefault(pa => pa.IsPrimary);

        var vPic = variantPicture?.Attachment;
        var pPic = productPicture?.Attachment;

        return new VariantDto
        {
            Id = variant.Id,
            Name = product?.Name ?? string.Empty,
            NameSecondLanguage = product?.NameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            ProductName = product?.Name ?? string.Empty,
            CreatedAt = variant.CreatedAt,
            ModifiedAt = variant.ModifiedAt,
            VariantCodes = variant.VariantCodes,
            VariantPicture = vPic != null ? new AttachmentDto
            {
                FileContent = Convert.ToBase64String(vPic.FileData),
                ContentType = vPic.FileContentType,
                FileName = vPic.FileName ?? string.Empty,
            } : pPic != null ? new AttachmentDto
            {
                FileContent = Convert.ToBase64String(pPic.FileData),
                ContentType = pPic.FileContentType,
                FileName = pPic.FileName ?? string.Empty,
            } : null,
            VariantAttributeValues = variant.VariantAttributeValues
                .Select(MapVariantAttributeValueDto)
                .ToList()
        };
    }

    private static VariantDto MapVariantSummaryDto(Variant variant, string productName, string? productNameSecondLanguage)
    {
        return new VariantDto
        {
            Id = variant.Id,
            Name = productName,
            NameSecondLanguage = productNameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            ProductName = productName,
            MaxDiscount = variant.MaxDiscount,
            ConditionalDiscount = variant.ConditionalDiscount,
            DefaultDiscount = variant.DefaultDiscount,
            DefaultDiscountType = variant.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = variant.IsDiscountBasedOnSellingPrice,
            Model = variant.Model,
            Version = variant.Version,
            CountryOfOrigin = variant.CountryOfOrigin,
            CreatedAt = variant.CreatedAt,
            ModifiedAt = variant.ModifiedAt,
            VariantCodes = variant.VariantCodes
        };
    }

    private static VariantAttributeValueDto MapVariantAttributeValueDto(VariantAttributeValue va)
    {
        return new VariantAttributeValueDto
        {
            Id = va.Id,
            VariantId = va.VariantId,
            AttributeDefinitionId = va.AttributeDefinitionId,
            AttributeValue = va.AttributeValue == null ? null : va.AttributeValue.Adapt<AttributeValueDto>(),
            AttributeDefinition = va.AttributeDefinition == null ? null : va.AttributeDefinition.Adapt<AttributeDefinitionDto>(),
            AttributeValueId = va.AttributeValueId,
            CreatedAt = va.CreatedAt,
            ModifiedAt = va.ModifiedAt
        };
    }
}
