using Domain.Account.Models.Dtos.Attachments;
using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class ProductRepository : BaseTreeSettingRepository<Product>, IProductRepository
{
    private readonly IApplicationDbContext _context;
    public ProductRepository(IApplicationDbContext context) : base(context) => _context = context;

    public async Task<IEnumerable<ProductDto>> GetDtos()
    {
        var products = await _context.Set<Product>()
            .Include(e=>e.ProductCodes)
            .Select(product => new
            {
                product,
                picture = _context.Set<ProductAttachment>()
                    .Include(e=>e.Attachment).FirstOrDefault(e=>e.IsPrimary && e.ProductId == product.Id),
                Variants = _context.Set<Variant>()
                    .Include(e=>e.VariantAttributeValues)
                    .Where(v => v.ProductId == product.Id && !v.IsDefault)
                    .Select(v => new
                    {
                        product.Name,//v.Name,
                        v.ApplyProductChanges,
                        AttributeValues = v.VariantAttributeValues
                            .Select(av => new
                            {
                                av.AttributeDefinitionId,
                                av.AttributeValueId
                            })
                    })
                    .ToList()
            })
            .ToListAsync();
        var result = products.Select(p => new ProductDto
        {
            Id = p.product.Id,
            ParentId = p.product.ParentId,
            CreatedAt = p.product.CreatedAt,
            ModifiedAt = p.product.ModifiedAt,
            NameSecondLanguage = p.product.NameSecondLanguage,
            Name = p.product.Name,
            ProductType = p.product.ProductType,
            NodeType = p.product.NodeType,
            Version = p.product.Version,
            HasVariants = p.Variants.Any(),
            VariantsCount = p.Variants.Count,
            ProductCodes = p.product.ProductCodes,
            ProductPicture = (p.picture != null && p.picture.Attachment != null)  ? new AttachmentDto()
            {
                FileContent = Convert.ToBase64String(p.picture.Attachment.FileData),
                ContentType = p.picture.Attachment.FileContentType,
                FileName = p.picture.Attachment.FileName,
            } : null,
            VariantCombinations = p.Variants.Select(v => new VariantCombinationDto
            {
                Name = v.Name,
                ApplyProductChanges = v.ApplyProductChanges,
                AttributeValues = v.AttributeValues
                    .ToDictionary(
                        x => x.AttributeDefinitionId,
                        x => x.AttributeValueId
                    )
            }).ToList()
        });

        return result;
    }

    public async Task<ProductDto?> GetDtoById(Guid id)
    {
        var data = await _context.Set<Product>()
            .Include(e=>e.ProductPackingUnits)
            .ThenInclude(e=>e.ProductPackingUnitSellingPrices)
            .Where(p => p.Id == id)
            .Select(product => new
            {
                Product = product,

                Variants = _context.Set<Variant>()
                    .Include(e=>e.VariantAttributeValues)
                    .Where(v => v.ProductId == product.Id && !v.IsDefault)
                    .Select(v => new
                    {
                        product.Name, // v.Name,
                        v.ApplyProductChanges,
                        AttributeValues = v.VariantAttributeValues
                            .Select(av => new
                            {
                                av.AttributeDefinitionId,
                                av.AttributeValueId
                            })
                    })
                    .Where(v => v.AttributeValues.Any())
                    .ToList(),

                ProductCodes = _context.Set<ProductCode>()
                    .Where(pc => pc.ProductId == product.Id)
                    .ToList(),
                SellingPricesDiscounts=  _context.Set<ProductSellingPriceDiscount>().Where(e=>e.ProductId==product.Id).Select(e=>e.Adapt<ProductSellingPriceDiscountDto>()).ToList(),
                Attachments = _context.Set<ProductAttachment>()
                    .Include(e=>e.Attachment)
                    .Where(pa => pa.ProductId == product.Id)
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
                AttributeDefinitions = _context.Set<ProductAttributeDefinition>()
                    .Where(e=>e.ProductId == product.Id)
                    .Select(e=> new ProductAttributeDefinitionDto()
                    {
                        AttributeDefinitionId = e.AttributeDefinitionId,
                        IsVariant = e.IsVariant,
                    }).ToList(),

                SupplierIds = _context.Set<ProductSupplier>()
                    .Where(ps => ps.ProductId == product.Id)
                    .Select(ps => ps.SupplierId)
                    .ToList(),

                ManufacturerIds = _context.Set<ProductManufacturerCompany>()
                    .Where(pmc => pmc.ProductId == product.Id)
                    .Select(pmc => pmc.ManufacturerCompanyId)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (data == null)
            return null;

        var product = data.Product;

        var dto = new ProductDto
        {
            Id = product.Id,
            ParentId = product.ParentId,
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
            SellingPriceDiscounts = data.SellingPricesDiscounts,
            ProductCodes = data.ProductCodes,
            ProductAttachments = data.Attachments,
            SuppliersIds = data.SupplierIds,
            ManufacturerCompaniesIds = data.ManufacturerIds,
            ProductAttributeDefinitions = data.AttributeDefinitions,
            VariantCombinations = data.Variants.Select(v => new VariantCombinationDto
            {
                Name = v.Name,
                ApplyProductChanges = v.ApplyProductChanges,
                AttributeValues = v.AttributeValues.ToDictionary(
                    x => x.AttributeDefinitionId,
                    x => x.AttributeValueId
                )
            }).ToList(),
            HasVariants = data.Variants.Any(),
            VariantsCount = data.Variants.Count,
            PackingUnits = product.ProductPackingUnits.Select(e=> new ProductPackingUnitDto()
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
                SellingPrices = e.ProductPackingUnitSellingPrices.Select(s=> new ProductPackingUnitSellingPriceDto()
                {
                    Amount = s.Amount,
                    SellingPriceId = s.SellingPriceId
                }).ToList()
            }).ToList()
        };

        return dto;
    }


    public async Task<IEnumerable<ProductDto>> GetProductsByType(ProductType? productType)
    {
        var query = _context.Set<Product>().AsQueryable();
        
        if (productType.HasValue)
        {
            query = query.Where(p => p.ProductType == productType.Value);
        }

        var products = await (from product in query
                             select new ProductDto
                             {
                                 Id = product.Id,
                                 ParentId = product.ParentId,
                                 ConditionalDiscount = product.ConditionalDiscount,
                                 CreatedAt = product.CreatedAt,
                                 CountryOfOrigin = product.CountryOfOrigin,
                                 DefaultDiscount = product.DefaultDiscount,
                                 ModifiedAt = product.ModifiedAt,
                                 ProductCodes = _context.Set<ProductCode>().Where(e => e.ProductId == product.Id).ToList(),
                                 DefaultDiscountType = product.DefaultDiscountType,
                                 Model = product.Model,
                                 Name = product.Name,
                                 NameSecondLanguage = product.NameSecondLanguage,
                                 IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
                                 ProductType = product.ProductType,
                                 MaxDiscount = product.MaxDiscount,
                                 NodeType = product.NodeType,
                                 Version = product.Version,
                                 HasVariants = _context.Set<Variant>().Any(v => v.ProductId == product.Id)
                             }).ToListAsync();

        return products;
    }
}

