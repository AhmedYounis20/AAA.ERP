using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Application.Repositories;
using ERP.Infrastructure.Services.BaseServices;
using Inventory.Infrastructure.Validation;
using Inventory.Application.Commands.Products;
using Inventory.Application.Commands.Variants;
using Inventory.Application.Services;
using Shared.BaseEntities;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.ExpiryLevels;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using Inventory.Domain.Models.Entities.Products;
using System.Text.RegularExpressions;
using Inventory.Domain.Models.Entities.Variants;
using ERP.Shared.BaseEntities.Attachments;

namespace Inventory.Infrastructure.Services;

public class ProductService : BaseTreeSettingService<Product, ProductCreateCommand, ProductUpdateCommand>, IProductService
{
    private readonly IProductRepository _repository;
    private readonly IBaseRepository<ProductCode> _codeRepository;
    private readonly IVariantRepository _variantRepository;
    private readonly IVariantAttributeValueRepository _variantAttributeValueRepository;
    private readonly IBaseRepository<InventoryThreshold> _inventoryThresholdRepository;
    private readonly IBaseRepository<ExpiryLevel> _expiryLevelRepository;
    private readonly IBaseRepository<ProductAttachment> _productAttachmentRepository;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IProductAttributeDefinitionRepository _productAttributeDefinitionRepository;
    private readonly IBaseRepository<ProductSellingPriceDiscount> _productSellingPriceDiscountRepository;
    private readonly IBaseRepository<ProductPackingUnit> _productPackingUnitRepository;
    private readonly IBaseRepository<ProductPackingUnitSellingPrice> _productPackingUnitSellingPriceRepository;
    private readonly IBaseRepository<ProductCostCenter> _productCostCenterRepository;
    private readonly IBaseRepository<ProductManufacturerCompany> _productManufacturerCompanyRepository;
    private readonly IBaseRepository<ProductSupplier> _productSupplierRepository;
    private readonly IBaseRepository<ProductSalesTax> _productSalesTaxRepository;
    private readonly IBaseRepository<ProductPurchaseTax> _productPurchaseTaxRepository;
    private readonly IBaseRepository<VariantSupplier> _variantSupplierRepository;
    private readonly IBaseRepository<VariantSalesTax> _variantSalesTaxRepository;
    private readonly IBaseRepository<VariantPurchaseTax> _variantPurchaseTaxRepository;
    private readonly IBaseRepository<VariantManufacturerCompany> _variantManufacturerCompanyRepository;
    private readonly IBaseRepository<VariantCostCenter> _variantCostCenterRepository;
    private readonly IBaseRepository<VariantSellingPriceDiscount> _variantSellingPriceDiscountRepository;
    private readonly IBaseRepository<VariantPackingUnit> _variantPackingUnitRepository;
    private readonly IBaseRepository<VariantPackingUnitSellingPrice> _variantPackingUnitSellingPriceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVariantService _variantService;

    public ProductService(
        IProductRepository repository,
        IBaseRepository<ProductCode> codeRepository,
        IVariantRepository variantRepository,
        IVariantAttributeValueRepository variantAttributeValueRepository,
        IBaseRepository<InventoryThreshold> inventoryThresholdRepository,
        IBaseRepository<ExpiryLevel> expiryLevelRepository,
        IBaseRepository<ProductAttachment> productAttachmentRepository,
        IAttachmentRepository attachmentRepository,
        IProductAttributeDefinitionRepository productAttributeDefinitionRepository,
        IBaseRepository<ProductSellingPriceDiscount> productSellingPriceDiscountRepository,
        IBaseRepository<ProductPackingUnit> productPackingUnitRepository,
        IBaseRepository<ProductPackingUnitSellingPrice> productPackingUnitSellingPriceRepository,
        IBaseRepository<ProductCostCenter> productCostCenterRepository,
        IBaseRepository<ProductManufacturerCompany> productManufacturerCompanyRepository,
        IBaseRepository<ProductSupplier> productSupplierRepository,
        IBaseRepository<ProductSalesTax> productSalesTaxRepository,
        IBaseRepository<ProductPurchaseTax> productPurchaseTaxRepository,
        IBaseRepository<VariantSupplier> variantSupplierRepository,
        IBaseRepository<VariantSalesTax> variantSalesTaxRepository,
        IBaseRepository<VariantPurchaseTax> variantPurchaseTaxRepository,
        IBaseRepository<VariantManufacturerCompany> variantManufacturerCompanyRepository,
        IBaseRepository<VariantCostCenter> variantCostCenterRepository,
        IBaseRepository<VariantSellingPriceDiscount> variantSellingPriceDiscountRepository,
        IBaseRepository<VariantPackingUnit> variantPackingUnitRepository,
        IBaseRepository<VariantPackingUnitSellingPrice> variantPackingUnitSellingPriceRepository,
        IUnitOfWork unitOfWork,
        IVariantService variantService) : base(repository)
    {
        _repository = repository;
        _codeRepository = codeRepository;
        _variantRepository = variantRepository;
        _variantAttributeValueRepository = variantAttributeValueRepository;
        _inventoryThresholdRepository = inventoryThresholdRepository;
        _expiryLevelRepository = expiryLevelRepository;
        _productAttachmentRepository = productAttachmentRepository;
        _attachmentRepository = attachmentRepository;
        _productAttributeDefinitionRepository = productAttributeDefinitionRepository;
        _productSellingPriceDiscountRepository = productSellingPriceDiscountRepository;
        _productPackingUnitRepository = productPackingUnitRepository;
        _productPackingUnitSellingPriceRepository = productPackingUnitSellingPriceRepository;
        _productCostCenterRepository = productCostCenterRepository;
        _productManufacturerCompanyRepository = productManufacturerCompanyRepository;
        _productSupplierRepository = productSupplierRepository;
        _productSalesTaxRepository = productSalesTaxRepository;
        _productPurchaseTaxRepository = productPurchaseTaxRepository;
        _variantSupplierRepository = variantSupplierRepository;
        _variantSalesTaxRepository = variantSalesTaxRepository;
        _variantPurchaseTaxRepository = variantPurchaseTaxRepository;
        _variantManufacturerCompanyRepository = variantManufacturerCompanyRepository;
        _variantCostCenterRepository = variantCostCenterRepository;
        _variantSellingPriceDiscountRepository = variantSellingPriceDiscountRepository;
        _variantPackingUnitRepository = variantPackingUnitRepository;
        _variantPackingUnitSellingPriceRepository = variantPackingUnitSellingPriceRepository;
        _unitOfWork = unitOfWork;
        _variantService = variantService;
    }

    public override async Task<ApiResponse<Product>> Create(ProductCreateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateCreate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<Product>
            {
                IsSuccess = false,
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for Product + Related items creation
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            Product product = new Product
            {
                Name = command.Name,
                NameSecondLanguage = command.NameSecondLanguage,
                NodeType = command.NodeType,
                ParentId = command.ParentId,
            };

            // Prepare navigation properties using helper methods
            product.ProductCodes = CreateProductCodes(command);
            
            if (command.NodeType == NodeType.Domain)
            {
                product.Model = command.Model;
                product.Version = command.Version;
                product.CountryOfOrigin = command.CountryOfOrigin;
                product.ProductType = command.ProductType;
                product.ConditionalDiscount = command.ConditionalDiscount;
                product.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                product.DefaultDiscount = command.IsDiscountBasedOnSellingPrice ? 0 : command.DefaultDiscount;
                product.DefaultDiscountType = command.DefaultDiscountType;
                product.MaxDiscount = command.MaxDiscount;
                product.IsSales = command.IsSales;
                product.IsPurchase = command.IsPurchase;
                product.TrackedBy = command.TrackedBy;

                product.ProductSellingPriceDiscounts = CreatProductSellingPriceDiscounts(command);
                product.ProductSuppliers = CreateProductSuppliers(command);
                product.ProductSalesTaxes = CreateProductSalesTaxes(command);
                product.ProductPurchaseTaxes = CreateProductPurchaseTaxes(command);
                product.ProductCostCenters = CreateProductCostCenters(command);
                product.ProductManufacturerCompanies = CreateProductManufacturerCompanies(command);
                product.ProductPackingUnits =  CreateProductPackingUnits(command);
                product.InventoryThresholdScope = command.InventoryThresholdScope;
                product.InventoryThresholdBranchId = command.InventoryThresholdBranchId;
                product.InventoryThresholds = CreateProductInventoryThresholds(command);
                product.ExpiryLevels = CreateProductExpiryLevels(command);

                // Prepare product attributes if provided
                if (command.ProductAttributeDefinitions?.Any() == true)
                {
                    product.ProductAttributeDefinitions = CreateProductAttributes(command.ProductAttributeDefinitions);
                }
            }

            // Handle product attachments (requires creating Attachment entities first)
            if (command.ProductAttachments != null && command.ProductAttachments.Any())
            {
                var filteredAttachments = command.ProductAttachments.Where(e => command.NodeType == NodeType.Domain || (e.AttachmentType == AttachmentType.Picture && e.IsPrimary));
                product.ProductAttachments =  CreateProductAttachment(filteredAttachments);
            }

            // Prepare variants for Domain products (no database hits - prepared in memory)
            if (command.NodeType == NodeType.Domain)
                product.Variants = _variantService.PrepareVariantsForProduct(command);

            // Single database hit - EF will cascade insert all navigation properties including variants
            await _repository.Add(product);

            // Commit transaction - everything succeeded
            await _unitOfWork.CommitAsync();

            return new ApiResponse<Product> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = product };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Product> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }

