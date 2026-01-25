using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Products;
using ERP.Domain.Commands.Inventory.Variants;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Products;
using System.Text.RegularExpressions;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Shared.BaseEntities.Attachments;

namespace ERP.Infrastracture.Services.Inventory;

public class ProductService : BaseTreeSettingService<Product, ProductCreateCommand, ProductUpdateCommand>, IProductService
{
    private readonly IProductRepository _repository;
    private readonly IBaseRepository<ProductCode> _codeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVariantService _variantService;

    public ProductService(IUnitOfWork unitOfWork, IVariantService variantService) : base(unitOfWork.ProductRepository)
    {
        _repository = unitOfWork.ProductRepository;
        _unitOfWork = unitOfWork;
        _codeRepository = _unitOfWork.ProductCodeRepository;
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
            await _repository.Add(product);
            await _unitOfWork.ProductCodeRepository.Add(new ProductCode
            {
                Code = command.Code,
                CodeType = ProductCodeType.Code,
                ProductId = product.Id,
            });

            if (command.NodeType == NodeType.Domain)
            {

                if (!string.IsNullOrEmpty(command.EGSCode))
                {
                    await _unitOfWork.ProductCodeRepository.Add(new ProductCode
                    {
                        Code = command.EGSCode,
                        CodeType = ProductCodeType.EGS,
                        ProductId = product.Id,
                    });
                }
                if (!string.IsNullOrEmpty(command.Gs1Code))
                {
                    await _unitOfWork.ProductCodeRepository.Add(new ProductCode
                    {
                        Code = command.Gs1Code,
                        CodeType = ProductCodeType.GS1,
                        ProductId = product.Id
                    });
                }
                product.Model = command.Model;
                product.Version = command.Version;
                product.CountryOfOrigin = command.CountryOfOrigin;
                product.ProductType = command.ProductType;
                product.ConditionalDiscount = command.ConditionalDiscount;
                product.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                product.DefaultDiscount = command.IsDiscountBasedOnSellingPrice ? 0 : command.DefaultDiscount;
                product.DefaultDiscountType = command.DefaultDiscountType;
                product.MaxDiscount = command.MaxDiscount;
                product.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                product.IsSales = command.IsSales;
                product.IsPurchase = command.IsPurchase;
                product.TrackedBy = command.TrackedBy;
                await _repository.Update(product);
                if (command.IsDiscountBasedOnSellingPrice)
                { 
                    await CreateProductSellingPriceDiscounts(command.SellingPriceDiscounts,product.Id);
                }
                await CreateProductSuppliers(command,product.Id);
                await CreateProductManufacturerCompanies(command, product.Id);
                await CreateProductBarcodes(command, product);

                await CreateProductPackingUnits(product.Id,command.PackingUnits);

                // Create product attributes if provided
                if (command.ProductAttributeDefinitions?.Any() == true)
                {
                    await CreateProductAttributes(product.Id, command.ProductAttributeDefinitions);
                }
            }

            // Handle product attachments
            if (command.ProductAttachments != null && command.ProductAttachments.Any())
                await UpdateProductAttachments(command.ProductAttachments.Where(e=> command.NodeType == NodeType.Domain ||(e.AttachmentType == AttachmentType.Picture && e.IsPrimary) ), product.Id);

            // Create variants for Domain products
            if (command.NodeType == NodeType.Domain)
            {
                var variantCreationResult = await CreateProductVariants(product, command);
                if (!variantCreationResult.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return new ApiResponse<Product>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessages = variantCreationResult.ErrorMessages
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
            return new ApiResponse<Product> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }

    private async Task CreateProductBarcodes(ProductCreateCommand command, Product product)
    {
        var barCodes = new List<ProductCode>();

        foreach (var barcode in command.BarCodes)
            barCodes.Add(
                new ProductCode
                {
                    Code = barcode,
                    CodeType = ProductCodeType.BarCode,
                    ProductId = product.Id
                });

        await _unitOfWork.ProductCodeRepository.Add(barCodes);
    }

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
                product.ProductType = command.ProductType;
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
                await UpdateProductManufacturerCompanies(command.ManufacturerCompaniesIds, product.Id);
                await UpdateProductSellingPriceDiscounts(product.Id,command.SellingPriceDiscounts);
                await UpdateProductPackingUnits(product.Id,command.PackingUnits);

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
            return new ApiResponse<Product> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
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
            var attachmentsIds = await _unitOfWork.ProductAttachmentRepository.GetQuery().Where(e => e.ProductId == id)
                .Select(e=>e.AttachmentId).ToListAsync();
            // Delete Product first (this will cascade to ProductAttachments)
            await _repository.Delete(id);
            
            // Force SaveChanges to complete the cascade deletion of ProductAttachments
            // This resolves the constraint conflict before deleting Attachments
            // await _unitOfWork.SaveChanges();
            
            // Now safely delete orphaned Attachments (no constraint conflicts)

            await _unitOfWork.AttachmentRepository.ExecuteDeleteAsync(e=> attachmentsIds.Contains(e.Id));
            
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
        var productAttributeDefinitionRepository = _unitOfWork.ProductAttributeDefinitionRepository;
        
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
        var productAttributeDefinitionRepository = _unitOfWork.ProductAttributeDefinitionRepository;
        
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
        
        await _unitOfWork.ProductSellingPriceDiscountRepository.Add(discounts);
    }

    private async Task UpdateProductPackingUnits(Guid productId, List<ProductPackingUnitDto>? productPackingUnits)
    {
        var productPackingUnitRepository = _unitOfWork.ProductPackingUnitRepository;
        
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

                await productPackingUnitRepository.Update(existingPackingUnit);

                // Update selling prices
                await UpdateProductPackingUnitSellingPrices(packingUnitDto, existingPackingUnit);
            }
        }

