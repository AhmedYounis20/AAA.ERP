using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Repositories.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Application.Repositories;
using ERP.Infrastructure.Services.BaseServices;
using Inventory.Infrastructure.Validation;
using Inventory.Application.Commands.Products;
using Inventory.Application.Commands.Variants;
using Inventory.Application.Services;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.ExpiryLevels;
using Inventory.Domain.Models.Entities.InventoryThresholds;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.Variants;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ERP.Shared.BaseEntities.Attachments;
using Shared.DTOs;

namespace Inventory.Infrastructure.Services;

public class VariantService : BaseService<Variant, VariantCreateCommand, VariantUpdateCommand>, IVariantService
{
    private readonly IVariantRepository _repository;
    private readonly IBaseRepository<VariantCode> _codeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAttributeValueRepository _attributeValueRepository;
    private readonly IProductAttributeDefinitionRepository _productAttributeDefinitionRepository;
    private readonly IVariantAttributeValueRepository _variantAttributeValueRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBaseRepository<InventoryThreshold> _inventoryThresholdRepository;
    private readonly IBaseRepository<ExpiryLevel> _expiryLevelRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IBaseRepository<VariantSupplier> _variantSupplierRepository;
    private readonly IBaseRepository<VariantSalesTax> _variantSalesTaxRepository;
    private readonly IBaseRepository<VariantPurchaseTax> _variantPurchaseTaxRepository;
    private readonly IBaseRepository<VariantCostCenter> _variantCostCenterRepository;
    private readonly IBaseRepository<VariantManufacturerCompany> _variantManufacturerCompanyRepository;
    private readonly IBaseRepository<VariantSellingPriceDiscount> _variantSellingPriceDiscountRepository;
    private readonly IBaseRepository<VariantPackingUnit> _variantPackingUnitRepository;
    private readonly IBaseRepository<VariantPackingUnitSellingPrice> _variantPackingUnitSellingPriceRepository;
    private readonly IBaseRepository<VariantAttachment> _variantAttachmentRepository;
    private readonly IAttachmentRepository _attachmentRepository;