    #region Helper Methods for Preparing Navigation Properties

    private List<ProductCode> CreateProductCodes(ProductCreateCommand command)
    {
        var productCodes = new List<ProductCode>
        {
            new ProductCode
            {
                Code = command.Code,
                CodeType = ProductCodeType.Code,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }
        };

        if (command.NodeType == NodeType.Domain)
        {
            if (!string.IsNullOrEmpty(command.EGSCode))
            {
                productCodes.Add(new ProductCode
                {
                    Code = command.EGSCode,
                    CodeType = ProductCodeType.EGS,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }

            if (!string.IsNullOrEmpty(command.Gs1Code))
            {
                productCodes.Add(new ProductCode
                {
                    Code = command.Gs1Code,
                    CodeType = ProductCodeType.GS1,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }

            // Prepare barcodes
            foreach (var barcode in command.BarCodes)
            {
                productCodes.Add(new ProductCode
                {
                    Code = barcode,
                    CodeType = ProductCodeType.BarCode,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
        }

        return  productCodes;
    }

    private List<ProductSellingPriceDiscount> CreatProductSellingPriceDiscounts(ProductCreateCommand command)
    {
        if (command.IsDiscountBasedOnSellingPrice && command.SellingPriceDiscounts?.Any() == true)
        {
            return  command.SellingPriceDiscounts.Select(e => new ProductSellingPriceDiscount
            {
                SellingPriceId = e.SellingPriceId,
                Discount = e.Discount,
                DiscountType = e.DiscountType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductSellingPriceDiscount>();
    }

    private List<ProductSupplier> CreateProductSuppliers(ProductCreateCommand command)
    {
        if (command.SuppliersIds?.Any() == true)
        {
            return command.SuppliersIds.Select(supplierId => new ProductSupplier
            {
                SupplierId = supplierId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductSupplier>();
    }

    private List<ProductSalesTax> CreateProductSalesTaxes(ProductCreateCommand command)
    {
        if (command.SalesTaxIds?.Any() == true)
        {
            return command.SalesTaxIds.Select(taxId => new ProductSalesTax
            {
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductSalesTax>();
    }

    private List<ProductPurchaseTax> CreateProductPurchaseTaxes(ProductCreateCommand command)
    {
        if (command.PurchaseTaxIds?.Any() == true)
        {
            return command.PurchaseTaxIds.Select(taxId => new ProductPurchaseTax
            {
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductPurchaseTax>();
    }

    private List<ProductCostCenter> CreateProductCostCenters(ProductCreateCommand command)
    {
        if (command.CostCenters?.Any() == true)
        {
            return command.CostCenters.Select(e => new ProductCostCenter
            {
                CostCenterId = e.CostCenterId,
                Percent = e.Percent,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductCostCenter>();
    }

    private List<ProductManufacturerCompany> CreateProductManufacturerCompanies(ProductCreateCommand command)
    {
        if (command.ManufacturerCompaniesIds?.Any() == true)
        {
           return command.ManufacturerCompaniesIds.Select(companyId => new ProductManufacturerCompany
            {
                ManufacturerCompanyId = companyId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductManufacturerCompany>();
    }

    private List<ProductPackingUnit> CreateProductPackingUnits(ProductCreateCommand command)
    {
        if (command.PackingUnits?.Any() == true)
        {
            return command.PackingUnits.Select(packingUnitDto => new ProductPackingUnit
            {
                PackingUnitId = packingUnitDto.PackingUnitId,
                AverageCostPrice = packingUnitDto.AverageCostPrice,
                IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                LastCostPrice = packingUnitDto.LastCostPrice,
                PartsCount = packingUnitDto.PartsCount,
                ModifiedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                IsDefaultSales = packingUnitDto.IsDefaultSales,
                IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                OrderNumber = packingUnitDto.OrderNumber,
                ProductPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(e => new ProductPackingUnitSellingPrice
                {
                    Amount = e.Amount,
                    SellingPriceId = e.SellingPriceId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                }).ToList()
            }).ToList();
        }

        return new List<ProductPackingUnit>();
    }

    private List<ProductAttributeDefinition> CreateProductAttributes(List<ProductAttributeDefinitionDto> productAttributeDefinitions)
    {
        if (productAttributeDefinitions?.Any() == true)
        {
            return productAttributeDefinitions.Select(attr => new ProductAttributeDefinition
            { 
                AttributeDefinitionId = attr.AttributeDefinitionId,
                IsVariant = attr.IsVariant,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        return new List<ProductAttributeDefinition>();
    }

    private List<ProductAttachment> CreateProductAttachment(IEnumerable<ProductAttachmentDto> productAttachmentDtos)
    {
        var attachmentData = new List<(Attachment attachment, ProductAttachmentDto dto)>();
        var productAttachments = new List<ProductAttachment>();

        // Prepare attachment entities and store their corresponding DTOs
        foreach (var productAttachmentDto in productAttachmentDtos)
        {
            var attachmentDto = productAttachmentDto.Attachment;

            if (attachmentDto != null)
            {
                // Create Attachment entity
                var attachment = new Attachment
                {
                    FileName = attachmentDto.FileName,
                    FileContentType = attachmentDto.ContentType,
                    FileData = Convert.FromBase64String(attachmentDto.FileContent),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                attachmentData.Add((attachment, productAttachmentDto));
            }
        }
        
        // Now create ProductAttachment entities with the attachment IDs
        foreach (var (attachment, productAttachmentDto) in attachmentData)
        {
            var productAttachment = new ProductAttachment
            {
                Attachment = attachment,
                AttachmentType = productAttachmentDto.AttachmentType,
                Description = productAttachmentDto.Description,
                IsPrimary = productAttachmentDto.IsPrimary,
                DisplayOrder = productAttachmentDto.DisplayOrder,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            productAttachments.Add(productAttachment);
        }

        return productAttachments;
    }

    #endregion

    public override async Task<ApiResponse<Product>> Update(ProductUpdateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid || validationResult.entity == null)
        {
            return new ApiResponse<Product>
            {
                IsSuccess = false,
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for Domain + SubDomains update
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            Product product = validationResult.entity;
            // Restrict SubDomain updates
            
            product.Name = command.Name;
            product.NameSecondLanguage = command.NameSecondLanguage;
            if (product.NodeType == NodeType.Domain)
            {
                // product.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                //
                product.Model = command.Model;
                product.Version = command.Version;
                product.CountryOfOrigin = command.CountryOfOrigin;
                // ProductType is not updatable
                product.MaxDiscount = command.MaxDiscount;
                product.ConditionalDiscount = command.ConditionalDiscount;
                product.DefaultDiscount = command.DefaultDiscount;
                product.DefaultDiscountType = command.DefaultDiscountType;
                product.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                product.IsSales = command.IsSales;
                product.IsPurchase = command.IsPurchase;
                product.TrackedBy = command.TrackedBy;
                await UpdateProductBarCodes(command.BarCodes, product.Id);
                await UpdateProductSuppliers(command.SuppliersIds, product.Id);
                await UpdateProductSalesTaxes(command.SalesTaxIds, product.Id);
                await UpdateProductPurchaseTaxes(command.PurchaseTaxIds, product.Id);
                await UpdateProductCostCenters(product.Id, command.CostCenters);
                await UpdateProductManufacturerCompanies(command.ManufacturerCompaniesIds, product.Id);
                await UpdateProductSellingPriceDiscounts(product.Id,command.SellingPriceDiscounts);
                await UpdateProductPackingUnits(product.Id,command.PackingUnits);
                product.InventoryThresholdScope = command.InventoryThresholdScope;
                product.InventoryThresholdBranchId = command.InventoryThresholdBranchId;
                await UpdateProductInventoryThresholds(product.Id, command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
                await UpdateProductExpiryLevels(product.Id, command.ExpiryLevels);
                // Update product attributes if provided
                if (command.ProductAttributeDefinitions?.Any() == true)
                {
                    await UpdateProductAttributes(product.Id, command.ProductAttributeDefinitions);
                }
            }
            
            // Handle item attachments update
            await UpdateProductAttachments(command.ProductAttachments.Where(e=> command.NodeType == NodeType.Domain ||(e.AttachmentType == AttachmentType.Picture && e.IsPrimary) ), product.Id);

            await _repository.Update(product);

            // Handle variant updates and combinations for Domain products
            if (product.NodeType == NodeType.Domain)
            {
                // Handle variant combinations and updates
                var variantCombinationResult = await UpdateProductVariants(product, command);
                if (!variantCombinationResult.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return new ApiResponse<Product>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessages = variantCombinationResult.ErrorMessages
                    };
                }
            }

            // Commit transaction - everything succeeded
            await _unitOfWork.CommitAsync();

            return new ApiResponse<Product> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = product };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Product> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILED"] };
        }
    }

    public override async Task<ApiResponse<Product>> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await ValidateDelete(id);
        if (!validationResult.isValid || validationResult.entity == null)
        {
            return new ApiResponse<Product>()
            {
                IsSuccess = validationResult.isValid,
                Errors = validationResult.errors.Select(e => new MessageTemplate()
                {
                    MessageKey = e
                }).ToList(),
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _inventoryThresholdRepository.ExecuteDeleteAsync(t => t.ProductId == id);
            await _expiryLevelRepository.ExecuteDeleteAsync(t => t.ProductId == id);
            var variantIds = await _variantRepository.GetQuery()
                .Where(v => v.ProductId == id)
                .Select(v => v.Id)
                .ToListAsync();
            if (variantIds.Count > 0)
            {
                await _inventoryThresholdRepository.ExecuteDeleteAsync(t =>
                    t.VariantId != null && variantIds.Contains(t.VariantId.Value));
                await _expiryLevelRepository.ExecuteDeleteAsync(t =>
                    t.VariantId != null && variantIds.Contains(t.VariantId.Value));
            }
            var attachmentsIds = await _productAttachmentRepository.GetQuery().Where(e => e.ProductId == id)
                .Select(e=>e.AttachmentId).ToListAsync();
            // Delete Product (cascades to ProductAttachments, Variants, etc.)
            await _repository.Delete(id);

            // Now safely delete orphaned Attachments (no constraint conflicts)

            await _attachmentRepository.ExecuteDeleteAsync(e=> attachmentsIds.Contains(e.Id));
            
            await _unitOfWork.CommitAsync();

            return new ApiResponse<Product>()
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<Product>()
            {
                IsSuccess = validationResult.isValid,
                Errors = [new MessageTemplate(){
                        MessageKey = "OPERATION_FAILD"
                    }],
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ApiResponse<string>> GetNextCode(Guid? parentId = null)
    {
        var parent = await _codeRepository.GetQuery()
            .Where(e => e.ProductId == parentId && e.CodeType == ProductCodeType.Code)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.Code)
            .FirstOrDefaultAsync();
        
        var lastSibling = await _repository.GetQuery()
            .Where(e => e.ParentId == parentId)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();
        
        var lastSiblingCode = lastSibling == null ? string.Empty : 
            await _codeRepository.GetQuery()
                .Where(e => e.ProductId == lastSibling.Id && e.CodeType == ProductCodeType.Code)
                .Select(e => e.Code)
                .FirstOrDefaultAsync();
        
        string code = GetNextChildCode(lastSiblingCode ?? string.Empty, parent ?? string.Empty);
        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = code
        };
    }

    public async Task<ApiResponse<List<ProductDto>>> GetProductDtos()
    {
        try
        {
            var result = await _repository.GetDtos();
            return new ApiResponse<List<ProductDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.ToList()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<ProductDto?>> GetProductDtoById(Guid id)
    {
        try
        {
            var result = await _repository.GetDtoById(id);
            return new ApiResponse<ProductDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }


    private string GetNextChildCode(string lastSiblingCode, string parentCode)
    {
        if (string.IsNullOrEmpty(lastSiblingCode))
        {
            return parentCode + "001";
        }

        var match = Regex.Match(lastSiblingCode, @"(\d+)$");
        if (match.Success)
        {
            var numberPart = match.Groups[1].Value;
            var prefix = lastSiblingCode.Substring(0, lastSiblingCode.Length - numberPart.Length);
            var nextNumber = int.Parse(numberPart) + 1;
            return prefix + nextNumber.ToString("D" + numberPart.Length);
        }

        return lastSiblingCode + "001";
    }
    
    private async Task CreateProductAttributes(Guid productId, List<ProductAttributeDefinitionDto> productAttributeDefinitions)
    {
        var productAttributeDefinitionRepository = _productAttributeDefinitionRepository;
        
        foreach (var attr in productAttributeDefinitions)
        {
            var productAttributeDefinition = new ProductAttributeDefinition
            {
                ProductId = productId,
                AttributeDefinitionId = attr.AttributeDefinitionId,
                IsVariant = attr.IsVariant,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        
            await productAttributeDefinitionRepository.Add(productAttributeDefinition);
        }
    }

    private async Task UpdateProductAttributes(Guid productId, List<ProductAttributeDefinitionDto> productAttributeDefinitions)
    {
        var productAttributeDefinitionRepository = _productAttributeDefinitionRepository;
        
        // Remove existing attributes
        var existingAttributes = await productAttributeDefinitionRepository.GetQuery()
            .Where(pa => pa.ProductId == productId)
            .ToListAsync();
        
        foreach (var attr in existingAttributes)
        {
            await productAttributeDefinitionRepository.Delete(attr);
        }
        
        // Add new attributes
        await CreateProductAttributes(productId, productAttributeDefinitions);
    }

    private  async Task CreateProductSellingPriceDiscounts(List<ProductSellingPriceDiscountDto> command,Guid productId)
    {
        List<ProductSellingPriceDiscount> discounts = command.Select(e => new ProductSellingPriceDiscount
        {
            ProductId = productId,
            SellingPriceId = e.SellingPriceId,
            Discount = e.Discount,
            DiscountType = e.DiscountType,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        }).ToList();
        
        await _productSellingPriceDiscountRepository.Add(discounts);
    }

    private async Task UpdateProductPackingUnits(Guid productId, List<ProductPackingUnitDto>? productPackingUnits)
    {
        var productPackingUnitRepository = _productPackingUnitRepository;
        
        // Get existing packing units
        var existingPackingUnits = await productPackingUnitRepository.GetQuery()
            .Where(ppu => ppu.ProductId == productId)
            .ToListAsync();

        // If product packing units is null or empty, remove all packing units
        if (productPackingUnits == null || !productPackingUnits.Any())
        {
            if (existingPackingUnits.Any())
            {
                await productPackingUnitRepository.Delete(existingPackingUnits);
            }
            return;
        }

        // Find packing units to remove (ones not in the command)
        var packingUnitsToRemove = existingPackingUnits
            .Where(e => !productPackingUnits.Any(p => p.PackingUnitId == e.PackingUnitId))
            .ToList();

        // Remove deleted packing units
        if (packingUnitsToRemove.Any())
        {
            await productPackingUnitRepository.Delete(packingUnitsToRemove);
        }

        // Process each packing unit from command
        var packingUnitsToAdd = new List<ProductPackingUnit>();
        var packingUnitsToUpdate = new List<ProductPackingUnit>();

        foreach (var packingUnitDto in productPackingUnits)
        {
            var existingPackingUnit = existingPackingUnits.FirstOrDefault(e => e.PackingUnitId == packingUnitDto.PackingUnitId);
            
            if (existingPackingUnit == null)
            {
                // Add new packing unit
                packingUnitsToAdd.Add(new ProductPackingUnit
                {
                    PackingUnitId = packingUnitDto.PackingUnitId,
                    ProductId = productId,
                    AverageCostPrice = packingUnitDto.AverageCostPrice,
                    IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                    LastCostPrice = packingUnitDto.LastCostPrice,
                    PartsCount = packingUnitDto.PartsCount,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDefaultSales = packingUnitDto.IsDefaultSales,
                    IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                    OrderNumber = packingUnitDto.OrderNumber,
                    ProductPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(e => new ProductPackingUnitSellingPrice
                    {
                        Amount = e.Amount,
                        SellingPriceId = e.SellingPriceId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                    }).ToList()
                });
            }
            else
            {
                // Update existing packing unit
                existingPackingUnit.PackingUnitId = packingUnitDto.PackingUnitId;
                existingPackingUnit.AverageCostPrice = packingUnitDto.AverageCostPrice;
                existingPackingUnit.IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit;
                existingPackingUnit.LastCostPrice = packingUnitDto.LastCostPrice;
                existingPackingUnit.PartsCount = packingUnitDto.PartsCount;
                existingPackingUnit.IsDefaultSales = packingUnitDto.IsDefaultSales;
                existingPackingUnit.IsDefaultPurchases = packingUnitDto.IsDefaultPurchases;
                existingPackingUnit.OrderNumber = packingUnitDto.OrderNumber;
                existingPackingUnit.ModifiedAt = DateTime.Now;
                packingUnitsToUpdate.Add(existingPackingUnit);

                // Update selling prices
                await UpdateProductPackingUnitSellingPrices(packingUnitDto, existingPackingUnit);
            }
        }

        // Add new packing units
        if (packingUnitsToAdd.Any())
        {
            await productPackingUnitRepository.Add(packingUnitsToAdd);
        }

        // Batch update existing packing units
        if (packingUnitsToUpdate.Any())
        {
            await productPackingUnitRepository.Update(packingUnitsToUpdate);
        }
    }

    private async Task UpdateProductPackingUnitSellingPrices(ProductPackingUnitDto packingUnitDto, ProductPackingUnit existingPackingUnit)
    {
        var existingPrices = await _productPackingUnitSellingPriceRepository.GetQuery()
            .Where(e => e.ProductPackingUnitId == existingPackingUnit.Id)
            .ToListAsync();

        var pricesToAdd = new List<ProductPackingUnitSellingPrice>();
        var pricesToUpdate = new List<ProductPackingUnitSellingPrice>();
        var pricesToRemove = existingPrices
            .Where(e => !packingUnitDto.SellingPrices.Any(p => p.SellingPriceId == e.SellingPriceId))
            .ToList();

        // Remove prices that are no longer in the command
        if (pricesToRemove.Any())
        {
            await _productPackingUnitSellingPriceRepository.Delete(pricesToRemove);
        }

        foreach (var priceDto in packingUnitDto.SellingPrices)
        {
            var existingPrice = existingPrices.FirstOrDefault(e => e.SellingPriceId == priceDto.SellingPriceId);
            
            if (existingPrice == null)
            {
                // Add new price
                pricesToAdd.Add(new ProductPackingUnitSellingPrice
                {
                    ProductPackingUnitId = existingPackingUnit.Id,
                    SellingPriceId = priceDto.SellingPriceId,
                    Amount = priceDto.Amount,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
            else
            {
                // Update existing price
                existingPrice.Amount = priceDto.Amount;
                existingPrice.ModifiedAt = DateTime.Now;
                pricesToUpdate.Add(existingPrice);
            }
        }

        if (pricesToAdd.Any())
        {
            await _productPackingUnitSellingPriceRepository.Add(pricesToAdd);
        }

        if (pricesToUpdate.Any())
        {
            await _productPackingUnitSellingPriceRepository.Update(pricesToUpdate);
        }
    }

    private async Task UpdateProductSellingPriceDiscounts(Guid productId, List<ProductSellingPriceDiscountDto> productSellingPriceDiscounts)
    {
        var repository = _productSellingPriceDiscountRepository;

        // Load existing discounts for this product once
        var existingDiscounts = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        // If no discounts provided, remove all existing
        if (productSellingPriceDiscounts == null || !productSellingPriceDiscounts.Any())
        {
            if (existingDiscounts.Any())
            {
                await repository.Delete(existingDiscounts);
            }
            return;
        }

        var existingBySellingPrice = existingDiscounts
            .ToDictionary(e => e.SellingPriceId, e => e);
        var incomingIds = productSellingPriceDiscounts
            .Select(d => d.SellingPriceId)
            .ToHashSet();

        // Discounts to remove (no longer present in the incoming list)
        var toRemove = existingDiscounts
            .Where(e => !incomingIds.Contains(e.SellingPriceId))
            .ToList();
        if (toRemove.Any())
        {
            await repository.Delete(toRemove);
        }

        var now = DateTime.Now;
        var toAdd = new List<ProductSellingPriceDiscount>();
        var toUpdate = new List<ProductSellingPriceDiscount>();

        foreach (var dto in productSellingPriceDiscounts)
        {
            if (existingBySellingPrice.TryGetValue(dto.SellingPriceId, out var existing))
            {
                // Update existing row if values changed
                existing.Discount = dto.Discount;
                existing.DiscountType = dto.DiscountType;
                existing.ModifiedAt = now;
                toUpdate.Add(existing);
            }
            else
            {
                // New discount row
                toAdd.Add(new ProductSellingPriceDiscount
                {
                    ProductId = productId,
                    SellingPriceId = dto.SellingPriceId,
                    Discount = dto.Discount,
                    DiscountType = dto.DiscountType,
                    CreatedAt = now,
                    ModifiedAt = now
                });
            }
        }

        if (toAdd.Any())
        {
            await repository.Add(toAdd);
        }

        if (toUpdate.Any())
        {
            await repository.Update(toUpdate);
        }
    }

    private async Task UpdateProductCostCenters(Guid productId, List<ProductCostCenterDto>? productCostCenters)
    {
        var repository = _productCostCenterRepository;

        var existingCostCenters = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        if (productCostCenters == null || !productCostCenters.Any())
        {
            if (existingCostCenters.Any())
            {
                await repository.Delete(existingCostCenters);
            }
            return;
        }

        var existingByCostCenterId = existingCostCenters
            .ToDictionary(e => e.CostCenterId, e => e);
        var incomingIds = productCostCenters
            .Select(d => d.CostCenterId)
            .ToHashSet();

        var toRemove = existingCostCenters
            .Where(e => !incomingIds.Contains(e.CostCenterId))
            .ToList();
        if (toRemove.Any())
        {
            await repository.Delete(toRemove);
        }

        var now = DateTime.Now;
        var toAdd = new List<ProductCostCenter>();
        var toUpdate = new List<ProductCostCenter>();

        foreach (var dto in productCostCenters)
        {
            if (existingByCostCenterId.TryGetValue(dto.CostCenterId, out var existing))
            {
                existing.Percent = dto.Percent;
                existing.ModifiedAt = now;
                toUpdate.Add(existing);
            }
            else
            {
                toAdd.Add(new ProductCostCenter
                {
                    ProductId = productId,
                    CostCenterId = dto.CostCenterId,
                    Percent = dto.Percent,
                    CreatedAt = now,
                    ModifiedAt = now
                });
            }
        }

        if (toAdd.Any())
        {
            await repository.Add(toAdd);
        }

        if (toUpdate.Any())
        {
            await repository.Update(toUpdate);
        }
    }

    private async Task UpdateProductManufacturerCompanies(List<Guid> companiesIds, Guid productId)
    {
        var repository = _productManufacturerCompanyRepository;
        var existingCompanies = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        // If no new companies provided, remove all existing
        if (companiesIds == null || !companiesIds.Any())
        {
            if (existingCompanies.Any())
            {
                await repository.Delete(existingCompanies);
            }
            return;
        }

        var existingCompanyIds = existingCompanies
            .Select(e => e.ManufacturerCompanyId)
            .ToHashSet();

        // Companies to remove (no longer present in the incoming list)
        var companiesToRemove = existingCompanies
            .Where(e => !companiesIds.Contains(e.ManufacturerCompanyId))
            .ToList();

        if (companiesToRemove.Any())
        {
            await repository.Delete(companiesToRemove);
        }

        // Companies to add (new ids not already linked)
        var now = DateTime.Now;
        var companiesToAdd = companiesIds
            .Where(id => !existingCompanyIds.Contains(id))
            .Select(companyId => new ProductManufacturerCompany
            {
                ManufacturerCompanyId = companyId,
                ProductId = productId,
                CreatedAt = now,
                ModifiedAt = now
            })
            .ToList();

        if (companiesToAdd.Any())
        {
            await repository.Add(companiesToAdd);
        }
    }
    private async Task UpdateProductBarCodes(List<string> barCodes, Guid productId)
    {
        // Normalize input (avoid duplicates & nulls)
        var newCodes = barCodes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Distinct()
            .ToHashSet();

        var oldEntities = await _codeRepository
            .GetQuery()
            .Where(e =>
                e.ProductId == productId &&
                e.CodeType == ProductCodeType.BarCode)
            .ToListAsync();

        var oldCodes = oldEntities
            .Select(e => e.Code)
            .ToHashSet();

        // 1️⃣ Remove deleted barcodes
        var removedEntities = oldEntities
            .Where(e => !newCodes.Contains(e.Code))
            .ToList();

        if (removedEntities.Any())
            await _codeRepository.Delete(removedEntities);

        // 2️⃣ Add newly added barcodes
        var addedCodes = newCodes
            .Where(c => !oldCodes.Contains(c))
            .Select(code => new ProductCode
            {
                Code = code,
                ProductId = productId,
                CodeType = ProductCodeType.BarCode,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            })
            .ToList();

        if (addedCodes.Any())
            await _codeRepository.Add(addedCodes);
    }
    
    private async Task UpdateProductSuppliers(List<Guid> suppliersIds, Guid productId)
    {
        var repository = _productSupplierRepository;
        var existingSuppliers = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        // If no new suppliers provided, remove all existing
        if (suppliersIds == null || !suppliersIds.Any())
        {
            if (existingSuppliers.Any())
            {
                await repository.Delete(existingSuppliers);
            }
            return;
        }

        var existingSupplierIds = existingSuppliers
            .Select(e => e.SupplierId)
            .ToHashSet();

        // Suppliers to remove (no longer present in the incoming list)
        var suppliersToRemove = existingSuppliers
            .Where(e => !suppliersIds.Contains(e.SupplierId))
            .ToList();

        if (suppliersToRemove.Any())
        {
            await repository.Delete(suppliersToRemove);
        }

        // Suppliers to add (new ids not already linked)
        var now = DateTime.Now;
        var suppliersToAdd = suppliersIds
            .Where(id => !existingSupplierIds.Contains(id))
            .Select(supplierId => new ProductSupplier
            {
                ProductId = productId,
                SupplierId = supplierId,
                CreatedAt = now,
                ModifiedAt = now,
            })
            .ToList();

        if (suppliersToAdd.Any())
        {
            await repository.Add(suppliersToAdd);
        }
    }

    private async Task UpdateProductSalesTaxes(List<Guid> salesTaxIds, Guid productId)
    {
        var repository = _productSalesTaxRepository;
        var existingTaxes = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        if (salesTaxIds == null || !salesTaxIds.Any())
        {
            if (existingTaxes.Any())
                await repository.Delete(existingTaxes);
            return;
        }

        var existingTaxIds = existingTaxes.Select(e => e.TaxId).ToHashSet();
        var taxesToRemove = existingTaxes.Where(e => !salesTaxIds.Contains(e.TaxId)).ToList();
        if (taxesToRemove.Any())
            await repository.Delete(taxesToRemove);

        var now = DateTime.Now;
        var taxesToAdd = salesTaxIds
            .Where(id => !existingTaxIds.Contains(id))
            .Select(taxId => new ProductSalesTax
            {
                ProductId = productId,
                TaxId = taxId,
                CreatedAt = now,
                ModifiedAt = now,
            })
            .ToList();

        if (taxesToAdd.Any())
            await repository.Add(taxesToAdd);
    }

    private async Task UpdateProductPurchaseTaxes(List<Guid> purchaseTaxIds, Guid productId)
    {
        var repository = _productPurchaseTaxRepository;
        var existingTaxes = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();

        if (purchaseTaxIds == null || !purchaseTaxIds.Any())
        {
            if (existingTaxes.Any())
                await repository.Delete(existingTaxes);
            return;
        }

        var existingTaxIds = existingTaxes.Select(e => e.TaxId).ToHashSet();
        var taxesToRemove = existingTaxes.Where(e => !purchaseTaxIds.Contains(e.TaxId)).ToList();
        if (taxesToRemove.Any())
            await repository.Delete(taxesToRemove);

        var now = DateTime.Now;
        var taxesToAdd = purchaseTaxIds
            .Where(id => !existingTaxIds.Contains(id))
            .Select(taxId => new ProductPurchaseTax
            {
                ProductId = productId,
                TaxId = taxId,
                CreatedAt = now,
                ModifiedAt = now,
            })
            .ToList();

        if (taxesToAdd.Any())
            await repository.Add(taxesToAdd);
    }

    private async Task UpdateProductAttachments(IEnumerable<ProductAttachmentDto> productAttachmentDtos, Guid productId)
    {
        await DeleteOldProductAttachments(productId);

        await AddNewProductAttachments(productAttachmentDtos, productId);
    }

    private async Task AddNewProductAttachments(IEnumerable<ProductAttachmentDto> productAttachmentDtos, Guid productId)
    {
        var productAttachments = new List<ProductAttachment>();
        foreach (var productAttachmentDto in productAttachmentDtos)
        {
        
            var attachmentDto = productAttachmentDto.Attachment;
        
            if (attachmentDto != null)
            {
                var productAttachment = new ProductAttachment
                {
                    ProductId = productId,
                    Attachment= new Attachment
                    {
                        FileName = attachmentDto.FileName,
                        FileContentType = attachmentDto.ContentType,
                        FileData = Convert.FromBase64String(attachmentDto.FileContent),
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    },
                    AttachmentType = productAttachmentDto.AttachmentType,
                    Description = productAttachmentDto.Description,
                    IsPrimary = productAttachmentDto.IsPrimary,
                    DisplayOrder = productAttachmentDto.DisplayOrder,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
        
                productAttachments.Add(productAttachment);
            }
        }
        
        if(productAttachments.Any())
            await _productAttachmentRepository.Add(productAttachments);
    }

    private async Task DeleteOldProductAttachments(Guid productId)
    {
        var productAttachments = await _productAttachmentRepository.GetQuery().Where(e => e.ProductId == productId).ToListAsync();
        var attachmentIds = productAttachments.Select(e => e.AttachmentId);
        if(productAttachments.Any())
            await _productAttachmentRepository.Delete(productAttachments);
        if(attachmentIds?.Any() == true)
            await _attachmentRepository.ExecuteDeleteAsync(e=> attachmentIds.Contains(e.Id));
    }
    
    private async Task<ApiResponse<bool>> UpdateProductVariants(Product product, ProductUpdateCommand command)
    {
        try
        {
            // Only products of type Stock can have combination variants; others only have the default variant
            var effectiveVariantCombinations = product.ProductType == ProductType.Stock ? command.VariantCombinations : null;

            // Get all existing variants for this product
            var existingVariants = await _variantRepository.GetByProductIdAsync(product.Id);

            // Get variant attribute values for all existing non-default variants in a single batched query
            var variantAttributeValuesMap = new Dictionary<Guid, Dictionary<Guid, Guid>>();
            var nonDefaultVariantIds = existingVariants
                .Where(v => !v.IsDefault)
                .Select(v => v.Id)
                .ToList();

            if (nonDefaultVariantIds.Any())
            {
                var allVariantAttributeValues = await _variantAttributeValueRepository
                    .GetQuery()
                    .Where(vav => nonDefaultVariantIds.Contains(vav.VariantId))
                    .ToListAsync();

                foreach (var group in allVariantAttributeValues.GroupBy(vav => vav.VariantId))
                {
                    variantAttributeValuesMap[group.Key] = group.ToDictionary(
                        vav => vav.AttributeDefinitionId,
                        vav => vav.AttributeValueId
                    );
                }
            }

            // Create a set of new combinations from the command
            var newCombinations = new HashSet<string>();
            if (effectiveVariantCombinations?.Any() == true)
            {
                foreach (var combination in effectiveVariantCombinations)
                {
                    var combinationKey = GetCombinationKey(combination.AttributeValues);
                    newCombinations.Add(combinationKey);
                }
            }

            // Process existing variants
            var variantsToDelete = new List<Variant>();
            var variantsToArchive = new List<Variant>();
            var variantsToUpdate = new List<Variant>();
            var processedCombinations = new HashSet<string>();

            // Handle default variant - always apply product changes
            var defaultVariant = existingVariants.FirstOrDefault(v => v.IsDefault);
            if (defaultVariant != null)
            {
                defaultVariant.MaxDiscount = product.MaxDiscount;
                defaultVariant.ConditionalDiscount = product.ConditionalDiscount;
                defaultVariant.DefaultDiscount = product.DefaultDiscount;
                defaultVariant.DefaultDiscountType = product.DefaultDiscountType;
                defaultVariant.IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice;
                defaultVariant.Model = product.Model;
                defaultVariant.Version = product.Version;
                defaultVariant.CountryOfOrigin = product.CountryOfOrigin;
                defaultVariant.IsSales = product.IsSales;
                defaultVariant.IsPurchase = product.IsPurchase;
                defaultVariant.IsArchived = effectiveVariantCombinations?.Any() == true;
                
                // Update suppliers
                await UpdateVariantSuppliers(command.SuppliersIds, defaultVariant.Id);

                // Update sales taxes
                await UpdateVariantSalesTaxes(command.SalesTaxIds, defaultVariant.Id);

                // Update purchase taxes
                await UpdateVariantPurchaseTaxes(command.PurchaseTaxIds, defaultVariant.Id);

                // Update manufacturer companies
                await UpdateVariantManufacturerCompanies(command.ManufacturerCompaniesIds, defaultVariant.Id);

                // Update cost centers
                await UpdateVariantCostCenters(command.CostCenters, defaultVariant.Id);

                // Update selling price discounts
                if (product.IsDiscountBasedOnSellingPrice)
                {
                    await UpdateVariantSellingPriceDiscounts(command.SellingPriceDiscounts, defaultVariant.Id);
                }

                // Update inventory thresholds (default variant always applies product thresholds)
                defaultVariant.InventoryThresholdScope = product.InventoryThresholdScope;
                defaultVariant.InventoryThresholdBranchId = product.InventoryThresholdBranchId;
                await UpdateVariantInventoryThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId, defaultVariant.Id);
                await UpdateVariantExpiryLevels(command.ExpiryLevels, defaultVariant.Id);

                // Update packing units (default variant always applies product changes)
                await UpdateVariantPackingUnitsFromProduct(command.PackingUnits, defaultVariant.Id);

                variantsToUpdate.Add(defaultVariant);
            }

            foreach (var variant in existingVariants)
            {
                // Skip default variant (already handled above)
                if (variant.IsDefault)
                    continue;

                // Skip if variant doesn't have attribute values in our map
                if (!variantAttributeValuesMap.ContainsKey(variant.Id))
                    continue;

                var variantKey = GetCombinationKey(variantAttributeValuesMap[variant.Id]);
                processedCombinations.Add(variantKey);

                if (newCombinations.Contains(variantKey))
                {
                    bool variantNeedsUpdate = false;
                    
                    // This variant matches a new combination - unarchive if archived
                    if (variant.IsArchived)
                    {
                        variant.IsArchived = false;
                        variantNeedsUpdate = true;
                    }

                    // Find the matching combination from command to check ApplyProductChanges
                    var matchingCombination = effectiveVariantCombinations?.FirstOrDefault(c => 
                        GetCombinationKey(c.AttributeValues) == variantKey);
                    
                    if (matchingCombination != null)
                    {
                        // Handle ApplyProductChanges flag
                        if (matchingCombination.ApplyProductChanges)
                        {
                            // If the incoming combination has ApplyProductChanges=true, update the variant
                            // Set ApplyProductChanges to true even if it was false in database
                            if (!variant.ApplyProductChanges)
                            {
                                variant.ApplyProductChanges = true;
                                variantNeedsUpdate = true;
                            }
                            
                            // Apply product changes to this variant
                            variant.MaxDiscount = product.MaxDiscount;
                            variant.ConditionalDiscount = product.ConditionalDiscount;
                            variant.DefaultDiscount = product.DefaultDiscount;
                            variant.DefaultDiscountType = product.DefaultDiscountType;
                            variant.IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice;
                            variant.IsSales= product.IsSales;
                            variant.IsPurchase = product.IsPurchase;
                            variant.Model = product.Model;
                            variant.Version = product.Version;
                            variant.CountryOfOrigin = product.CountryOfOrigin;
                            variantNeedsUpdate = true;

                            // Update suppliers
                            await UpdateVariantSuppliers(command.SuppliersIds, variant.Id);

                            // Update sales taxes
                            await UpdateVariantSalesTaxes(command.SalesTaxIds, variant.Id);

                            // Update purchase taxes
                            await UpdateVariantPurchaseTaxes(command.PurchaseTaxIds, variant.Id);

                            // Update manufacturer companies
                            await UpdateVariantManufacturerCompanies(command.ManufacturerCompaniesIds, variant.Id);

                            // Update cost centers
                            await UpdateVariantCostCenters(command.CostCenters, variant.Id);

                            // Update selling price discounts
                            if (product.IsDiscountBasedOnSellingPrice)
                            {
                                await UpdateVariantSellingPriceDiscounts(command.SellingPriceDiscounts, variant.Id);
                            }

                            // Update inventory thresholds (same as product when ApplyProductChanges)
                            variant.InventoryThresholdScope = product.InventoryThresholdScope;
                            variant.InventoryThresholdBranchId = product.InventoryThresholdBranchId;
                            await UpdateVariantInventoryThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId, variant.Id);
                            await UpdateVariantExpiryLevels(command.ExpiryLevels, variant.Id);

                            // Update packing units
                            await UpdateVariantPackingUnitsFromProduct(command.PackingUnits, variant.Id);
                        }
                        else
                        {
                            // If the incoming combination has ApplyProductChanges=false
                            // Set ApplyProductChanges to false (even if it was true in database)
                            // Do NOT apply product changes, only unarchive if needed
                            if (variant.ApplyProductChanges)
                            {
                                variant.ApplyProductChanges = false;
                                variantNeedsUpdate = true;
                            }
                        }
                    }

                    // Add to update list if variant was modified (will handle both unarchive and ApplyProductChanges)
                    if (variantNeedsUpdate)
                    {
                        if (!variantsToUpdate.Any(v => v.Id == variant.Id))
                        {
                            variantsToUpdate.Add(variant);
                        }
                    }
                }
                else
                {
                    // This variant doesn't match any new combination
                    // Delete or archive regardless of ApplyProductChanges flag
                    if (!variant.IsUsed)
                    {
                        // Delete unused variants
                        variantsToDelete.Add(variant);
                    }
                    else
                    {
                        // Archive used variants
                        variant.IsArchived = true;
                        variantsToArchive.Add(variant);
                    }
                }
            }

            // Delete unused variants (delete their thresholds and expiry levels first due to FK Restrict)
            if (variantsToDelete.Any())
            {
                var variantIdsToDelete = variantsToDelete.Select(v => v.Id).ToList();
                await _inventoryThresholdRepository.ExecuteDeleteAsync(t =>
                    t.VariantId != null && variantIdsToDelete.Contains(t.VariantId.Value));
                await _expiryLevelRepository.ExecuteDeleteAsync(t =>
                    t.VariantId != null && variantIdsToDelete.Contains(t.VariantId.Value));
                await _variantRepository.Delete(variantsToDelete);
            }

            // Archive used variants
            if (variantsToArchive.Any())
            {
                await _variantRepository.Update(variantsToArchive);
            }

            // Update variants (includes unarchiving and applying product changes)
            if (variantsToUpdate.Any())
            {
                await _variantRepository.Update(variantsToUpdate);
            }

            // Create new variants for combinations that don't exist
            // Calculate starting variant number - find the highest existing variant number or start from 1
            int variantNumber = 1;
            var existingNonDefaultVariants = existingVariants.Where(v => !v.IsDefault && !variantsToDelete.Contains(v)).ToList();
            if (existingNonDefaultVariants.Any())
            {
                // Try to extract variant numbers from codes (format: PRODUCTCODE-NUMBER)
                var maxNumber = 0;
                foreach (var variant in existingNonDefaultVariants)
                {
                    var code = variant.Code?.Code ?? string.Empty;
                    var match = Regex.Match(code, @"-(\d+)$");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out var num))
                    {
                        maxNumber = Math.Max(maxNumber, num);
                    }
                }
                variantNumber = maxNumber + 1;
            }
            
            var newVariantCommands = new List<VariantCreateCommand>();

            foreach (var combination in effectiveVariantCombinations ?? [])
            {
                var combinationKey = GetCombinationKey(combination.AttributeValues);
                
                // Check if this combination already exists (not in processed combinations means it's new)
                if (!processedCombinations.Contains(combinationKey))
                {
                    var variantCode = $"{command.Code}-{variantNumber}";
                    
                    var variantCreateCommand = new VariantCreateCommand
                    {
                        Name = combination.Name,
                        NameSecondLanguage = combination.Name,
                        Description = $"Variant of {product.Name}",
                        ProductId = product.Id,
                        Code = variantCode,
                        Gs1Code = null,
                        EGSCode = null,
                        BarCodes = [],
                        MaxDiscount = product.MaxDiscount,
                        ConditionalDiscount = product.ConditionalDiscount,
                        DefaultDiscount = product.DefaultDiscount,
                        DefaultDiscountType = product.DefaultDiscountType,
                        IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
                        IsSales = product.IsSales,
                        IsPurchase = product.IsPurchase,
                        ApplyProductChanges = combination.ApplyProductChanges,
                        Model = product.Model,
                        Version = product.Version,
                        CountryOfOrigin = product.CountryOfOrigin,
                        SuppliersIds = command.SuppliersIds,
                        SalesTaxIds = command.SalesTaxIds,
                        PurchaseTaxIds = command.PurchaseTaxIds,
                        ManufacturerCompaniesIds = command.ManufacturerCompaniesIds,
                        CostCenters = command.CostCenters.Select(e => new VariantCostCenterDto
                        {
                            CostCenterId = e.CostCenterId,
                            Percent = e.Percent
                        }).ToList(),
                        SellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => new VariantSellingPriceDiscountDto
                        {
                            SellingPriceId = e.SellingPriceId,
                            Discount = e.Discount,
                            DiscountType = e.DiscountType
                        }).ToList(),
                        VariantAttachments = [],
                        VariantAttributeValues = combination.AttributeValues.Select(kvp => new VariantAttributeValueDto
                        {
                            AttributeDefinitionId = kvp.Key,
                            AttributeValueId = kvp.Value
                        }).ToList(),
                        IsDefault = false,
                        IsArchived = false
                    };

                    newVariantCommands.Add(variantCreateCommand);
                    variantNumber++;
                }
            }

            if (newVariantCommands.Any())
            {
                var bulkCreateResult = await _variantService.CreateVariantsBulkForProductUpdate(newVariantCommands, false);
                if (!bulkCreateResult.IsSuccess)
                {
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        StatusCode = bulkCreateResult.StatusCode,
                        ErrorMessages = bulkCreateResult.ErrorMessages
                    };
                }
            }

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product variants: {ex}");
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { "VARIANT_UPDATE_FAILED" }
            };
        }
    }

    private async Task UpdateVariantSuppliers(List<Guid> suppliersIds, Guid variantId)
    {
        var oldSuppliers = await _variantSupplierRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        
        if (oldSuppliers.Any())
        {
            await _variantSupplierRepository.Delete(oldSuppliers);
        }
        
        var variantSuppliers = new List<VariantSupplier>();
        foreach (var supplierId in suppliersIds)
        {
            variantSuppliers.Add(
                new VariantSupplier
                {
                    VariantId = variantId,
                    SupplierId = supplierId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                });
        }
        
        if (variantSuppliers.Any())
        {
            await _variantSupplierRepository.Add(variantSuppliers);
        }
    }

    private async Task UpdateVariantSalesTaxes(List<Guid> salesTaxIds, Guid variantId)
    {
        var oldTaxes = await _variantSalesTaxRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();

        if (oldTaxes.Any())
            await _variantSalesTaxRepository.Delete(oldTaxes);

        if (salesTaxIds?.Any() == true)
        {
            var variantSalesTaxes = salesTaxIds.Select(taxId => new VariantSalesTax
            {
                VariantId = variantId,
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ToList();
            await _variantSalesTaxRepository.Add(variantSalesTaxes);
        }
    }

    private async Task UpdateVariantPurchaseTaxes(List<Guid> purchaseTaxIds, Guid variantId)
    {
        var oldTaxes = await _variantPurchaseTaxRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();

        if (oldTaxes.Any())
            await _variantPurchaseTaxRepository.Delete(oldTaxes);

        if (purchaseTaxIds?.Any() == true)
        {
            var variantPurchaseTaxes = purchaseTaxIds.Select(taxId => new VariantPurchaseTax
            {
                VariantId = variantId,
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ToList();
            await _variantPurchaseTaxRepository.Add(variantPurchaseTaxes);
        }
    }

    private async Task UpdateVariantManufacturerCompanies(List<Guid> companiesIds, Guid variantId)
    {
        var oldCompanies = await _variantManufacturerCompanyRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        
        if (oldCompanies.Any())
        {
            await _variantManufacturerCompanyRepository.Delete(oldCompanies);
        }
        
        var variantManufacturerCompanies = new List<VariantManufacturerCompany>();
        foreach (var companyId in companiesIds)
        {
            variantManufacturerCompanies.Add(
                new VariantManufacturerCompany
                {
                    VariantId = variantId,
                    ManufacturerCompanyId = companyId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                });
        }
        
        if (variantManufacturerCompanies.Any())
        {
            await _variantManufacturerCompanyRepository.Add(variantManufacturerCompanies);
        }
    }

    private async Task UpdateVariantCostCenters(List<ProductCostCenterDto>? productCostCenters, Guid variantId)
    {
        var repository = _variantCostCenterRepository;

        var existingCostCenters = await repository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();

        if (productCostCenters == null || !productCostCenters.Any())
        {
            if (existingCostCenters.Any())
            {
                await repository.Delete(existingCostCenters);
            }
            return;
        }

        var existingByCostCenterId = existingCostCenters
            .ToDictionary(e => e.CostCenterId, e => e);
        var incomingIds = productCostCenters
            .Select(d => d.CostCenterId)
            .ToHashSet();

        var toRemove = existingCostCenters
            .Where(e => !incomingIds.Contains(e.CostCenterId))
            .ToList();
        if (toRemove.Any())
        {
            await repository.Delete(toRemove);
        }

        var now = DateTime.Now;
        var toAdd = new List<VariantCostCenter>();
        var toUpdate = new List<VariantCostCenter>();

        foreach (var dto in productCostCenters)
        {
            if (existingByCostCenterId.TryGetValue(dto.CostCenterId, out var existing))
            {
                existing.Percent = dto.Percent;
                existing.ModifiedAt = now;
                toUpdate.Add(existing);
            }
            else
            {
                toAdd.Add(new VariantCostCenter
                {
                    VariantId = variantId,
                    CostCenterId = dto.CostCenterId,
                    Percent = dto.Percent,
                    CreatedAt = now,
                    ModifiedAt = now
                });
            }
        }

        if (toAdd.Any())
        {
            await repository.Add(toAdd);
        }

        if (toUpdate.Any())
        {
            await repository.Update(toUpdate);
        }
    }

    private async Task UpdateVariantSellingPriceDiscounts(List<ProductSellingPriceDiscountDto> productSellingPriceDiscounts, Guid variantId)
    {
        var variantSellingPriceDiscountRepository = _variantSellingPriceDiscountRepository;
        
        // Remove existing discounts
        var existingDiscounts = await variantSellingPriceDiscountRepository.GetQuery()
            .Where(psd => psd.VariantId == variantId)
            .ToListAsync();
        
        if (existingDiscounts.Any())
        {
            await variantSellingPriceDiscountRepository.Delete(existingDiscounts);
        }
        
        // Add new discounts
        if (productSellingPriceDiscounts?.Any() == true)
        {
            var discounts = productSellingPriceDiscounts.Select(e => new VariantSellingPriceDiscount
            {
                VariantId = variantId,
                SellingPriceId = e.SellingPriceId,
                Discount = e.Discount,
                DiscountType = e.DiscountType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ToList();
            
            await variantSellingPriceDiscountRepository.Add(discounts);
        }
    }

    private async Task UpdateVariantPackingUnitsFromProduct(List<ProductPackingUnitDto>? productPackingUnits, Guid variantId)
    {
        var variantPackingUnitRepository = _variantPackingUnitRepository;
        
        // Get existing variant packing units
        var existingPackingUnits = await variantPackingUnitRepository.GetQuery()
            .Where(vpu => vpu.VariantId == variantId)
            .ToListAsync();

        // If product packing units is null or empty, remove all variant packing units
        if (productPackingUnits == null || !productPackingUnits.Any())
        {
            if (existingPackingUnits.Any())
            {
                await variantPackingUnitRepository.Delete(existingPackingUnits);
            }
            return;
        }

        // Find packing units to remove (ones not in the product packing units)
        var packingUnitsToRemove = existingPackingUnits
            .Where(e => !productPackingUnits.Any(p => p.PackingUnitId == e.PackingUnitId))
            .ToList();

        // Remove deleted packing units
        if (packingUnitsToRemove.Any())
        {
            await variantPackingUnitRepository.Delete(packingUnitsToRemove);
        }

        // Process each packing unit from product
        var packingUnitsToAdd = new List<VariantPackingUnit>();
        var packingUnitsToUpdate = new List<VariantPackingUnit>();

        foreach (var packingUnitDto in productPackingUnits)
        {
            var existingPackingUnit = existingPackingUnits.FirstOrDefault(e => e.PackingUnitId == packingUnitDto.PackingUnitId);
            
            if (existingPackingUnit == null)
            {
                // Add new packing unit
                packingUnitsToAdd.Add(new VariantPackingUnit
                {
                    PackingUnitId = packingUnitDto.PackingUnitId,
                    VariantId = variantId,
                    AverageCostPrice = packingUnitDto.AverageCostPrice,
                    IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                    LastCostPrice = packingUnitDto.LastCostPrice,
                    PartsCount = packingUnitDto.PartsCount,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDefaultSales = packingUnitDto.IsDefaultSales,
                    IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                    OrderNumber = packingUnitDto.OrderNumber,
                    VariantPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(e => new VariantPackingUnitSellingPrice
                    {
                        Amount = e.Amount,
                        SellingPriceId = e.SellingPriceId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                    }).ToList()
                });
            }
            else
            {
                // Update existing packing unit
                existingPackingUnit.PackingUnitId = packingUnitDto.PackingUnitId;
                existingPackingUnit.AverageCostPrice = packingUnitDto.AverageCostPrice;
                existingPackingUnit.IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit;
                existingPackingUnit.LastCostPrice = packingUnitDto.LastCostPrice;
                existingPackingUnit.PartsCount = packingUnitDto.PartsCount;
                existingPackingUnit.IsDefaultSales = packingUnitDto.IsDefaultSales;
                existingPackingUnit.IsDefaultPurchases = packingUnitDto.IsDefaultPurchases;
                existingPackingUnit.OrderNumber = packingUnitDto.OrderNumber;
                existingPackingUnit.ModifiedAt = DateTime.Now;
                packingUnitsToUpdate.Add(existingPackingUnit);

                // Update selling prices
                await UpdateVariantPackingUnitSellingPrices(packingUnitDto, existingPackingUnit);
            }
        }

        // Add new packing units
        if (packingUnitsToAdd.Any())
        {
            await variantPackingUnitRepository.Add(packingUnitsToAdd);
        }

        // Batch update existing packing units
        if (packingUnitsToUpdate.Any())
        {
            await variantPackingUnitRepository.Update(packingUnitsToUpdate);
        }
    }

    private async Task UpdateVariantPackingUnitSellingPrices(ProductPackingUnitDto packingUnitDto, VariantPackingUnit existingPackingUnit)
    {
        var existingPrices = await _variantPackingUnitSellingPriceRepository.GetQuery()
            .Where(e => e.VariantPackingUnitId == existingPackingUnit.Id)
            .ToListAsync();

        var pricesToAdd = new List<VariantPackingUnitSellingPrice>();
        var pricesToUpdate = new List<VariantPackingUnitSellingPrice>();
        var pricesToRemove = existingPrices
            .Where(e => !packingUnitDto.SellingPrices.Any(p => p.SellingPriceId == e.SellingPriceId))
            .ToList();

        // Remove prices that are no longer in the product packing unit
        if (pricesToRemove.Any())
        {
            await _variantPackingUnitSellingPriceRepository.Delete(pricesToRemove);
        }

        foreach (var priceDto in packingUnitDto.SellingPrices)
        {
            var existingPrice = existingPrices.FirstOrDefault(e => e.SellingPriceId == priceDto.SellingPriceId);
            
            if (existingPrice == null)
            {
                // Add new price
                pricesToAdd.Add(new VariantPackingUnitSellingPrice
                {
                    VariantPackingUnitId = existingPackingUnit.Id,
                    SellingPriceId = priceDto.SellingPriceId,
                    Amount = priceDto.Amount,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
            else
            {
                // Update existing price
                existingPrice.Amount = priceDto.Amount;
                existingPrice.ModifiedAt = DateTime.Now;
                pricesToUpdate.Add(existingPrice);
            }
        }

        if (pricesToAdd.Any())
        {
            await _variantPackingUnitSellingPriceRepository.Add(pricesToAdd);
        }

        if (pricesToUpdate.Any())
        {
            await _variantPackingUnitSellingPriceRepository.Update(pricesToUpdate);
        }
    }

    private string GetCombinationKey(Dictionary<Guid, Guid> attributeValues)
    {
        // Create a unique key for the combination by sorting attribute definition IDs
        var sortedKeys = attributeValues.Keys.OrderBy(k => k).ToList();
        return string.Join("|", sortedKeys.Select(k => $"{k}:{attributeValues[k]}"));
    }

    #region Validations

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ProductCreateCommand command)
    {
        var result = await base.ValidateCreate(command);
        if (!result.isValid)
            return result;
        
        // Only products of type Stock can have combination variants
        if (command.ProductType != ProductType.Stock && command.VariantCombinations?.Any() == true)
        {
            result.errors.Add("Variant combinations are only allowed for products of type Stock");
            result.isValid = false;
        }

        var isCodeExist = await _codeRepository.GetQuery().AnyAsync(e=>e.Code==command.Code);
        if (isCodeExist)
        {
            result.errors.Add("Product code already exists");
            result.isValid = false;
        }

        var isGs1Exist = await _codeRepository.GetQuery().AnyAsync(e => e.Code == command.Gs1Code);
        if (isGs1Exist)
        {
            result.errors.Add("Product GS1 already exists");
            result.isValid = false;
        }
        var isEgsExist = await _codeRepository.GetQuery().AnyAsync(e => e.Code == command.EGSCode);
        if (isEgsExist)
        {
            result.errors.Add("Product EGS already exists");
            result.isValid = false;
        }
        var isBarCodeExist = await _codeRepository.GetQuery().AnyAsync(e => command.BarCodes.Contains(e.Code));
        if (isBarCodeExist)
        {
            result.errors.Add("One of barcodes  already exists");
            result.isValid = false;
        }
        var (thresholdsValid, thresholdErrors) = InventoryThresholdValidationHelper.ValidateThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
        if (!thresholdsValid)
            result.errors.AddRange(thresholdErrors);
        result.isValid = result.isValid && thresholdsValid;
        var (expiryLevelsValid, expiryLevelErrors) = ExpiryLevelValidationHelper.ValidateExpiryLevels(command.ExpiryLevels);
        if (!expiryLevelsValid)
            result.errors.AddRange(expiryLevelErrors);
        result.isValid = result.isValid && expiryLevelsValid;
        return result;
        
    }

    protected override async Task<(bool isValid, List<string> errors, Product? entity)> ValidateUpdate(ProductUpdateCommand command)
    {
        var result =  await base.ValidateUpdate(command);    
        
        var isBarCodeExist = await _codeRepository.GetQuery().AnyAsync(e => command.BarCodes.Contains(e.Code) && e.ProductId != command.Id);
        if (isBarCodeExist)
        {
            result.errors.Add("One of barcodes is associated with another product");
            result.isValid = false;
        }
        var (thresholdsValid, thresholdErrors) = InventoryThresholdValidationHelper.ValidateThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
        if (!thresholdsValid)
            result.errors.AddRange(thresholdErrors);
        result.isValid = result.isValid && thresholdsValid;
        var (expiryLevelsValid, expiryLevelErrors) = ExpiryLevelValidationHelper.ValidateExpiryLevels(command.ExpiryLevels);
        if (!expiryLevelsValid)
            result.errors.AddRange(expiryLevelErrors);
        result.isValid = result.isValid && expiryLevelsValid;
        return result;
    }

    private List<InventoryThreshold> CreateProductInventoryThresholds(ProductCreateCommand command)
    {
        if (command.InventoryThresholds == null || !command.InventoryThresholds.Any())
            return new List<InventoryThreshold>();
        var scope = command.InventoryThresholdScope ?? InventoryThresholdScope.All;
        var branchId = command.InventoryThresholdBranchId;
        var now = DateTime.Now;
        return command.InventoryThresholds.Select((dto, index) => new InventoryThreshold
        {
            OwnerType = OwnerType.Product,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            MinQuantity = dto.MinQuantity,
            MaxQuantity = dto.MaxQuantity,
            Scope = scope,
            BranchId = branchId,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
    }

    private async Task UpdateProductInventoryThresholds(Guid productId, List<InventoryThresholdDto>? inventoryThresholds, InventoryThresholdScope? scope, Guid? branchId)
    {
        var repository = _inventoryThresholdRepository;
        var existing = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();
        if (existing.Any())
            await repository.Delete(existing);
        if (inventoryThresholds == null || !inventoryThresholds.Any())
            return;
        var scopeValue = scope ?? InventoryThresholdScope.All;
        var now = DateTime.Now;
        var toAdd = inventoryThresholds.Select((dto, index) => new InventoryThreshold
        {
            ProductId = productId,
            OwnerType = OwnerType.Product,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            MinQuantity = dto.MinQuantity,
            MaxQuantity = dto.MaxQuantity,
            Scope = scopeValue,
            BranchId = branchId,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
        await repository.Add(toAdd);
    }

    private List<ExpiryLevel> CreateProductExpiryLevels(ProductCreateCommand command)
    {
        if (command.ExpiryLevels == null || !command.ExpiryLevels.Any())
            return new List<ExpiryLevel>();
        var now = DateTime.Now;
        return command.ExpiryLevels.Select((dto, index) => new ExpiryLevel
        {
            OwnerType = OwnerType.Product,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            DaysFrom = dto.DaysFrom,
            DaysTo = dto.DaysTo,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
    }

    private async Task UpdateProductExpiryLevels(Guid productId, List<ExpiryLevelDto>? expiryLevels)
    {
        var repository = _expiryLevelRepository;
        var existing = await repository.GetQuery()
            .Where(e => e.ProductId == productId)
            .ToListAsync();
        if (existing.Any())
            await repository.Delete(existing);
        if (expiryLevels == null || !expiryLevels.Any())
            return;
        var now = DateTime.Now;
        var toAdd = expiryLevels.Select((dto, index) => new ExpiryLevel
        {
            ProductId = productId,
            OwnerType = OwnerType.Product,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            DaysFrom = dto.DaysFrom,
            DaysTo = dto.DaysTo,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
        await repository.Add(toAdd);
    }

    private async Task UpdateVariantExpiryLevels(List<ExpiryLevelDto>? expiryLevels, Guid variantId)
    {
        var repository = _expiryLevelRepository;
        var existing = await repository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        if (existing.Any())
            await repository.Delete(existing);
        if (expiryLevels == null || !expiryLevels.Any())
            return;
        var now = DateTime.Now;
        var toAdd = expiryLevels.Select((dto, index) => new ExpiryLevel
        {
            VariantId = variantId,
            OwnerType = OwnerType.Variant,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            DaysFrom = dto.DaysFrom,
            DaysTo = dto.DaysTo,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
        await repository.Add(toAdd);
    }

    private async Task UpdateVariantInventoryThresholds(List<InventoryThresholdDto>? inventoryThresholds, InventoryThresholdScope? scope, Guid? branchId, Guid variantId)
    {
        var repository = _inventoryThresholdRepository;
        var existing = await repository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        if (existing.Any())
            await repository.Delete(existing);
        if (inventoryThresholds == null || !inventoryThresholds.Any())
            return;
        var scopeValue = scope ?? InventoryThresholdScope.All;
        var now = DateTime.Now;
        var toAdd = inventoryThresholds.Select((dto, index) => new InventoryThreshold
        {
            VariantId = variantId,
            OwnerType = OwnerType.Variant,
            Level = dto.Level > 0 ? dto.Level : index + 1,
            MinQuantity = dto.MinQuantity,
            MaxQuantity = dto.MaxQuantity,
            Scope = scopeValue,
            BranchId = branchId,
            EnableNotification = dto.EnableNotification,
            CreatedAt = now,
            ModifiedAt = now
        }).ToList();
        await repository.Add(toAdd);
    }
    #endregion
}