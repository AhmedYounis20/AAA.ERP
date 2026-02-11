using Domain.Account.Models.Dtos.Attachments;
using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.Repositories.BaseRepositories;
using Shared.DTOs;
using System.Linq.Expressions;

namespace ERP.Infrastracture.Repositories.Inventory;

public class VariantRepository : BaseRepository<Variant>, IVariantRepository
{
    private readonly IApplicationDbContext _context;
    public VariantRepository(IApplicationDbContext context) : base(context) => _context = context;

    public async Task<IEnumerable<VariantDto>> GetDtos()
    {
        // Reuse the same mapping logic as pagination but without paging
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
                .ThenInclude(p => p.ProductAttachments)
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
        query = query.Where(v =>
            !v.IsDefault || !_context.Set<Variant>().Any(e => e.ProductId == v.ProductId && !e.IsDefault));
        
        var paginatedVariants = await query.ToPaginatedResultAsync(pageNumber, pageSize, cancellationToken);

        // Build DTOs in memory (all data already loaded for the current page)
        return paginatedVariants.Map(variant =>
        {
            var product = variant.Product;

            // Get primary variant picture from loaded attachments
            var variantPicture = variant.VariantAttachments
                .FirstOrDefault(va => va.IsPrimary);

            // Get primary product picture from loaded product attachments
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
                VariantPicture = (vPic != null) ? new AttachmentDto()
                {
                    FileContent = Convert.ToBase64String(vPic.FileData),
                    ContentType = vPic.FileContentType,
                    FileName = vPic.FileName ?? string.Empty,
                } : ((pPic != null) ? new AttachmentDto()
                {
                    FileContent = Convert.ToBase64String(pPic.FileData),
                    ContentType = pPic.FileContentType,
                    FileName = pPic.FileName ?? string.Empty,
                } : null),
                VariantAttributeValues = variant.VariantAttributeValues
                    .Select(va => new VariantAttributeValueDto
                    {
                        Id = va.Id,
                        VariantId = va.VariantId,
                        AttributeDefinitionId = va.AttributeDefinitionId,
                        AttributeValue = va.AttributeValue == null ? null : va.AttributeValue.Adapt<AttributeValueDto>(),
                        AttributeDefinition = va.AttributeDefinition == null ? null : va.AttributeDefinition.Adapt<AttributeDefinitionDto>(),
                        AttributeValueId = va.AttributeValueId,
                        CreatedAt = va.CreatedAt,
                        ModifiedAt = va.ModifiedAt
                    }).ToList()
            };
        });
    }

    public async Task<VariantDto?> GetDtoById(Guid id)
    {
        // Load variant with all related data in one database trip
        var variant = await _context.Set<Variant>()
            .AsNoTracking()
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

        var dto = new VariantDto
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
                .Select(va => new VariantAttributeValueDto
                {
                    Id = va.Id,
                    VariantId = va.VariantId,
                    AttributeDefinitionId = va.AttributeDefinitionId,
                    AttributeValueId = va.AttributeValueId,
                    AttributeValue = va.AttributeValue == null ? null : va.AttributeValue.Adapt<AttributeValueDto>(),
                    AttributeDefinition = va.AttributeDefinition == null ? null : va.AttributeDefinition.Adapt<AttributeDefinitionDto>(),
                    CreatedAt = va.CreatedAt,
                    ModifiedAt = va.ModifiedAt
                })
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

        return dto;
    }

    public async Task<IEnumerable<VariantDto>> GetVariantsByProductId(Guid productId)
    {
        var variants = await (from variant in _context.Set<Variant>()
                             join product in _context.Set<Product>() on variant.ProductId equals product.Id
                             where variant.ProductId == productId
                             select new VariantDto
                             {
                                 Id = variant.Id,
                                 Name = product.Name,
                                 NameSecondLanguage = product.NameSecondLanguage,
                                 Description = variant.Description,
                                 ProductId = variant.ProductId,
                                 ProductName = product.Name ?? string.Empty,
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
                                 VariantCodes = _context.Set<VariantCode>().Where(e => e.VariantId == variant.Id).ToList()
                             }).ToListAsync();

        return variants;
    }

    public async Task<IEnumerable<VariantDto>> GetVariantsWithAttributes()
    {
        var variants = await (from variant in _context.Set<Variant>()
                             join product in _context.Set<Product>() on variant.ProductId equals product.Id
                             select new VariantDto
                             {
                                 Id = variant.Id,
                                 Name = product.Name,
                                 NameSecondLanguage = product.NameSecondLanguage,
                                 Description = variant.Description,
                                 ProductId = variant.ProductId,
                                 ProductName = product.Name ?? string.Empty,
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
                                VariantAttributeValues = _context.Set<VariantAttributeValue>()
                                    .Where(va => va.VariantId == variant.Id)
                                    .Select(va => new VariantAttributeValueDto
                                    {
                                        Id = va.Id,
                                        VariantId = va.VariantId,
                                        AttributeDefinitionId = va.AttributeDefinitionId,
                                        AttributeValueId = va.AttributeValueId,
                                        CreatedAt = va.CreatedAt,
                                        ModifiedAt = va.ModifiedAt
                                    }).ToList()
                             }).ToListAsync();

        return variants;
    }

    public async Task<VariantDto?> GetVariantWithAttributes(Guid id)
    {
        var variant = await (from v in _context.Set<Variant>()
                            join product in _context.Set<Product>() on v.ProductId equals product.Id
                            where v.Id == id
                            select new VariantDto
                            {
                                Id = v.Id,
                                Name = product.Name,
                                NameSecondLanguage = product.NameSecondLanguage,
                                Description = v.Description,
                                ProductId = v.ProductId,
                                ProductName = product.Name ?? string.Empty,
                                MaxDiscount = v.MaxDiscount,
                                ConditionalDiscount = v.ConditionalDiscount,
                                DefaultDiscount = v.DefaultDiscount,
                                DefaultDiscountType = v.DefaultDiscountType,
                                IsDiscountBasedOnSellingPrice = v.IsDiscountBasedOnSellingPrice,
                                Model = v.Model,
                                Version = v.Version,
                                CountryOfOrigin = v.CountryOfOrigin,
                                CreatedAt = v.CreatedAt,
                                ModifiedAt = v.ModifiedAt,
                                VariantAttributeValues = _context.Set<VariantAttributeValue>()
                                    .Where(va => va.VariantId == v.Id)
                                    .Select(va => new VariantAttributeValueDto
                                    {
                                        Id = va.Id,
                                        VariantId = va.VariantId,
                                        AttributeValueId = va.AttributeValueId,
                                        CreatedAt = va.CreatedAt,
                                        ModifiedAt = va.ModifiedAt
                                    }).ToList()
                            }).FirstOrDefaultAsync();

        return variant;
    }

    public async Task<IEnumerable<VariantDto>> SearchVariants(string searchTerm)
    {
        var variants = await (from variant in _context.Set<Variant>()
                             join product in _context.Set<Product>() on variant.ProductId equals product.Id
                             where
                                 // variant.Name.Contains(searchTerm) || 
                                 //   (variant.NameSecondLanguage != null && variant.NameSecondLanguage.Contains(searchTerm)) ||
                                   product.Name.Contains(searchTerm) ||
                                   (product.NameSecondLanguage != null && product.NameSecondLanguage.Contains(searchTerm))
                             select new VariantDto
                             {
                                 Id = variant.Id,
                                 Name = product.Name,
                                 NameSecondLanguage = product.NameSecondLanguage,
                                 Description = variant.Description,
                                 ProductId = variant.ProductId,
                                 ProductName = product.Name ?? string.Empty,
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
                                 VariantCodes = _context.Set<VariantCode>().Where(e => e.VariantId == variant.Id).ToList()
                             }).ToListAsync();

        return variants;
    }

    public async Task<List<Variant>> GetByProductIdAsync(Guid productId)
    {
        return await _dbSet
            .Include(e=>e.VariantCodes)
            .Where(v => v.ProductId == productId)
            .ToListAsync();
    }
}
