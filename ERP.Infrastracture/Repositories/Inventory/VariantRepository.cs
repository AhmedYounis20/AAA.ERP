using Domain.Account.Models.Dtos.Attachments;
using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class VariantRepository : BaseRepository<Variant>, IVariantRepository
{
    private readonly IApplicationDbContext _context;
    public VariantRepository(IApplicationDbContext context) : base(context) => _context = context;

    public async Task<IEnumerable<VariantDto>> GetDtos()
    {
        var variants = await (from variant in _context.Set<Variant>().Where(e=> !e.IsArchived)
                             join product in _context.Set<Product>() on variant.ProductId equals product.Id
                             let variantPicture = _context.Set<VariantAttachment>().Include(e=>e.Attachment).FirstOrDefault(e=>e.IsPrimary && e.VariantId == variant.Id)
                             let productPicture = _context.Set<ProductAttachment>().Include(e=>e.Attachment).FirstOrDefault(e=>e.IsPrimary && e.ProductId == variant.ProductId)
                             let vPic = variantPicture != null ? variantPicture.Attachment : null
                             let pPic = productPicture != null ? productPicture.Attachment : null
                             select new VariantDto
                             {
                                 Id = variant.Id,
                                 Name = product.Name,
                                 NameSecondLanguage = product.NameSecondLanguage,
                                 Description = variant.Description,
                                 ProductId = variant.ProductId,
                                 ProductName = product.Name ?? string.Empty,
                                 CreatedAt = variant.CreatedAt,
                                 ModifiedAt = variant.ModifiedAt,
                                 VariantCodes = _context.Set<VariantCode>().Where(e => e.VariantId == variant.Id).ToList(),
                                 VariantPicture = (vPic != null) ? new AttachmentDto()
                                 {
                                     FileContent = Convert.ToBase64String(vPic.FileData),
                                     ContentType = vPic.FileContentType,
                                     FileName = vPic.FileName ?? string.Empty,
                                 } : ((pPic != null) ?
                                     new AttachmentDto()
                                 {
                                     FileContent = Convert.ToBase64String(pPic.FileData),
                                     ContentType = pPic.FileContentType,
                                     FileName = pPic.FileName ?? string.Empty,
                                 }:null),
                                 VariantAttributeValues = _context.Set<VariantAttributeValue>()
                                    .Include(e=>e.AttributeDefinition)
                                    .Include(e=>e.AttributeValue)
                                    .Where(va => va.VariantId == variant.Id)
                                    .Select(va => new VariantAttributeValueDto
                                    {
                                        Id = va.Id,
                                        VariantId = va.VariantId,
                                        AttributeDefinitionId = va.AttributeDefinitionId,
                                        AttributeValue = va.AttributeValue == null ? null: va.AttributeValue.Adapt<AttributeValueDto>(),
                                        AttributeDefinition = va.AttributeDefinition == null ? null: va.AttributeDefinition.Adapt<AttributeDefinitionDto>(),
                                        AttributeValueId = va.AttributeValueId,
                                        CreatedAt = va.CreatedAt,
                                        ModifiedAt = va.ModifiedAt
                                    }).ToList()
                             }).ToListAsync();

        return variants;
    }

    public async Task<VariantDto?> GetDtoById(Guid id)
    {
        var variant = await (from v in _context.Set<Variant>()
                .Include(e=>e.VariantPackingUnitPrices)
                .ThenInclude(e=>e.VariantPackingUnitSellingPrices)
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
                                ProductType = product.ProductType,
                                VariantCodes = _context.Set<VariantCode>().Where(e => e.VariantId == v.Id).ToList(),
                                SuppliersIds = _context.Set<VariantSupplier>()
                                    .Where(vs => vs.VariantId == v.Id)
                                    .Select(vs => vs.SupplierId).ToList(),
                                ManufacturerCompaniesIds = _context.Set<VariantManufacturerCompany>()
                                    .Where(vmc => vmc.VariantId == v.Id)
                                    .Select(vmc => vmc.ManufacturerCompanyId).ToList(),
                                SellingPriceDiscounts = _context.Set<VariantSellingPriceDiscount>()
                                    .Where(vspd => vspd.VariantId == v.Id)
                                    .Select(vspd => new VariantSellingPriceDiscountDto
                                    {
                                        Id = vspd.Id,
                                        VariantId = vspd.VariantId,
                                        SellingPriceId = vspd.SellingPriceId,
                                        Discount = vspd.Discount,
                                        DiscountType = vspd.DiscountType,
                                        CreatedAt = vspd.CreatedAt,
                                        ModifiedAt = vspd.ModifiedAt
                                    }).ToList(),
                                PackingUnits = v.VariantPackingUnitPrices.Select(e=> new VariantPackingUnitDto()
                                {
                                    PackingUnitId = e.PackingUnitId,
                                    LastCostPrice = e.LastCostPrice,
                                    AverageCostPrice = e.AverageCostPrice,
                                    IsDefaultPackingUnit = e.IsDefaultPackingUnit,
                                    IsDefaultPurchases = e.IsDefaultPurchases,
                                    IsDefaultSales = e.IsDefaultSales,
                                    OrderNumber = e.OrderNumber,
                                    PartsCount = e.PartsCount,
                                    SellingPrices = e.VariantPackingUnitSellingPrices.Select(s=> new VariantPackingUnitSellingPriceDto()
                                    {
                                        Amount = s.Amount,
                                        SellingPriceId = s.SellingPriceId
                                    }).ToList(),
                                    }).ToList(),
                                VariantAttachments = _context.Set<VariantAttachment>()
                                    .Include(e=>e.Attachment)
                                    .Where(va => va.VariantId == v.Id)
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
                                        Attachment = va.Attachment == null ? null :new AttachmentDto()
                                        {
                                            ContentType   = va.Attachment.FileContentType,
                                            AttachmentId = va.AttachmentId,
                                            FileContent = Convert.ToBase64String(va.Attachment.FileData),
                                            FileName = va.Attachment.FileName
                                        }
                                    }).ToList(),
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