    public VariantService(
        IVariantRepository repository,
        IBaseRepository<VariantCode> codeRepository,
        IAttributeValueRepository attributeValueRepository,
        IProductAttributeDefinitionRepository productAttributeDefinitionRepository,
        IVariantAttributeValueRepository variantAttributeValueRepository,
        IProductRepository productRepository,
        IBaseRepository<InventoryThreshold> inventoryThresholdRepository,
        IBaseRepository<ExpiryLevel> expiryLevelRepository,
        ISupplierRepository supplierRepository,
        IBaseRepository<VariantSupplier> variantSupplierRepository,
        IBaseRepository<VariantSalesTax> variantSalesTaxRepository,
        IBaseRepository<VariantPurchaseTax> variantPurchaseTaxRepository,
        IBaseRepository<VariantCostCenter> variantCostCenterRepository,
        IBaseRepository<VariantManufacturerCompany> variantManufacturerCompanyRepository,
        IBaseRepository<VariantSellingPriceDiscount> variantSellingPriceDiscountRepository,
        IBaseRepository<VariantPackingUnit> variantPackingUnitRepository,
        IBaseRepository<VariantPackingUnitSellingPrice> variantPackingUnitSellingPriceRepository,
        IBaseRepository<VariantAttachment> variantAttachmentRepository,
        IAttachmentRepository attachmentRepository,
        IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _codeRepository = codeRepository;
        _attributeValueRepository = attributeValueRepository;
        _productAttributeDefinitionRepository = productAttributeDefinitionRepository;
        _variantAttributeValueRepository = variantAttributeValueRepository;
        _productRepository = productRepository;
        _inventoryThresholdRepository = inventoryThresholdRepository;
        _expiryLevelRepository = expiryLevelRepository;
        _supplierRepository = supplierRepository;
        _variantSupplierRepository = variantSupplierRepository;
        _variantSalesTaxRepository = variantSalesTaxRepository;
        _variantPurchaseTaxRepository = variantPurchaseTaxRepository;
        _variantCostCenterRepository = variantCostCenterRepository;
        _variantManufacturerCompanyRepository = variantManufacturerCompanyRepository;
        _variantSellingPriceDiscountRepository = variantSellingPriceDiscountRepository;
        _variantPackingUnitRepository = variantPackingUnitRepository;
        _variantPackingUnitSellingPriceRepository = variantPackingUnitSellingPriceRepository;
        _variantAttachmentRepository = variantAttachmentRepository;
        _attachmentRepository = attachmentRepository;
        _unitOfWork = unitOfWork;
    }
    public virtual async Task<ApiResponse<PaginatedResult<VariantDto>>> ReadAllDtosPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        return await ReadAllDtosPaginated(filter,null,cancellationToken);
    }
    public virtual async Task<ApiResponse<PaginatedResult<VariantDto>>> ReadAllDtosPaginated(
        BaseFilterDto filter,
        Expression<Func<Variant, bool>>? additionalFilter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Build filter expression
            Expression<Func<Variant, bool>>? filterExpression = additionalFilter;

            // Add date range filter if specified
            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = CombineFilters(filterExpression, e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value));
            }

            // Determine sort expression
            Expression<Func<Variant, object>>? orderBy = GetOrderByExpression(filter.SortBy);

            var result = await _repository.GetPaginatedDtos(
                filter.PageNumber,
                filter.PageSize,
                filterExpression,
                orderBy,
                filter.SortDescending,
                cancellationToken);

            return new ApiResponse<PaginatedResult<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }
    public override async Task<ApiResponse<Variant>> Create(VariantCreateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateCreate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<Variant>
            {
                IsSuccess = false,
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for Variant + Related items creation
        //await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            Variant variant = new Variant
            {
                // Name = command.Name,
                // NameSecondLanguage = command.NameSecondLanguage,
                Description = command.Description,
                ProductId = command.ProductId,
                MaxDiscount = command.MaxDiscount,
                ConditionalDiscount = command.ConditionalDiscount,
                DefaultDiscount = command.DefaultDiscount,
                DefaultDiscountType = command.DefaultDiscountType,
                IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice,
                Model = command.Model,
                Version = command.Version,
                CountryOfOrigin = command.CountryOfOrigin,
                IsArchived = command.IsArchived,
                ApplyProductChanges = command.ApplyProductChanges,
                IsDefault = command.IsDefault,
                IsSales =  command.IsSales,
                IsPurchase = command.IsPurchase,
                InventoryThresholdScope = command.InventoryThresholdScope,
                InventoryThresholdBranchId = command.InventoryThresholdBranchId
            };

            // Add the variant first to get the ID
            variant = await _repository.Add(variant);

            // Create variant codes
            await CreateVariantCodesForCreate(variant.Id, command);

            // Handle variant suppliers
            if (command.SuppliersIds?.Any() == true)
            {
                await CreateVariantSuppliers(command, variant.Id);
            }

            // Handle variant sales taxes
            if (command.SalesTaxIds?.Any() == true)
            {
                await CreateVariantSalesTaxes(command, variant.Id);
            }

            // Handle variant purchase taxes
            if (command.PurchaseTaxIds?.Any() == true)
            {
                await CreateVariantPurchaseTaxes(command, variant.Id);
            }

            // Handle variant cost centers
            if (command.CostCenters?.Any() == true)
            {
                await CreateVariantCostCenters(command, variant.Id);
            }

            // Handle variant manufacturer companies
            if (command.ManufacturerCompaniesIds?.Any() == true)
            {
                await CreateVariantManufacturerCompanies(command, variant.Id);
            }

            // Handle selling price discounts
            if (command.IsDiscountBasedOnSellingPrice && command.SellingPriceDiscounts?.Any() == true)
            {
                await CreateVariantSellingPriceDiscounts(command, variant.Id);
            }

            // Handle packing units
            if (command.PackingUnits?.Any() == true)
            {
                await CreateVariantPackingUnits(command, variant.Id);
            }

            // Handle variant attachments
            if (command.VariantAttachments?.Any() == true)
            {
                await CreateVariantAttachments(command.VariantAttachments, variant.Id);
            }

            // Create variant attributes if provided
            if (command.VariantAttributeValues?.Any() == true)
            {
                await CreateVariantAttributes(variant.Id, command.VariantAttributeValues);
            }

            // Create inventory thresholds if provided
            if (command.InventoryThresholds?.Any() == true)
            {
                await CreateVariantInventoryThresholds(variant.Id, command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
            }

            // Create expiry levels if provided
            if (command.ExpiryLevels?.Any() == true)
            {
                await CreateVariantExpiryLevels(variant.Id, command.ExpiryLevels);
            }

            // Commit transaction - everything succeeded
            //await _unitOfWork.CommitAsync();

            return new ApiResponse<Variant> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = variant };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            //await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Variant> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }

    public override async Task<ApiResponse<Variant>> Update(VariantUpdateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid || validationResult.entity == null)
        {
            return new ApiResponse<Variant>
            {
                IsSuccess = false,
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for the ENTIRE Variant update process
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            Variant variant = validationResult.entity;
            
            // Update basic properties
            // variant.Name = command.Name;
            // variant.NameSecondLanguage = command.NameSecondLanguage;
            variant.Description = command.Description;
            variant.ProductId = command.ProductId;
            variant.MaxDiscount = command.MaxDiscount;
            variant.ConditionalDiscount = command.ConditionalDiscount;
            variant.DefaultDiscount = command.DefaultDiscount;
            variant.DefaultDiscountType = command.DefaultDiscountType;
            variant.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
            variant.Model = command.Model;
            variant.Version = command.Version;
            variant.CountryOfOrigin = command.CountryOfOrigin;
            variant.IsSales = command.IsSales;
            variant.IsPurchase = command.IsPurchase;
            await _repository.Update(variant);
            
            // variant.IsArchived = command.IsArchived;

            // Update variant codes
            await UpdateVariantCodes(variant.Id, command);

            // Update suppliers
            await UpdateVariantSuppliers(command, variant.Id);

            // Update sales taxes
            await UpdateVariantSalesTaxes(command, variant.Id);

            // Update purchase taxes
            await UpdateVariantPurchaseTaxes(command, variant.Id);

            // Update cost centers
            await UpdateVariantCostCenters(command, variant.Id);

            // Update manufacturer companies
            await UpdateVariantManufacturerCompanies(command, variant.Id);

            // Update selling price discounts
            await UpdateVariantSellingPriceDiscounts(command, variant.Id);

            // Update packing units
            await UpdateVariantPackingUnits(command, variant.Id);

            // Update variant attachments

            await UpdateVariantAttachmentsMethod(command.VariantAttachments, variant.Id);

            // Update inventory thresholds
            await UpdateVariantInventoryThresholds(variant.Id, command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);

            // Update expiry levels
            await UpdateVariantExpiryLevels(variant.Id, command.ExpiryLevels);

            // Commit transaction - everything succeeded
            await _unitOfWork.CommitAsync();

            return new ApiResponse<Variant> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = variant };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Variant> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }

    public override async Task<ApiResponse<Variant>> Delete(Guid id, bool isValidate = true)
    {
        await _inventoryThresholdRepository.ExecuteDeleteAsync(t => t.VariantId == id);
        await _expiryLevelRepository.ExecuteDeleteAsync(t => t.VariantId == id);
        return await base.Delete(id, isValidate);
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(VariantCreateCommand command)
    {
        var validationResult = await base.ValidateCreate(command);
        
        // Check if code is unique
        var checkRepeatedCode = await _codeRepository.GetQuery().AnyAsync(e => e.Code == command.Code);
        if (checkRepeatedCode)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("VARIANT_CODE_IS_DUPLICATED");
        }

        // Check Gs1Code uniqueness
        if (!string.IsNullOrEmpty(command.Gs1Code))
        {
            var checkRepeatedGs1Code = await _codeRepository.GetQuery().AnyAsync(e => e.Code == command.Gs1Code);
            if (checkRepeatedGs1Code)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("VARIANT_GS1_CODE_IS_DUPLICATED");
            }
        }

        // Check EGSCode uniqueness
        if (!string.IsNullOrEmpty(command.EGSCode))
        {
            var checkRepeatedEgsCode = await _codeRepository.GetQuery().AnyAsync(e => e.Code == command.EGSCode);
            if (checkRepeatedEgsCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("VARIANT_EGS_CODE_IS_DUPLICATED");
            }
        }

        // Check for duplicate barcodes in the command
        if (command.BarCodes != null && command.BarCodes.Count != command.BarCodes.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_BARCODES");
        }

        // Check if any barcode already exists
        if (command.BarCodes?.Any() == true)
        {
            var checkRepeatedBarCode = await _codeRepository.GetQuery().AnyAsync(e => command.BarCodes.Contains(e.Code));
            if (checkRepeatedBarCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("VARIANT_ONE_OF_BARCODES_IS_DUPLICATED");
            }
        }

        // Check for duplicate suppliers
        if (command.SuppliersIds != null && command.SuppliersIds.Count != command.SuppliersIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS");
        }

        // Check for duplicate manufacturer companies
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Count != command.ManufacturerCompaniesIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES");
        }

        // Verify all suppliers exist
        if (command.SuppliersIds?.Any() == true)
        {
            var existedSuppliersCount = await _supplierRepository.GetQuery()
                .CountAsync(e => command.SuppliersIds.Contains(e.Id));
            if (command.SuppliersIds.Count > existedSuppliersCount)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ONE_OF_SUPPLIERS_NOT_FOUND");
            }
        }

        // Note: Manufacturer company existence will be validated by database foreign key constraints

        // Verify product exists
        var product = await _productRepository.Get(command.ProductId);
        if (product == null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PRODUCT_NOT_FOUND");
        }

        var (thresholdsValid, thresholdErrors) = InventoryThresholdValidationHelper.ValidateThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
        if (!thresholdsValid)
            validationResult.errors.AddRange(thresholdErrors);
        validationResult.isValid = validationResult.isValid && thresholdsValid;
        var (expiryLevelsValid, expiryLevelErrors) = ExpiryLevelValidationHelper.ValidateExpiryLevels(command.ExpiryLevels);
        if (!expiryLevelsValid)
            validationResult.errors.AddRange(expiryLevelErrors);
        validationResult.isValid = validationResult.isValid && expiryLevelsValid;

        return validationResult;
    }

    protected override async Task<(bool isValid, List<string> errors, Variant? entity)> ValidateUpdate(VariantUpdateCommand command)
    {
        var validationResult = await base.ValidateUpdate(command);
        
        // Check for duplicate barcodes in the command
        if (command.BarCodes != null && command.BarCodes.Count != command.BarCodes.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_BARCODES");
        }

        // Check if any barcode already exists for other variants
        if (command.BarCodes?.Any() == true)
        {
            var checkRepeatedBarCode = await _codeRepository.GetQuery()
                .AnyAsync(e => e.VariantId != command.Id && command.BarCodes.Contains(e.Code));
            if (checkRepeatedBarCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("VARIANT_ONE_OF_BARCODES_IS_DUPLICATED");
            }
        }

        // Check for duplicate suppliers
        if (command.SuppliersIds != null && command.SuppliersIds.Count != command.SuppliersIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS");
        }

        // Check for duplicate manufacturer companies
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Count != command.ManufacturerCompaniesIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES");
        }

        // Verify all suppliers exist
        if (command.SuppliersIds?.Any() == true)
        {
            var existedSuppliersCount = await _supplierRepository.GetQuery()
                .CountAsync(e => command.SuppliersIds.Contains(e.Id));
            if (command.SuppliersIds.Count > existedSuppliersCount)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ONE_OF_SUPPLIERS_NOT_FOUND");
            }
        }

        // Note: Manufacturer company existence will be validated by database foreign key constraints

        // Verify product exists
        var product = await _productRepository.Get(command.ProductId);
        if (product == null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PRODUCT_NOT_FOUND");
        }

        var (thresholdsValid, thresholdErrors) = InventoryThresholdValidationHelper.ValidateThresholds(command.InventoryThresholds, command.InventoryThresholdScope, command.InventoryThresholdBranchId);
        if (!thresholdsValid)
            validationResult.errors.AddRange(thresholdErrors);
        validationResult.isValid = validationResult.isValid && thresholdsValid;
        var (expiryLevelsValid, expiryLevelErrors) = ExpiryLevelValidationHelper.ValidateExpiryLevels(command.ExpiryLevels);
        if (!expiryLevelsValid)
            validationResult.errors.AddRange(expiryLevelErrors);
        validationResult.isValid = validationResult.isValid && expiryLevelsValid;

        return validationResult;
    }

    private async Task CreateVariantInventoryThresholds(Guid variantId, List<InventoryThresholdDto> thresholds, InventoryThresholdScope? scope, Guid? branchId)
    {
        if (thresholds == null || !thresholds.Any()) return;
        var scopeValue = scope ?? InventoryThresholdScope.All;
        var now = DateTime.Now;
        var entities = thresholds.Select((dto, index) => new InventoryThreshold
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
        await _inventoryThresholdRepository.Add(entities);
    }

    private async Task CreateVariantExpiryLevels(Guid variantId, List<ExpiryLevelDto> expiryLevels)
    {
        if (expiryLevels == null || !expiryLevels.Any()) return;
        var now = DateTime.Now;
        var entities = expiryLevels.Select((dto, index) => new ExpiryLevel
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
        await _expiryLevelRepository.Add(entities);
    }

    private async Task UpdateVariantInventoryThresholds(Guid variantId, List<InventoryThresholdDto>? inventoryThresholds, InventoryThresholdScope? scope, Guid? branchId)
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

    private async Task UpdateVariantExpiryLevels(Guid variantId, List<ExpiryLevelDto>? expiryLevels)
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

    public async Task<ApiResponse<List<VariantDto>>> GetVariantDtos()
    {
        try
        {
            var result = await _repository.GetDtos();
            return new ApiResponse<List<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.ToList()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<VariantDto?>> GetVariantDtoById(Guid id)
    {
        try
        {
            var result = await _repository.GetDtoById(id);
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId)
    {
        try
        {
            var result = await _repository.GetVariantsByProductId(productId);
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsWithAttributes()
    {
        try
        {
            var result = await _repository.GetVariantsWithAttributes();
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<VariantDto?>> GetVariantWithAttributes(Guid id)
    {
        try
        {
            var result = await _repository.GetVariantWithAttributes(id);
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm)
    {
        try
        {
            var result = await _repository.SearchVariants(searchTerm);
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<VariantDto>> CreateVariantWithAttributes(VariantCreateCommand command)
    {
        try
        {
            // The Create method now handles codes, attributes, and transactions
            var variantResult = await Create(command, true);
            if (!variantResult.IsSuccess || variantResult.Result == null)
            {
                return new ApiResponse<VariantDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = variantResult.ErrorMessages
                };
            }

            var variant = variantResult.Result;

            // Get the variant with attributes for the response
            var variantDto = await _repository.GetVariantWithAttributes(variant.Id);
            return new ApiResponse<VariantDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = variantDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VariantDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    /// <summary>
    /// Bulk-create variants for product update scenarios.
    /// This keeps the existing Create method untouched while allowing
    /// UpdateProductVariants to insert many variants in a batched way.
    /// </summary>
    public async Task<ApiResponse<bool>> CreateVariantsBulkForProductUpdate(
        List<VariantCreateCommand> commands,
        bool isValidate = true)
    {
        if (commands == null || !commands.Any())
        {
            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }

        if (isValidate)
        {
            var allErrors = new List<string>();

            foreach (var command in commands)
            {
                var validationResult = await ValidateCreate(command);
                if (!validationResult.isValid)
                {
                    allErrors.AddRange(validationResult.errors);
                }
            }

            if (allErrors.Any())
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = allErrors.Distinct().ToList()
                };
            }
        }

        try
        {
            var variants = BuildVariantsForBulkCreate(commands);

            // Single batched insert for all variants with their navigation properties
            await _repository.Add(variants);

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<VariantDto>> UpdateVariantWithAttributes(VariantUpdateCommand command)
    {
        try
        {
            // The Update method now handles codes, attributes, and transactions
            var variantResult = await Update(command, true);
            if (!variantResult.IsSuccess || variantResult.Result == null)
            {
                return new ApiResponse<VariantDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = variantResult.ErrorMessages
                };
            }

            var variant = variantResult.Result;

            // Get the updated variant with attributes for the response
            var variantDto = await _repository.GetVariantWithAttributes(variant.Id);
            return new ApiResponse<VariantDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = variantDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VariantDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
   
    private async Task CreateVariantCodesForCreate(Guid variantId, VariantCreateCommand command)
    {
        var codes = new List<VariantCode>();

        if (!string.IsNullOrEmpty(command.Code))
        {
            codes.Add(new VariantCode
            {
                Code = command.Code,
                VariantId = variantId,
                CodeType = VariantCodeType.Code,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (!string.IsNullOrEmpty(command.Gs1Code))
        {
            codes.Add(new VariantCode
            {
                Code = command.Gs1Code,
                VariantId = variantId,
                CodeType = VariantCodeType.GS1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (!string.IsNullOrEmpty(command.EGSCode))
        {
            codes.Add(new VariantCode
            {
                Code = command.EGSCode,
                VariantId = variantId,
                CodeType = VariantCodeType.EGS,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (command.BarCodes != null && command.BarCodes.Any())
        {
            foreach (var barcode in command.BarCodes)
            {
                codes.Add(new VariantCode
                {
                    Code = barcode,
                    VariantId = variantId,
                    CodeType = VariantCodeType.BarCode,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
        }

        if (codes.Any())
        {
            await _codeRepository.Add(codes);
        }
    }

    private async Task CreateVariantCodesForUpdate(Guid variantId, string? gs1Code, string? egsCode, List<string> barCodes)
    {
        var codes = new List<VariantCode>();

        if (!string.IsNullOrEmpty(gs1Code))
        {
            codes.Add(new VariantCode
            {
                Code = gs1Code,
                VariantId = variantId,
                CodeType = VariantCodeType.GS1
            });
        }

        if (!string.IsNullOrEmpty(egsCode))
        {
            codes.Add(new VariantCode
            {
                Code = egsCode,
                VariantId = variantId,
                CodeType = VariantCodeType.EGS
            });
        }

        if (barCodes != null && barCodes.Any())
        {
            foreach (var barcode in barCodes)
            {
                codes.Add(new VariantCode
                {
                    Code = barcode,
                    VariantId = variantId,
                    CodeType = VariantCodeType.BarCode
                });
            }
        }

        foreach (var code in codes)
        {
            await _codeRepository.Add(code);
        }
    }

    private async Task UpdateVariantCodes(Guid variantId, VariantUpdateCommand command)
    {
        // Remove existing codes using native EF Core ExecuteDeleteAsync
        await _codeRepository.ExecuteDeleteAsync(c => c.VariantId == variantId && c.CodeType != VariantCodeType.Code);

        // Add new codes
        await CreateVariantCodesForUpdate(variantId, command.Gs1Code, command.EGSCode, command.BarCodes);
    }

    private async Task CreateVariantSuppliers(VariantCreateCommand command, Guid variantId)
    {
        var variantSuppliers = new List<VariantSupplier>();
        foreach (var supplierId in command.SuppliersIds)
        {
            variantSuppliers.Add(new VariantSupplier
            {
                VariantId = variantId,
                SupplierId = supplierId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }
        
        if (variantSuppliers.Any())
        {
            await _variantSupplierRepository.Add(variantSuppliers);
        }
    }

    private async Task UpdateVariantSuppliers(VariantUpdateCommand command, Guid variantId)
    {
        // Get existing supplier IDs to determine which ones to add
        var existingSupplierIds = await _variantSupplierRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .Select(e => e.SupplierId)
            .ToListAsync();
        
        // Remove suppliers not in the new list using native ExecuteDeleteAsync
        if (existingSupplierIds.Any())
        {
            await _variantSupplierRepository.ExecuteDeleteAsync(
                vs => vs.VariantId == variantId && !command.SuppliersIds.Contains(vs.SupplierId));
        }
        
        // Add new suppliers
        var suppliersToAdd = command.SuppliersIds
            .Where(supplierId => !existingSupplierIds.Contains(supplierId))
            .Select(supplierId => new VariantSupplier
            {
                VariantId = variantId,
                SupplierId = supplierId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        
        if (suppliersToAdd.Any())
        {
            await _variantSupplierRepository.Add(suppliersToAdd);
        }
    }

    private async Task CreateVariantSalesTaxes(VariantCreateCommand command, Guid variantId)
    {
        var variantSalesTaxes = command.SalesTaxIds.Select(taxId => new VariantSalesTax
        {
            VariantId = variantId,
            TaxId = taxId,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        }).ToList();

        if (variantSalesTaxes.Any())
            await _variantSalesTaxRepository.Add(variantSalesTaxes);
    }

    private async Task CreateVariantPurchaseTaxes(VariantCreateCommand command, Guid variantId)
    {
        var variantPurchaseTaxes = command.PurchaseTaxIds.Select(taxId => new VariantPurchaseTax
        {
            VariantId = variantId,
            TaxId = taxId,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        }).ToList();

        if (variantPurchaseTaxes.Any())
            await _variantPurchaseTaxRepository.Add(variantPurchaseTaxes);
    }

    private async Task CreateVariantCostCenters(VariantCreateCommand command, Guid variantId)
    {
        var variantCostCenters = command.CostCenters.Select(e => new VariantCostCenter
        {
            VariantId = variantId,
            CostCenterId = e.CostCenterId,
            Percent = e.Percent,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        }).ToList();

        if (variantCostCenters.Any())
        {
            await _variantCostCenterRepository.Add(variantCostCenters);
        }
    }

    private async Task UpdateVariantCostCenters(VariantUpdateCommand command, Guid variantId)
    {
        var existingCostCenters = await _variantCostCenterRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();

        var costCenters = command.CostCenters ?? [];

        var costCentersToRemove = existingCostCenters
            .Where(e => !costCenters.Any(d => d.CostCenterId == e.CostCenterId))
            .ToList();

        var costCentersToUpdate = new List<VariantCostCenter>();
        var costCentersToAdd = new List<VariantCostCenter>();

        foreach (var costCenterDto in costCenters)
        {
            var existingCostCenter = existingCostCenters.FirstOrDefault(e => e.CostCenterId == costCenterDto.CostCenterId);
            if (existingCostCenter != null)
            {
                existingCostCenter.Percent = costCenterDto.Percent;
                existingCostCenter.ModifiedAt = DateTime.Now;
                costCentersToUpdate.Add(existingCostCenter);
            }
            else
            {
                costCentersToAdd.Add(new VariantCostCenter
                {
                    VariantId = variantId,
                    CostCenterId = costCenterDto.CostCenterId,
                    Percent = costCenterDto.Percent,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
        }

        if (costCentersToRemove.Any())
        {
            await _variantCostCenterRepository.Delete(costCentersToRemove);
        }

        if (costCentersToUpdate.Any())
        {
            await _variantCostCenterRepository.Update(costCentersToUpdate);
        }

        if (costCentersToAdd.Any())
        {
            await _variantCostCenterRepository.Add(costCentersToAdd);
        }
    }

    private async Task UpdateVariantSalesTaxes(VariantUpdateCommand command, Guid variantId)
    {
        var existingTaxIds = await _variantSalesTaxRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .Select(e => e.TaxId)
            .ToListAsync();

        if (existingTaxIds.Any())
        {
            await _variantSalesTaxRepository.ExecuteDeleteAsync(
                vst => vst.VariantId == variantId && !command.SalesTaxIds.Contains(vst.TaxId));
        }

        var taxesToAdd = command.SalesTaxIds
            .Where(taxId => !existingTaxIds.Contains(taxId))
            .Select(taxId => new VariantSalesTax
            {
                VariantId = variantId,
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();

        if (taxesToAdd.Any())
            await _variantSalesTaxRepository.Add(taxesToAdd);
    }

    private async Task UpdateVariantPurchaseTaxes(VariantUpdateCommand command, Guid variantId)
    {
        var existingTaxIds = await _variantPurchaseTaxRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .Select(e => e.TaxId)
            .ToListAsync();

        if (existingTaxIds.Any())
        {
            await _variantPurchaseTaxRepository.ExecuteDeleteAsync(
                vpt => vpt.VariantId == variantId && !command.PurchaseTaxIds.Contains(vpt.TaxId));
        }

        var taxesToAdd = command.PurchaseTaxIds
            .Where(taxId => !existingTaxIds.Contains(taxId))
            .Select(taxId => new VariantPurchaseTax
            {
                VariantId = variantId,
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();

        if (taxesToAdd.Any())
            await _variantPurchaseTaxRepository.Add(taxesToAdd);
    }

    private async Task CreateVariantManufacturerCompanies(VariantCreateCommand command, Guid variantId)
    {
        var variantManufacturerCompanies = new List<VariantManufacturerCompany>();
        foreach (var companyId in command.ManufacturerCompaniesIds)
        {
            variantManufacturerCompanies.Add(new VariantManufacturerCompany
            {
                VariantId = variantId,
                ManufacturerCompanyId = companyId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }
        
        if (variantManufacturerCompanies.Any())
        {
            await _variantManufacturerCompanyRepository.Add(variantManufacturerCompanies);
        }
    }

    private async Task UpdateVariantManufacturerCompanies(VariantUpdateCommand command, Guid variantId)
    {
        // Get existing company IDs to determine which ones to add
        var existingCompanyIds = await _variantManufacturerCompanyRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .Select(e => e.ManufacturerCompanyId)
            .ToListAsync();
        
        // Remove companies not in the new list using native ExecuteDeleteAsync
        if (existingCompanyIds.Any())
        {
            await _variantManufacturerCompanyRepository.ExecuteDeleteAsync(
                vmc => vmc.VariantId == variantId && !command.ManufacturerCompaniesIds.Contains(vmc.ManufacturerCompanyId));
        }
        
        // Add new companies
        var companiesToAdd = command.ManufacturerCompaniesIds
            .Where(companyId => !existingCompanyIds.Contains(companyId))
            .Select(companyId => new VariantManufacturerCompany
            {
                VariantId = variantId,
                ManufacturerCompanyId = companyId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        
        if (companiesToAdd.Any())
        {
            await _variantManufacturerCompanyRepository.Add(companiesToAdd);
        }
    }

    private async Task CreateVariantSellingPriceDiscounts(VariantCreateCommand command, Guid variantId)
    {
        var discounts = command.SellingPriceDiscounts.Select(e => new VariantSellingPriceDiscount
        {
            VariantId = variantId,
            SellingPriceId = e.SellingPriceId,
            Discount = e.Discount,
            DiscountType = e.DiscountType,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        }).ToList();
        
        if (discounts.Any())
        {
            await _variantSellingPriceDiscountRepository.Add(discounts);
        }
    }

    private async Task UpdateVariantSellingPriceDiscounts(VariantUpdateCommand command, Guid variantId)
    {
        var existingDiscounts = await _variantSellingPriceDiscountRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .ToListAsync();
        
        var discountsToRemove = existingDiscounts
            .Where(e => !command.SellingPriceDiscounts.Any(d => d.SellingPriceId == e.SellingPriceId))
            .ToList();
        
        var discountsToUpdate = new List<VariantSellingPriceDiscount>();
        var discountsToAdd = new List<VariantSellingPriceDiscount>();
        
        foreach (var discountDto in command.SellingPriceDiscounts)
        {
            var existingDiscount = existingDiscounts.FirstOrDefault(e => e.SellingPriceId == discountDto.SellingPriceId);
            if (existingDiscount != null)
            {
                existingDiscount.Discount = discountDto.Discount;
                existingDiscount.DiscountType = discountDto.DiscountType;
                existingDiscount.ModifiedAt = DateTime.Now;
                discountsToUpdate.Add(existingDiscount);
            }
            else
            {
                discountsToAdd.Add(new VariantSellingPriceDiscount
                {
                    VariantId = variantId,
                    SellingPriceId = discountDto.SellingPriceId,
                    Discount = discountDto.Discount,
                    DiscountType = discountDto.DiscountType,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
        }
        
        if (discountsToRemove.Any())
        {
            await _variantSellingPriceDiscountRepository.Delete(discountsToRemove);
        }
        
        if (discountsToUpdate.Any())
        {
            await _variantSellingPriceDiscountRepository.Update(discountsToUpdate);
        }
        
        if (discountsToAdd.Any())
        {
            await _variantSellingPriceDiscountRepository.Add(discountsToAdd);
        }
    }

    private async Task CreateVariantPackingUnits(VariantCreateCommand command, Guid variantId)
    {
        var packingUnits = new List<VariantPackingUnit>();
        
        foreach (var packingUnitDto in command.PackingUnits)
        {
            packingUnits.Add(new VariantPackingUnit
            {
                VariantId = variantId,
                PackingUnitId = packingUnitDto.PackingUnitId,
                AverageCostPrice = packingUnitDto.AverageCostPrice,
                IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                LastCostPrice = packingUnitDto.LastCostPrice,
                PartsCount = packingUnitDto.PartsCount,
                IsDefaultSales = packingUnitDto.IsDefaultSales,
                IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                OrderNumber = packingUnitDto.OrderNumber,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                VariantPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(sp => new VariantPackingUnitSellingPrice
                {
                    Amount = sp.Amount,
                    SellingPriceId = sp.SellingPriceId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                }).ToList()
            });
        }
        
        if (packingUnits.Any())
        {
            await _variantPackingUnitRepository.Add(packingUnits);
        }
    }

    private async Task UpdateVariantPackingUnits(VariantUpdateCommand command, Guid variantId)
    {
        var variantPackingUnitRepository = _variantPackingUnitRepository;
        
        // Get existing packing units
        var existingPackingUnits = await variantPackingUnitRepository.GetQuery()
            .Where(vpu => vpu.VariantId == variantId)
            .ToListAsync();

        // If packing units is null or empty, remove all packing units
        if (command.PackingUnits == null || !command.PackingUnits.Any())
        {
            if (existingPackingUnits.Any())
            {
                await variantPackingUnitRepository.Delete(existingPackingUnits);
            }
            return;
        }

        // Find packing units to remove (ones not in the command)
        var packingUnitsToRemove = existingPackingUnits
            .Where(e => !command.PackingUnits.Any(p => p.PackingUnitId == e.PackingUnitId))
            .ToList();

        // Remove deleted packing units
        if (packingUnitsToRemove.Any())
        {
            await variantPackingUnitRepository.Delete(packingUnitsToRemove);
        }

        // Process each packing unit from command
        var packingUnitsToAdd = new List<VariantPackingUnit>();

        foreach (var packingUnitDto in command.PackingUnits)
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

    private async Task UpdateVariantPackingUnitSellingPrices(VariantPackingUnitDto packingUnitDto, VariantPackingUnit existingPackingUnit)
    {
        var existingPrices = await _variantPackingUnitSellingPriceRepository.GetQuery()
            .Where(e => e.VariantPackingUnitId == existingPackingUnit.Id)
            .ToListAsync();

        var pricesToAdd = new List<VariantPackingUnitSellingPrice>();
        var pricesToUpdate = new List<VariantPackingUnitSellingPrice>();
        var pricesToRemove = existingPrices
            .Where(e => !packingUnitDto.SellingPrices.Any(p => p.SellingPriceId == e.SellingPriceId))
            .ToList();

        // Remove prices that are no longer in the packing unit
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

    private async Task CreateVariantAttachments(List<VariantAttachmentDto> variantAttachmentDtos, Guid variantId)
    {
        // Use optimized version
        await CreateVariantAttachmentsOptimized(variantAttachmentDtos, variantId);
    }

    private async Task CreateVariantAttachmentsOptimized(List<VariantAttachmentDto> variantAttachmentDtos, Guid variantId)
    {
        if (!variantAttachmentDtos?.Any() == true) return;

        var attachmentsToAdd = new List<Attachment>();
        var variantAttachmentsToAdd = new List<VariantAttachment>();
        var now = DateTime.Now;

        // Batch process base64 conversions and prepare entities
        foreach (var variantAttachmentDto in variantAttachmentDtos.Where(dto => dto.Attachment != null))
        {
            var attachmentDto = variantAttachmentDto.Attachment!;
            
            var variantAttachment = new VariantAttachment
            {
                VariantId = variantId,
                Attachment = new Attachment
                            {
                                FileName = attachmentDto.FileName,
                                FileContentType = attachmentDto.ContentType,
                                FileData = Convert.FromBase64String(attachmentDto.FileContent),
                                CreatedAt = now,
                                ModifiedAt = now
                            },
                AttachmentType = variantAttachmentDto.AttachmentType,
                Description = variantAttachmentDto.Description,
                IsPrimary = variantAttachmentDto.IsPrimary,
                DisplayOrder = variantAttachmentDto.DisplayOrder,
                CreatedAt = now,
                ModifiedAt = now
            };

            variantAttachmentsToAdd.Add(variantAttachment);
        }

        if (variantAttachmentsToAdd.Any())
        {
            await _variantAttachmentRepository.BulkInsertAsync(variantAttachmentsToAdd);
        }
    }

    private async Task UpdateVariantAttachmentsMethod(List<VariantAttachmentDto> variantAttachmentDtos, Guid variantId)
    {
        // Get attachment IDs first
        var attachmentIds = await _variantAttachmentRepository.GetQuery()
            .Where(e => e.VariantId == variantId)
            .Select(e => e.AttachmentId)
            .ToListAsync();
        
        // Delete VariantAttachment relationships first
        await _variantAttachmentRepository.ExecuteDeleteAsync(va => va.VariantId == variantId);
        
        // Delete attachments using traditional load-and-delete (most compatible approach)
        if (attachmentIds.Any())
        {
            foreach (var attachmentId in attachmentIds)
            {
                // Load the entity first, then delete it (avoids any EF Core ExecuteDeleteAsync issues)
                var attachment = await _attachmentRepository.Get(attachmentId);
                if (attachment != null)
                {
                    await _attachmentRepository.Delete(attachment);
                }
            }
        }
        
        // Add new attachments
        await CreateVariantAttachmentsOptimized(variantAttachmentDtos, variantId);
    }


    private async Task CreateVariantAttributes(Guid variantId, List<VariantAttributeValueDto> variantAttributes)
    {
        var variantAttributeValueRepository = _variantAttributeValueRepository;
        
        foreach (var attr in variantAttributes)
        {
            var variantAttributeValue = new VariantAttributeValue
            {
                VariantId = variantId,
                AttributeDefinitionId = attr.AttributeDefinitionId,
                AttributeValueId = attr.AttributeValueId
            };
            await variantAttributeValueRepository.Add(variantAttributeValue);
        }
    }

    private async Task UpdateVariantAttributes(Guid variantId, List<VariantAttributeValueDto> variantAttributes)
    {
        var variantAttributeValueRepository = _variantAttributeValueRepository;
        
        // Remove existing attributes using native EF Core ExecuteDeleteAsync
        await variantAttributeValueRepository.ExecuteDeleteAsync(va => va.VariantId == variantId);

        //Add new attributes
        await CreateVariantAttributes(variantId, variantAttributes);
    }

    private string GetNextVariantCode(string lastVariantCode)
    {
        if (string.IsNullOrEmpty(lastVariantCode))
        {
            return "V001";
        }

        var match = Regex.Match(lastVariantCode, @"(\d+)$");
        if (match.Success)
        {
            var numberPart = match.Groups[1].Value;
            var prefix = lastVariantCode.Substring(0, lastVariantCode.Length - numberPart.Length);
            var nextNumber = int.Parse(numberPart) + 1;
            return prefix + nextNumber.ToString("D" + numberPart.Length);
        }

        return lastVariantCode + "001";
    }


    public async Task<List<VariantDto>> GetVariantsByAttribute(Guid attributeDefinitionId, Guid attributeValueId)
    {
        var variantAttributeValues = await _variantAttributeValueRepository.GetVariantsByAttributeValueAsync(attributeDefinitionId, attributeValueId);

        var variantIds = variantAttributeValues.Select(vav => vav.VariantId);
        var variants = await _repository.GetQuery().Where(e => variantIds.Any(v => v == e.Id)).ToListAsync();

        return variants.Select(MapToVariantDto).ToList();
    }

    public async Task<List<VariantDto>> GetVariantsByAttributeValues(Dictionary<Guid, Guid> attributeFilters)
    {
        if (!attributeFilters.Any())
            return new List<VariantDto>();

        var variantIds = new List<Guid>();

        foreach (var filter in attributeFilters)
        {
            var variantAttributeValues = await _variantAttributeValueRepository.GetVariantsByAttributeValueAsync(filter.Key, filter.Value);
            var currentVariantIds = variantAttributeValues.Select(vav => vav.VariantId).ToList();

            if (!variantIds.Any())
            {
                variantIds = currentVariantIds;
            }
            else
            {
                variantIds = variantIds.Intersect(currentVariantIds).ToList();
            }
        }

        var variants = await _repository.GetQuery().Where(e => variantIds.Any(v => v == e.Id)).ToListAsync();
        return variants.Select(MapToVariantDto).ToList();
    }

    public async Task<List<VariantDto>> GetVariantsByProductAndAttributes(Guid productId, Dictionary<Guid, Guid> attributeFilters)
    {
        var productVariants = await _repository.GetByProductIdAsync(productId);

        if (!attributeFilters.Any())
            return productVariants.Select(MapToVariantDto).ToList();

        var filteredVariants = new List<VariantDto>();

        foreach (var variant in productVariants)
        {
            var variantAttributeValues = await _variantAttributeValueRepository.GetByVariantIdAsync(variant.Id);
            var variantAttributes = variantAttributeValues.ToDictionary(vav => vav.AttributeDefinitionId, vav => vav.AttributeValueId);

            bool matchesAllFilters = attributeFilters.All(filter =>
                variantAttributes.ContainsKey(filter.Key) && variantAttributes[filter.Key] == filter.Value);

            if (matchesAllFilters)
            {
                filteredVariants.Add(MapToVariantDto(variant));
            }
        }

        return filteredVariants;
    }

    public async Task<List<AttributeDefinitionDto>> GetAvailableAttributesForProduct(Guid productId)
    {
        var productAttributeDefinitions = await _productAttributeDefinitionRepository.GetByProductIdAsync(productId);

        return productAttributeDefinitions.Select(pad => new AttributeDefinitionDto
        {
            Id = pad.AttributeDefinition!.Id,
            // Code = pad.AttributeDefinition.Code,
            Name = pad.AttributeDefinition.Name,
            NameSecondLanguage = pad.AttributeDefinition.NameSecondLanguage,
            IsActive = pad.AttributeDefinition.IsActive,
            SortOrder = pad.AttributeDefinition.SortOrder,
            PredefinedValues = pad.AttributeDefinition.PredefinedValues?.Select(av => new AttributeValueDto
            {
                Id = av.Id,
                AttributeDefinitionId = av.AttributeDefinitionId,
                Name = av.Name,
                NameSecondLanguage = av.NameSecondLanguage,
                SortOrder = av.SortOrder,
                IsActive = av.IsActive
            }).ToList() ?? new List<AttributeValueDto>()
        }).ToList();
    }

    public async Task<List<VariantDto>> GenerateVariantsForProduct(Guid productId)
    {
        var product = await _productRepository.Get(productId);
        if (product == null)
            throw new ArgumentException($"Product with ID {productId} not found");

        var variantAttributes = await _productAttributeDefinitionRepository.GetVariantAttributesByProductIdAsync(productId);
        if (!variantAttributes.Any())
            return new List<VariantDto>();

        var variantAttributeDefinitionIds = variantAttributes.Select(va => va.AttributeDefinitionId).ToList();
        return await GenerateVariantsFromAttributes(productId, variantAttributeDefinitionIds);
    }

    public async Task<List<VariantDto>> GenerateVariantsFromAttributes(Guid productId, List<Guid> variantAttributeDefinitionIds)
    {
        var combinations = await GetAllPossibleVariantCombinations(productId);
        var variants = new List<VariantDto>();

        foreach (var combination in combinations)
        {
            // Check if variant already exists
            var existingVariant = await FindExistingVariantAsync(productId, combination);
            if (existingVariant == null)
            {
                var variant = await CreateVariantWithAttributeValues(productId, combination);
                variants.Add(variant);
            }
        }

        return variants;
    }

    public async Task<VariantDto> CreateVariantWithAttributeValues(Guid productId, Dictionary<Guid, Guid> attributeValues)
    {
        var product = await _productRepository.Get(productId);
        if (product == null)
            throw new ArgumentException($"Product with ID {productId} not found");

        // Create variant name based on attribute values
        var variantName = await GenerateVariantName(productId, attributeValues);

        var createCommand = new Variant
        {
            ProductId = productId,
            // Name = variantName,
            // NameSecondLanguage = variantName, // You might want to generate English name too
            Description = $"Variant of {product.Name}",
            MaxDiscount = product.MaxDiscount,
            ConditionalDiscount = product.ConditionalDiscount,
            DefaultDiscount = product.DefaultDiscount,
            DefaultDiscountType = product.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = product.IsDiscountBasedOnSellingPrice,
            Model = product.Model,
            Version = product.Version,
            CountryOfOrigin = product.CountryOfOrigin,
            IsArchived = true,
        };

        var variant = await _repository.Add(createCommand);

        // Create variant attribute values
        foreach (var kvp in attributeValues)
        {
            var variantAttributeValue = new VariantAttributeValue
            {
                VariantId = variant.Id,
                AttributeDefinitionId = kvp.Key,
                AttributeValueId = kvp.Value,
            };
            await _variantAttributeValueRepository.Add(variantAttributeValue);
        }

        return MapToVariantDto(variant);
    }

    public async Task<bool> ValidateVariantCombination(Guid productId, Dictionary<Guid, Guid> attributeValues)
    {
        var productAttributes = await _productAttributeDefinitionRepository.GetByProductIdAsync(productId);
        var productAttributeDefinitionIds = productAttributes.Select(pa => pa.AttributeDefinitionId).ToHashSet();

        // Check if all provided attributes belong to the product
        foreach (var attributeDefinitionId in attributeValues.Keys)
        {
            if (!productAttributeDefinitionIds.Contains(attributeDefinitionId))
                return false;
        }

        // Check if all attribute values are valid
        foreach (var kvp in attributeValues)
        {
            var attributeValue = await _attributeValueRepository.Get(kvp.Value);
            if (attributeValue == null || attributeValue.AttributeDefinitionId != kvp.Key)
                return false;
        }

        return true;
    }

    public async Task<List<Dictionary<Guid, Guid>>> GetAllPossibleVariantCombinations(Guid productId)
    {
        var variantAttributes = await _productAttributeDefinitionRepository.GetVariantAttributesByProductIdAsync(productId);
        if (!variantAttributes.Any())
            return new List<Dictionary<Guid, Guid>>();

        var attributeValueLists = new List<List<(Guid AttributeDefinitionId, Guid AttributeValueId)>>();

        foreach (var variantAttribute in variantAttributes)
        {
            var attributeValues = await _attributeValueRepository.GetByAttributeDefinitionIdAsync(variantAttribute.AttributeDefinitionId);
            var valuePairs = attributeValues.Select(av => (variantAttribute.AttributeDefinitionId, av.Id)).ToList();
            attributeValueLists.Add(valuePairs);
        }

        return GenerateCombinations(attributeValueLists);
    }

    private async Task<Variant?> FindExistingVariantAsync(Guid productId, Dictionary<Guid, Guid> attributeValues)
    {
        var productVariants = await _repository.GetByProductIdAsync(productId);

        foreach (var variant in productVariants)
        {
            var variantAttributeValues = await _variantAttributeValueRepository.GetByVariantIdAsync(variant.Id);
            var variantAttributeValueMap = variantAttributeValues.ToDictionary(vav => vav.AttributeDefinitionId, vav => vav.AttributeValueId);

            if (AreAttributeValueMapsEqual(attributeValues, variantAttributeValueMap))
                return variant;
        }

        return null;
    }

    private bool AreAttributeValueMapsEqual(Dictionary<Guid, Guid> map1, Dictionary<Guid, Guid> map2)
    {
        if (map1.Count != map2.Count)
            return false;

        foreach (var kvp in map1)
        {
            if (!map2.ContainsKey(kvp.Key) || map2[kvp.Key] != kvp.Value)
                return false;
        }

        return true;
    }

    private async Task<string> GenerateVariantName(Guid productId, Dictionary<Guid, Guid> attributeValues)
    {
        var product = await _productRepository.Get(productId);
        var nameParts = new List<string> { product?.Name ?? "Variant" };

        foreach (var kvp in attributeValues)
        {
            var attributeValue = await _attributeValueRepository.Get(kvp.Value);
            if (attributeValue != null)
            {
                nameParts.Add(attributeValue.Name);
            }
        }

        return string.Join(" - ", nameParts);
    }

    private List<Dictionary<Guid, Guid>> GenerateCombinations(List<List<(Guid AttributeDefinitionId, Guid AttributeValueId)>> attributeValueLists)
    {
        if (!attributeValueLists.Any())
            return new List<Dictionary<Guid, Guid>>();

        var combinations = new List<Dictionary<Guid, Guid>>();
        GenerateCombinationsRecursive(attributeValueLists, 0, new Dictionary<Guid, Guid>(), combinations);
        return combinations;
    }

    private void GenerateCombinationsRecursive(
        List<List<(Guid AttributeDefinitionId, Guid AttributeValueId)>> attributeValueLists,
        int currentIndex,
        Dictionary<Guid, Guid> currentCombination,
        List<Dictionary<Guid, Guid>> combinations)
    {
        if (currentIndex >= attributeValueLists.Count)
        {
            combinations.Add(new Dictionary<Guid, Guid>(currentCombination));
            return;
        }

        var currentAttributeValues = attributeValueLists[currentIndex];
        foreach (var (attributeDefinitionId, attributeValueId) in currentAttributeValues)
        {
            currentCombination[attributeDefinitionId] = attributeValueId;
            GenerateCombinationsRecursive(attributeValueLists, currentIndex + 1, currentCombination, combinations);
            currentCombination.Remove(attributeDefinitionId);
        }
    }

    private VariantDto MapToVariantDto(Variant variant)
    {
        return new VariantDto
        {
            Id = variant.Id,
            // Name = variant.Name,
            // NameSecondLanguage = variant.NameSecondLanguage,
            Description = variant.Description,
            ProductId = variant.ProductId,
            MaxDiscount = variant.MaxDiscount,
            ConditionalDiscount = variant.ConditionalDiscount,
            DefaultDiscount = variant.DefaultDiscount,
            DefaultDiscountType = variant.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = variant.IsDiscountBasedOnSellingPrice,
            Model = variant.Model,
            Version = variant.Version,
            CountryOfOrigin = variant.CountryOfOrigin,
            IsActive = variant.IsArchived,
        };
    }

    private static void HandleVariantManufacturerCompanies(VariantCreateCommand command, Variant variant)
    {
        variant.VariantManufacturerCompanies = [];
        foreach (var companyId in command.ManufacturerCompaniesIds)
            variant.VariantManufacturerCompanies.Add(
                new VariantManufacturerCompany
                {
                    ManufacturerCompanyId = companyId,
                });
    }

    private static void HandleVariantSuppliers(VariantCreateCommand command, Variant variant)
    {
        variant.VariantSuppliers = [];
        foreach (var supplierId in command.SuppliersIds)
            variant.VariantSuppliers.Add(
                new VariantSupplier
                {
                    SupplierId = supplierId,
                });
    }

    public List<Variant> PrepareVariantsForProduct(ProductCreateCommand command)
    {
        var variants = new List<Variant>();
        // Only products of type Stock can have combination variants; others only have the default variant
        bool hasVariantCombinations = command.ProductType == ProductType.Stock && command.VariantCombinations?.Any() == true;

        // Create default variant
        var defaultVariant = new Variant
        {
            Description = "Default variant",
            MaxDiscount = command.MaxDiscount,
            ConditionalDiscount = command.ConditionalDiscount,
            DefaultDiscount = command.IsDiscountBasedOnSellingPrice ? 0 : command.DefaultDiscount,
            DefaultDiscountType = command.DefaultDiscountType,
            IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice,
            Model = command.Model,
            Version = command.Version,
            CountryOfOrigin = command.CountryOfOrigin,
            IsPurchase = command.IsPurchase,
            IsSales = command.IsSales,
            IsDefault = true,
            IsArchived = hasVariantCombinations, // Default variant is archived if there are combination variants
            ApplyProductChanges = true,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        // Prepare default variant codes
        PrepareVariantCodes(defaultVariant, command.Code, command.Gs1Code, command.EGSCode, command.BarCodes);

        // Prepare default variant suppliers
        if (command.SuppliersIds?.Any() == true)
        {
            defaultVariant.VariantSuppliers = command.SuppliersIds.Select(supplierId => new VariantSupplier
            {
                SupplierId = supplierId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant sales taxes
        if (command.SalesTaxIds?.Any() == true)
        {
            defaultVariant.VariantSalesTaxes = command.SalesTaxIds.Select(taxId => new VariantSalesTax
            {
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant purchase taxes
        if (command.PurchaseTaxIds?.Any() == true)
        {
            defaultVariant.VariantPurchaseTaxes = command.PurchaseTaxIds.Select(taxId => new VariantPurchaseTax
            {
                TaxId = taxId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant manufacturer companies
        if (command.ManufacturerCompaniesIds?.Any() == true)
        {
            defaultVariant.VariantManufacturerCompanies = command.ManufacturerCompaniesIds.Select(companyId => new VariantManufacturerCompany
            {
                ManufacturerCompanyId = companyId,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant cost centers
        if (command.CostCenters?.Any() == true)
        {
            defaultVariant.VariantCostCenters = command.CostCenters.Select(e => new VariantCostCenter
            {
                CostCenterId = e.CostCenterId,
                Percent = e.Percent,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant selling price discounts
        if (command.IsDiscountBasedOnSellingPrice && command.SellingPriceDiscounts?.Any() == true)
        {
            defaultVariant.VariantSellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => new VariantSellingPriceDiscount
            {
                SellingPriceId = e.SellingPriceId,
                Discount = e.Discount,
                DiscountType = e.DiscountType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            }).ToList();
        }

        // Prepare default variant packing units
        if (command.PackingUnits?.Any() == true)
        {
            defaultVariant.VariantPackingUnitPrices = command.PackingUnits.Select(packingUnitDto =>
            {
                var packingUnit = new VariantPackingUnit
                {
                    PackingUnitId = packingUnitDto.PackingUnitId,
                    AverageCostPrice = packingUnitDto.AverageCostPrice,
                    IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                    LastCostPrice = packingUnitDto.LastCostPrice,
                    PartsCount = packingUnitDto.PartsCount,
                    IsDefaultSales = packingUnitDto.IsDefaultSales,
                    IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                    OrderNumber = packingUnitDto.OrderNumber,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    VariantPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(e => new VariantPackingUnitSellingPrice
                    {
                        Amount = e.Amount,
                        SellingPriceId = e.SellingPriceId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList()
                };
                return packingUnit;
            }).ToList();
        }

        // Prepare default variant inventory thresholds
        defaultVariant.InventoryThresholdScope = command.InventoryThresholdScope ?? InventoryThresholdScope.All;
        defaultVariant.InventoryThresholdBranchId = command.InventoryThresholdBranchId;
        if (command.InventoryThresholds?.Any() == true)
        {
            var scopeValue = defaultVariant.InventoryThresholdScope ?? InventoryThresholdScope.All;
            var now = DateTime.Now;
            defaultVariant.InventoryThresholds = command.InventoryThresholds.Select((dto, index) => new InventoryThreshold
            {
                OwnerType = OwnerType.Variant,
                Level = dto.Level > 0 ? dto.Level : index + 1,
                MinQuantity = dto.MinQuantity,
                MaxQuantity = dto.MaxQuantity,
                Scope = scopeValue,
                BranchId = defaultVariant.InventoryThresholdBranchId,
                EnableNotification = dto.EnableNotification,
                CreatedAt = now,
                ModifiedAt = now
            }).ToList();
        }
        if (command.ExpiryLevels?.Any() == true)
        {
            var now = DateTime.Now;
            defaultVariant.ExpiryLevels = command.ExpiryLevels.Select((dto, index) => new ExpiryLevel
            {
                OwnerType = OwnerType.Variant,
                Level = dto.Level > 0 ? dto.Level : index + 1,
                DaysFrom = dto.DaysFrom,
                DaysTo = dto.DaysTo,
                EnableNotification = dto.EnableNotification,
                CreatedAt = now,
                ModifiedAt = now
            }).ToList();
        }

        variants.Add(defaultVariant);

        // Create combination variants if provided
        if (hasVariantCombinations)
        {
            int variantNumber = 1;
            foreach (var combination in command.VariantCombinations!)
            {
                var variantCode = $"{command.Code}-{variantNumber}";
                
                var combinationVariant = new Variant
                {
                    Description = $"Variant of {command.Name}",
                    MaxDiscount = command.MaxDiscount,
                    ConditionalDiscount = command.ConditionalDiscount,
                    DefaultDiscount = command.DefaultDiscount,
                    DefaultDiscountType = command.DefaultDiscountType,
                    IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice,
                    IsPurchase = command.IsPurchase,
                    IsSales = command.IsSales,
                    ApplyProductChanges = combination.ApplyProductChanges,
                    Model = command.Model,
                    Version = command.Version,
                    CountryOfOrigin = command.CountryOfOrigin,
                    IsDefault = false,
                    IsArchived = false, // Combination variants are active
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                // Prepare combination variant codes (no GS1/EGS codes for combination variants)
                PrepareVariantCodes(combinationVariant, variantCode, null, null, []);

                // Prepare combination variant suppliers
                if (command.SuppliersIds?.Any() == true)
                {
                    combinationVariant.VariantSuppliers = command.SuppliersIds.Select(supplierId => new VariantSupplier
                    {
                        SupplierId = supplierId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant sales taxes
                if (command.SalesTaxIds?.Any() == true)
                {
                    combinationVariant.VariantSalesTaxes = command.SalesTaxIds.Select(taxId => new VariantSalesTax
                    {
                        TaxId = taxId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant purchase taxes
                if (command.PurchaseTaxIds?.Any() == true)
                {
                    combinationVariant.VariantPurchaseTaxes = command.PurchaseTaxIds.Select(taxId => new VariantPurchaseTax
                    {
                        TaxId = taxId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant manufacturer companies
                if (command.ManufacturerCompaniesIds?.Any() == true)
                {
                    combinationVariant.VariantManufacturerCompanies = command.ManufacturerCompaniesIds.Select(companyId => new VariantManufacturerCompany
                    {
                        ManufacturerCompanyId = companyId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant cost centers
                if (command.CostCenters?.Any() == true)
                {
                    combinationVariant.VariantCostCenters = command.CostCenters.Select(e => new VariantCostCenter
                    {
                        CostCenterId = e.CostCenterId,
                        Percent = e.Percent,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant selling price discounts
                if (command.IsDiscountBasedOnSellingPrice && command.SellingPriceDiscounts?.Any() == true)
                {
                    combinationVariant.VariantSellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => new VariantSellingPriceDiscount
                    {
                        SellingPriceId = e.SellingPriceId,
                        Discount = e.Discount,
                        DiscountType = e.DiscountType,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant packing units
                if (command.PackingUnits?.Any() == true)
                {
                    combinationVariant.VariantPackingUnitPrices = command.PackingUnits.Select(packingUnitDto =>
                    {
                        var packingUnit = new VariantPackingUnit
                        {
                            PackingUnitId = packingUnitDto.PackingUnitId,
                            AverageCostPrice = packingUnitDto.AverageCostPrice,
                            IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                            LastCostPrice = packingUnitDto.LastCostPrice,
                            PartsCount = packingUnitDto.PartsCount,
                            IsDefaultSales = packingUnitDto.IsDefaultSales,
                            IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                            OrderNumber = packingUnitDto.OrderNumber,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now,
                            VariantPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(e => new VariantPackingUnitSellingPrice
                            {
                                Amount = e.Amount,
                                SellingPriceId = e.SellingPriceId,
                                CreatedAt = DateTime.Now,
                                ModifiedAt = DateTime.Now
                            }).ToList()
                        };
                        return packingUnit;
                    }).ToList();
                }

                // Prepare combination variant attribute values
                if (combination.AttributeValues?.Any() == true)
                {
                    combinationVariant.VariantAttributeValues = combination.AttributeValues.Select(kvp => new VariantAttributeValue
                    {
                        AttributeDefinitionId = kvp.Key,
                        AttributeValueId = kvp.Value,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    }).ToList();
                }

                // Prepare combination variant inventory thresholds
                combinationVariant.InventoryThresholdScope = command.InventoryThresholdScope ?? InventoryThresholdScope.All;
                combinationVariant.InventoryThresholdBranchId = command.InventoryThresholdBranchId;
                if (command.InventoryThresholds?.Any() == true)
                {
                    var scopeValue = combinationVariant.InventoryThresholdScope ?? InventoryThresholdScope.All;
                    var now = DateTime.Now;
                    combinationVariant.InventoryThresholds = command.InventoryThresholds.Select((dto, index) => new InventoryThreshold
                    {
                        OwnerType = OwnerType.Variant,
                        Level = dto.Level > 0 ? dto.Level : index + 1,
                        MinQuantity = dto.MinQuantity,
                        MaxQuantity = dto.MaxQuantity,
                        Scope = scopeValue,
                        BranchId = combinationVariant.InventoryThresholdBranchId,
                        EnableNotification = dto.EnableNotification,
                        CreatedAt = now,
                        ModifiedAt = now
                    }).ToList();
                }
                if (command.ExpiryLevels?.Any() == true)
                {
                    var now = DateTime.Now;
                    combinationVariant.ExpiryLevels = command.ExpiryLevels.Select((dto, index) => new ExpiryLevel
                    {
                        OwnerType = OwnerType.Variant,
                        Level = dto.Level > 0 ? dto.Level : index + 1,
                        DaysFrom = dto.DaysFrom,
                        DaysTo = dto.DaysTo,
                        EnableNotification = dto.EnableNotification,
                        CreatedAt = now,
                        ModifiedAt = now
                    }).ToList();
                }

                variants.Add(combinationVariant);
                variantNumber++;
            }
        }

        return variants;
    }

    private void PrepareVariantCodes(Variant variant, string code, string? gs1Code, string? egsCode, List<string> barCodes)
    {
        var variantCodes = new List<VariantCode>();

        if (!string.IsNullOrEmpty(code))
        {
            variantCodes.Add(new VariantCode
            {
                Code = code,
                CodeType = VariantCodeType.Code,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (!string.IsNullOrEmpty(gs1Code))
        {
            variantCodes.Add(new VariantCode
            {
                Code = gs1Code,
                CodeType = VariantCodeType.GS1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (!string.IsNullOrEmpty(egsCode))
        {
            variantCodes.Add(new VariantCode
            {
                Code = egsCode,
                CodeType = VariantCodeType.EGS,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            });
        }

        if (barCodes != null && barCodes.Any())
        {
            foreach (var barcode in barCodes)
            {
                variantCodes.Add(new VariantCode
                {
                    Code = barcode,
                    CodeType = VariantCodeType.BarCode,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                });
            }
        }
        variant.VariantCodes = variantCodes;
    }

    /// <summary>
    /// Build Variant aggregates (with navigation collections) in memory for bulk creation.
    /// Children are attached via navigation properties so EF can cascade-insert them.
    /// </summary>
    private List<Variant> BuildVariantsForBulkCreate(List<VariantCreateCommand> commands)
    {
        var now = DateTime.Now;
        var variants = new List<Variant>();

        foreach (var command in commands)
        {
            var variant = new Variant
            {
                Description = command.Description,
                ProductId = command.ProductId,
                MaxDiscount = command.MaxDiscount,
                ConditionalDiscount = command.ConditionalDiscount,
                DefaultDiscount = command.DefaultDiscount,
                DefaultDiscountType = command.DefaultDiscountType,
                IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice,
                Model = command.Model,
                Version = command.Version,
                CountryOfOrigin = command.CountryOfOrigin,
                IsArchived = command.IsArchived,
                ApplyProductChanges = command.ApplyProductChanges,
                IsDefault = command.IsDefault,
                IsSales = command.IsSales,
                IsPurchase = command.IsPurchase,
                CreatedAt = now,
                ModifiedAt = now
            };

            // Variant codes
            PrepareVariantCodes(variant, command.Code, command.Gs1Code, command.EGSCode, command.BarCodes);

            // Suppliers
            if (command.SuppliersIds?.Any() == true)
            {
                variant.VariantSuppliers = command.SuppliersIds.Select(supplierId => new VariantSupplier
                {
                    SupplierId = supplierId,
                    CreatedAt = now,
                    ModifiedAt = now
                }).ToList();
            }

            // Manufacturer companies
            if (command.ManufacturerCompaniesIds?.Any() == true)
            {
                variant.VariantManufacturerCompanies = command.ManufacturerCompaniesIds.Select(companyId => new VariantManufacturerCompany
                {
                    ManufacturerCompanyId = companyId,
                    CreatedAt = now,
                    ModifiedAt = now
                }).ToList();
            }

            // Cost centers
            if (command.CostCenters?.Any() == true)
            {
                variant.VariantCostCenters = command.CostCenters.Select(e => new VariantCostCenter
                {
                    CostCenterId = e.CostCenterId,
                    Percent = e.Percent,
                    CreatedAt = now,
                    ModifiedAt = now
                }).ToList();
            }

            // Selling price discounts
            if (command.IsDiscountBasedOnSellingPrice && command.SellingPriceDiscounts?.Any() == true)
            {
                variant.VariantSellingPriceDiscounts = command.SellingPriceDiscounts.Select(e => new VariantSellingPriceDiscount
                {
                    SellingPriceId = e.SellingPriceId,
                    Discount = e.Discount,
                    DiscountType = e.DiscountType,
                    CreatedAt = now,
                    ModifiedAt = now
                }).ToList();
            }

            // Packing units
            if (command.PackingUnits?.Any() == true)
            {
                variant.VariantPackingUnitPrices = command.PackingUnits.Select(packingUnitDto =>
                {
                    var packingUnit = new VariantPackingUnit
                    {
                        PackingUnitId = packingUnitDto.PackingUnitId,
                        AverageCostPrice = packingUnitDto.AverageCostPrice,
                        IsDefaultPackingUnit = packingUnitDto.IsDefaultPackingUnit,
                        LastCostPrice = packingUnitDto.LastCostPrice,
                        PartsCount = packingUnitDto.PartsCount,
                        IsDefaultSales = packingUnitDto.IsDefaultSales,
                        IsDefaultPurchases = packingUnitDto.IsDefaultPurchases,
                        OrderNumber = packingUnitDto.OrderNumber,
                        CreatedAt = now,
                        ModifiedAt = now,
                        VariantPackingUnitSellingPrices = packingUnitDto.SellingPrices.Select(sp => new VariantPackingUnitSellingPrice
                        {
                            Amount = sp.Amount,
                            SellingPriceId = sp.SellingPriceId,
                            CreatedAt = now,
                            ModifiedAt = now
                        }).ToList()
                    };
                    return packingUnit;
                }).ToList();
            }

            // Variant attributes
            if (command.VariantAttributeValues?.Any() == true)
            {
                variant.VariantAttributeValues = command.VariantAttributeValues.Select(attr => new VariantAttributeValue
                {
                    AttributeDefinitionId = attr.AttributeDefinitionId,
                    AttributeValueId = attr.AttributeValueId,
                    CreatedAt = now,
                    ModifiedAt = now
                }).ToList();
            }

            // Attachments (if any are provided from other flows)
            if (command.VariantAttachments?.Any() == true)
            {
                variant.VariantAttachments = command.VariantAttachments
                    .Where(dto => dto.Attachment != null)
                    .Select(dto => new VariantAttachment
                    {
                        Attachment = new Attachment
                        {
                            FileName = dto.Attachment!.FileName,
                            FileContentType = dto.Attachment.ContentType,
                            FileData = Convert.FromBase64String(dto.Attachment.FileContent),
                            CreatedAt = now,
                            ModifiedAt = now
                        },
                        AttachmentType = dto.AttachmentType,
                        Description = dto.Description,
                        IsPrimary = dto.IsPrimary,
                        DisplayOrder = dto.DisplayOrder,
                        CreatedAt = now,
                        ModifiedAt = now
                    })
                    .ToList();
            }

            variants.Add(variant);
        }

        return variants;
    }
}