        // Add new packing units
        if (packingUnitsToAdd.Any())
        {
            await productPackingUnitRepository.Add(packingUnitsToAdd);
        }
    }

    private async Task UpdateProductPackingUnitSellingPrices(ProductPackingUnitDto packingUnitDto, ProductPackingUnit existingPackingUnit)
    {
        var existingPrices = await _unitOfWork.ProductPackingUnitSellingPriceRepository.GetQuery()
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
            await _unitOfWork.ProductPackingUnitSellingPriceRepository.Delete(pricesToRemove);
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
            await _unitOfWork.ProductPackingUnitSellingPriceRepository.Add(pricesToAdd);
        }

        if (pricesToUpdate.Any())
        {
            await _unitOfWork.ProductPackingUnitSellingPriceRepository.Update(pricesToUpdate);
        }
    }

    private async Task UpdateProductSellingPriceDiscounts(Guid productId, List<ProductSellingPriceDiscountDto> productSellingPriceDiscounts)
    {
        var productSellingPriceDiscountRepository = _unitOfWork.ProductSellingPriceDiscountRepository;
        
        // Remove existing discounts
        var existingDiscounts = await productSellingPriceDiscountRepository.GetQuery()
            .Where(psd => psd.ProductId == productId)
            .ToListAsync();
        
        foreach (var discount in existingDiscounts)
        {
            await productSellingPriceDiscountRepository.Delete(discount);
        }
        
        // Add new discounts
        await CreateProductSellingPriceDiscounts(productSellingPriceDiscounts, productId);
    }


    private async Task CreateProductManufacturerCompanies(ProductCreateCommand command,Guid productId)
    {
        var productManufacturerCompanies = new List<ProductManufacturerCompany>();
        foreach (var companyId in command.ManufacturerCompaniesIds)
            productManufacturerCompanies.Add(
                new ProductManufacturerCompany
                {
                    ManufacturerCompanyId = companyId,
                    ProductId = productId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt= DateTime.Now,
        
                });
        
        await _unitOfWork.ProductManufacturerCompanyRepository.Add(productManufacturerCompanies);
    }


    private async Task UpdateProductManufacturerCompanies(List<Guid> companiesIds, Guid productId)
    {
        var oldCompanies = await _unitOfWork.ProductManufacturerCompanyRepository.GetQuery().Where(e => e.ProductId == productId).ToListAsync();
        await _unitOfWork.ProductManufacturerCompanyRepository.Delete(oldCompanies);
        
        var productManufacturerCompanies = new List<ProductManufacturerCompany>();
        foreach (var companyId in companiesIds)
            productManufacturerCompanies.Add(
                new ProductManufacturerCompany
                {
                    ManufacturerCompanyId = companyId,
                    ProductId = productId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
        
                });
        
        await _unitOfWork.ProductManufacturerCompanyRepository.Add(productManufacturerCompanies);
    }
    private async Task UpdateProductBarCodes(List<string> barCodes, Guid productId)
    {
        // Normalize input (avoid duplicates & nulls)
        var newCodes = barCodes
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Distinct()
            .ToHashSet();

        var oldEntities = await _unitOfWork.ProductCodeRepository
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
            await _unitOfWork.ProductCodeRepository.Delete(removedEntities);

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
            await _unitOfWork.ProductCodeRepository.Add(addedCodes);
    }


    private async Task CreateProductPackingUnits(Guid productId,List<ProductPackingUnitDto> command)
    {
        List<ProductPackingUnit> packingUnits = [];
        
        foreach (var itempackingUnit in command)
            packingUnits.Add(
                new ProductPackingUnit
                {
                    PackingUnitId = itempackingUnit.PackingUnitId,
                    ProductId=productId,
                    AverageCostPrice = itempackingUnit.AverageCostPrice,
                    IsDefaultPackingUnit = itempackingUnit.IsDefaultPackingUnit,
                    LastCostPrice = itempackingUnit.LastCostPrice,
                    PartsCount = itempackingUnit.PartsCount,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDefaultSales = itempackingUnit.IsDefaultSales,
                    IsDefaultPurchases = itempackingUnit.IsDefaultPurchases,
                    OrderNumber = itempackingUnit.OrderNumber,
                    ProductPackingUnitSellingPrices = itempackingUnit.SellingPrices.Select(e => new ProductPackingUnitSellingPrice
                    {
                        Amount = e.Amount,
                        SellingPriceId = e.SellingPriceId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
        
                    }).ToList()
                });
        
        await _unitOfWork.ProductPackingUnitRepository.Add(packingUnits);
    }

    private async Task  CreateProductSuppliers(ProductCreateCommand command,Guid productId)
    {
        var productSuppliers = new List<ProductSupplier>();
        foreach (var supplierId in command.SuppliersIds)
            productSuppliers.Add(
                new ProductSupplier
                {
                    ProductId = productId,
                    SupplierId = supplierId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                });
        
        await _unitOfWork.ProductSupplierRepository.Add(productSuppliers);
    }

    private async Task UpdateProductSuppliers(List<Guid> suppliersIds, Guid productId)
    {
        var oldSuppliers = await _unitOfWork.ProductSupplierRepository.GetQuery().Where(e=>e.ProductId == productId).ToListAsync();
        await _unitOfWork.ProductSupplierRepository.Delete(oldSuppliers);
        
        var productSuppliers = new List<ProductSupplier>();
        foreach (var supplierId in suppliersIds)
            productSuppliers.Add(
                new ProductSupplier
                {
                    ProductId = productId,
                    SupplierId = supplierId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                });
        
        await _unitOfWork.ProductSupplierRepository.Add(productSuppliers);
    }

    private async Task UpdateProductAttachments(IEnumerable<ProductAttachmentDto> productAttachmentDtos, Guid productId)
    {
        await DeleteOldProductAttachments(productId);

        await AddNewProductAttachments(productAttachmentDtos, productId);
    }

    private async Task AddNewProductAttachments(IEnumerable<ProductAttachmentDto> productAttachmentDtos, Guid productId)
    {
        foreach (var productAttachmentDto in productAttachmentDtos)
        {
        
            var attachmentDto = productAttachmentDto.Attachment;
        
            if (attachmentDto != null)
            {
                var attachment = new Attachment
                {
                    FileName = attachmentDto.FileName,
                    FileContentType = attachmentDto.ContentType,
                    FileData = Convert.FromBase64String(attachmentDto.FileContent),
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
        
                await _unitOfWork.AttachmentRepository.Add(attachment);
        
                var productAttachment = new ProductAttachment
                {
                    ProductId = productId,
                    AttachmentId = attachment.Id,
                    AttachmentType = productAttachmentDto.AttachmentType,
                    Description = productAttachmentDto.Description,
                    IsPrimary = productAttachmentDto.IsPrimary,
                    DisplayOrder = productAttachmentDto.DisplayOrder,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
        
                await _unitOfWork.ProductAttachmentRepository.Add(productAttachment);
            }
        }
    }

    private async Task DeleteOldProductAttachments(Guid productId)
    {
        var productAttachments = await _unitOfWork.ProductAttachmentRepository.GetQuery().Where(e => e.ProductId == productId).ToListAsync();
        var attachmentIds = productAttachments.Select(e => e.AttachmentId);
        if(productAttachments.Any())
            await _unitOfWork.ProductAttachmentRepository.Delete(productAttachments);
        foreach (var attachmentId in attachmentIds)
            await _unitOfWork.AttachmentRepository.Delete(attachmentId);
    }

    private async Task<ApiResponse<bool>> CreateProductVariants(Product product, ProductCreateCommand command)
    {
        try
        {
            bool hasVariantCombinations = command.VariantCombinations?.Any() == true;

            // Create default variant
            // IsActive = false if there are variant combinations (default is archived), true otherwise
            var defaultVariantCommand = new VariantCreateCommand
            {
                Name = product.Name ?? "Default Variant",
                NameSecondLanguage = product.NameSecondLanguage,
                Description = "Default variant",
                ProductId = product.Id,
                Code = command.Code, // Default variant gets the product code
                Gs1Code = command.Gs1Code,
                EGSCode = command.EGSCode,
                BarCodes = command.BarCodes,
                MaxDiscount = command.MaxDiscount,
                ConditionalDiscount = command.ConditionalDiscount,
                DefaultDiscount = command.IsDiscountBasedOnSellingPrice ? 0 : command.DefaultDiscount,
                DefaultDiscountType = command.DefaultDiscountType,
                IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice,
                Model = command.Model,
                Version = command.Version,
                CountryOfOrigin = command.CountryOfOrigin,
                SuppliersIds = command.SuppliersIds,
                ManufacturerCompaniesIds = command.ManufacturerCompaniesIds,
                IsPurchase = command.IsPurchase,
                IsSales = command.IsSales,
                SellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => e.Adapt<VariantSellingPriceDiscountDto>()).ToList(),
                PackingUnits = command.PackingUnits.Select(e => e.Adapt<VariantPackingUnitDto>()).ToList(),
                VariantAttachments = [],
                VariantAttributeValues = [],
                IsDefault = true,
                IsArchived = hasVariantCombinations // Default variant is inactive (archived) if there are combination variants
            };

            var defaultVariantResult = await _variantService.Create(defaultVariantCommand, false);
            if (!defaultVariantResult.IsSuccess)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessages = defaultVariantResult.ErrorMessages
                };
            }

            // Create combination variants if provided
            if (hasVariantCombinations)
            {
                int variantNumber = 1;
                foreach (var combination in command.VariantCombinations!)
                {
                    var variantCode = $"{command.Code}-{variantNumber}";
                    
                    var combinationVariantCommand = new VariantCreateCommand
                    {
                        Name = combination.Name,
                        NameSecondLanguage = combination.Name,
                        Description = $"Variant of {product.Name}",
                        ProductId = product.Id,
                        Code = variantCode,
                        Gs1Code = null, // Combination variants don't inherit GS1/EGS codes
                        EGSCode = null,
                        BarCodes = [],
                        MaxDiscount =  product.MaxDiscount ,
                        ConditionalDiscount = product.ConditionalDiscount,
                        DefaultDiscount = product.DefaultDiscount,
                        DefaultDiscountType = product.DefaultDiscountType,
                        IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
                        IsPurchase = command.IsPurchase,
                        IsSales = command.IsSales,
                        ApplyProductChanges = combination.ApplyProductChanges,
                        Model = product.Model,
                        Version = product.Version,
                        CountryOfOrigin = product.CountryOfOrigin,
                        SuppliersIds =  command.SuppliersIds,
                        ManufacturerCompaniesIds =command.ManufacturerCompaniesIds,
                        SellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => e.Adapt<VariantSellingPriceDiscountDto>()).ToList(),
                        PackingUnits = command.PackingUnits.Select(e => e.Adapt<VariantPackingUnitDto>()).ToList(),
                        VariantAttachments = [],
                        VariantAttributeValues = combination.AttributeValues.Select(kvp => new VariantAttributeValueDto
                        {
                            AttributeDefinitionId = kvp.Key,
                            AttributeValueId = kvp.Value
                        }).ToList(),
                        IsDefault = false,
                        IsArchived = false, // Combination variants are active
                    };

                    var combinationVariantResult = await _variantService.Create(combinationVariantCommand, false);
                    if (!combinationVariantResult.IsSuccess)
                    {
                        return new ApiResponse<bool>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorMessages = combinationVariantResult.ErrorMessages
                        };
                    }

                    variantNumber++;
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
            Console.WriteLine($"Error creating product variants: {ex}");
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { "VARIANT_CREATION_FAILED" }
            };
        }
    }

    private async Task<ApiResponse<bool>> UpdateProductVariants(Product product, ProductUpdateCommand command)
    {
        try
        {
            // Get all existing variants for this product
            var existingVariants = await _unitOfWork.VariantRepository.GetByProductIdAsync(product.Id);
            
            // Get variant attribute values for all existing variants (excluding default variant)
            var variantAttributeValuesMap = new Dictionary<Guid, Dictionary<Guid, Guid>>();
            foreach (var variant in existingVariants.Where(v => !v.IsDefault))
            {
                var attributeValues = await _unitOfWork.VariantAttributeValueRepository.GetByVariantIdAsync(variant.Id);
                variantAttributeValuesMap[variant.Id] = attributeValues.ToDictionary(
                    vav => vav.AttributeDefinitionId,
                    vav => vav.AttributeValueId
                );
            }

            // Create a set of new combinations from the command
            var newCombinations = new HashSet<string>();
            if (command.VariantCombinations?.Any() == true)
            {
                foreach (var combination in command.VariantCombinations)
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
                defaultVariant.IsArchived = command.VariantCombinations?.Any() == true;
                
                // Update suppliers
                await UpdateVariantSuppliers(command.SuppliersIds, defaultVariant.Id);

                // Update manufacturer companies
                await UpdateVariantManufacturerCompanies(command.ManufacturerCompaniesIds, defaultVariant.Id);

                // Update selling price discounts
                if (product.IsDiscountBasedOnSellingPrice)
                {
                    await UpdateVariantSellingPriceDiscounts(command.SellingPriceDiscounts, defaultVariant.Id);
                }

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
                    var matchingCombination = command.VariantCombinations.FirstOrDefault(c => 
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

                            // Update manufacturer companies
                            await UpdateVariantManufacturerCompanies(command.ManufacturerCompaniesIds, variant.Id);

                            // Update selling price discounts
                            if (product.IsDiscountBasedOnSellingPrice)
                            {
                                await UpdateVariantSellingPriceDiscounts(command.SellingPriceDiscounts, variant.Id);
                            }

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

            // Delete unused variants
            if (variantsToDelete.Any())
            {
                await _unitOfWork.VariantRepository.Delete(variantsToDelete);
            }

            // Archive used variants
            if (variantsToArchive.Any())
            {
                foreach (var variant in variantsToArchive)
                {
                    await _unitOfWork.VariantRepository.Update(variant);
                }
            }

            // Update variants (includes unarchiving and applying product changes)
            if (variantsToUpdate.Any())
            {
                foreach (var variant in variantsToUpdate)
                {
                    await _unitOfWork.VariantRepository.Update(variant);
                }
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
            
            foreach (var combination in command.VariantCombinations)
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
                        ManufacturerCompaniesIds = command.ManufacturerCompaniesIds,
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

                    var createResult = await _variantService.Create(variantCreateCommand, false);
                    if (!createResult.IsSuccess)
                    {
                        return new ApiResponse<bool>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorMessages = createResult.ErrorMessages
                        };
                    }

                    variantNumber++;
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
        var oldSuppliers = await _unitOfWork.VariantSupplierRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        
        if (oldSuppliers.Any())
        {
            await _unitOfWork.VariantSupplierRepository.Delete(oldSuppliers);
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
            await _unitOfWork.VariantSupplierRepository.Add(variantSuppliers);
        }
    }

    private async Task UpdateVariantManufacturerCompanies(List<Guid> companiesIds, Guid variantId)
    {
        var oldCompanies = await _unitOfWork.VariantManufacturerCompanyRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        
        if (oldCompanies.Any())
        {
            await _unitOfWork.VariantManufacturerCompanyRepository.Delete(oldCompanies);
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
            await _unitOfWork.VariantManufacturerCompanyRepository.Add(variantManufacturerCompanies);
        }
    }

    private async Task UpdateVariantSellingPriceDiscounts(List<ProductSellingPriceDiscountDto> productSellingPriceDiscounts, Guid variantId)
    {
        var variantSellingPriceDiscountRepository = _unitOfWork.VariantSellingPriceDiscountRepository;
        
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
        var variantPackingUnitRepository = _unitOfWork.VariantPackingUnitRepository;
        
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

                await variantPackingUnitRepository.Update(existingPackingUnit);

                // Update selling prices
                await UpdateVariantPackingUnitSellingPrices(packingUnitDto, existingPackingUnit);
            }
        }

        // Add new packing units
        if (packingUnitsToAdd.Any())
        {
            await variantPackingUnitRepository.Add(packingUnitsToAdd);
        }
    }

    private async Task UpdateVariantPackingUnitSellingPrices(ProductPackingUnitDto packingUnitDto, VariantPackingUnit existingPackingUnit)
    {
        var existingPrices = await _unitOfWork.VariantPackingUnitSellingPriceRepository.GetQuery()
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
            await _unitOfWork.VariantPackingUnitSellingPriceRepository.Delete(pricesToRemove);
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
            await _unitOfWork.VariantPackingUnitSellingPriceRepository.Add(pricesToAdd);
        }

        if (pricesToUpdate.Any())
        {
            await _unitOfWork.VariantPackingUnitSellingPriceRepository.Update(pricesToUpdate);
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
        return result;
    }

    #endregion
}