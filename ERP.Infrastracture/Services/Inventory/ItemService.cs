using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;
using System.Text.RegularExpressions;

namespace ERP.Infrastracture.Services.Inventory;
public class ItemService :
    BaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand>, IItemService
{

    IItemRepository _repository { get; set; }
    IBaseRepository<ItemCode> _codeRepossitory { get; set; }
    IUnitOfWork _unitOfWork { get; set; }

    public ItemService(IItemRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _codeRepossitory = _unitOfWork.ItemCodeRepository;
    }

    public async Task<ApiResponse<string>> GeNextCode(Guid? parentId = null)
    {
        var parent = await _codeRepossitory.GetQuery().Where(e => e.ItemId == parentId && e.CodeType == ItemCodeType.Code).OrderByDescending(e => e.CreatedAt).Select(e => e.Code).FirstOrDefaultAsync();
        var lastSibling = await _repository.GetQuery().Where(e => e.ParentId == parentId).OrderByDescending(e => e.CreatedAt).FirstOrDefaultAsync();
        var lastSiblingCode = lastSibling == null ? string.Empty : await _codeRepossitory.GetQuery().Where(e => e.ItemId == lastSibling.Id && e.CodeType == ItemCodeType.Code).Select(e => e.Code).FirstOrDefaultAsync();
        string code = GetNextChildCode(lastSiblingCode ?? string.Empty, parent ?? string.Empty);
        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = code
        };
    }
    public override async Task<ApiResponse<Item>> Create(ItemCreateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateCreate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<Item>
            {
                IsSuccess = false,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList(),
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for Domain + SubDomains creation
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            Item item = new Item
            {
                Name = command.Name,
                NameSecondLanguage = command.NameSecondLanguage,
                NodeType = command.NodeType,
                ParentId = command.ParentId,
                ItemCodes = [new ItemCode {
                    Code = command.Code,
                    CodeType = ItemCodeType.Code,
                }]
            };

            if (command.NodeType == NodeType.Domain)
            {
                if (!string.IsNullOrEmpty(command.EGSCode))
                {
                    item.ItemCodes.Add(new ItemCode
                    {
                        Code = command.EGSCode,
                        CodeType = ItemCodeType.EGS,
                    });
                }
                if (!string.IsNullOrEmpty(command.Gs1Code))
                {
                    item.ItemCodes.Add(new ItemCode
                    {
                        Code = command.Gs1Code,
                        CodeType = ItemCodeType.GS1,
                    });
                }
                item.Model = command.Model;
                item.Version = command.Version;
                item.CountryOfOrigin = command.CountryOfOrigin;
                item.ItemType = command.ItemType;
                item.ConditionalDiscount = command.ConditionalDiscount;
                item.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                item.DefaultDiscount = command.DefaultDiscount;
                item.DefaultDiscountType = command.DefaultDiscountType;
                item.ApplyDomainChanges = command.ApplyDomainChanges;
                if (command.IsDiscountBasedOnSellingPrice)
                {
                    HandleDicountBasedOnSellingPrice(command, item);
                }
                item.MaxDiscount = command.MaxDiscount;

                if (item.IsDiscountBasedOnSellingPrice)
                    item.DefaultDiscountType = 0;

                HandleItemSuppliers(command, item);
                HandleItemManufacturerCompanies(command, item);
                foreach (var barcode in command.BarCodes)
                    item.ItemCodes.Add(
                        new ItemCode
                        {
                            Code = barcode,
                            CodeType = ItemCodeType.BarCode
                        });

                HandleItemPackingUnitsPrices(command, item);
            }

            item = await _repository.Add(item);

            // Create SubDomains if this is a Domain item with combinations
            if (command.NodeType == NodeType.Domain && command.SubDomainCombinations?.Any() == true)
            {
                var subDomainResult = await CreateSubDomains(item.Id, command);
                if (!subDomainResult.IsSuccess)
                {
                    // Rollback transaction if SubDomain creation fails
                    await _unitOfWork.RollbackAsync();
                    return new ApiResponse<Item> 
                    { 
                        IsSuccess = false, 
                        StatusCode = HttpStatusCode.InternalServerError, 
                        Errors = subDomainResult.Errors 
                    };
                }
            }

            // Commit transaction - everything succeeded
            await _unitOfWork.CommitAsync();

            return new ApiResponse<Item> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = item };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "OPERATION_FAILD" } } };
        }
    }

    private static void HandleItemPackingUnitsPrices(IItemCommand command, Item item)
    {
        item.ItemPackingUnitPrices = [];
        foreach (var itempackingUnit in command.PackingUnits)
            item.ItemPackingUnitPrices.Add(
                new ItemPackingUnit
                {
                    PackingUnitId = itempackingUnit.PackingUnitId,
                    AverageCostPrice = itempackingUnit.AverageCostPrice,
                    IsDefaultPackingUnit = itempackingUnit.IsDefaultPackingUnit,
                    LastCostPrice = itempackingUnit.LastCostPrice,
                    PartsCount = itempackingUnit.PartsCount,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDefaultSales = itempackingUnit.IsDefaultSales,
                    IsDefaultPurchases = itempackingUnit.IsDefaultPurchases,
                    OrderNumber = itempackingUnit.OrderNumber,
                    ItemPackingUnitSellingPrices = itempackingUnit.SellingPrices.Select(e => new ItemPackingUnitSellingPrice
                    {
                        Amount = e.Amount,
                        SellingPriceId = e.SellingPriceId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,

                    }).ToList()
                });
    }

    private static void HandleItemManufacturerCompanies(IItemCommand command, Item item)
    {
        item.ItemManufacturerCompanies = [];
        foreach (var companyId in command.ManufacturerCompaniesIds)
            item.ItemManufacturerCompanies.Add(
                new ItemManufacturerCompany
                {
                    ManufacturerCompanyId = companyId,
                });
    }

    private static void HandleItemSuppliers(IItemCommand command, Item item)
    {
        item.ItemSuppliers = [];
        foreach (var supplierId in command.SuppliersIds)
            item.ItemSuppliers.Add(
                new ItemSupplier
                {
                    SupplierId = supplierId,
                });
    }

    private static void HandleDicountBasedOnSellingPrice(IItemCommand command, Item item)
    {
        List<ItemSellingPriceDiscount> discounts = command.SellingPriceDiscounts.Select(e => new ItemSellingPriceDiscount
        {
            SellingPriceId = e.SellingPriceId,
            Discount = e.Discount,
            DiscountType = e.DiscountType
        }).ToList();

        item.ItemSellingPriceDiscounts = discounts;
    }

    public override async Task<ApiResponse<Item>> Update(ItemUpdateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid || validationResult.entity == null)
        {
            return new ApiResponse<Item>
            {
                IsSuccess = false,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList(),
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        // Start transaction for Domain + SubDomains update
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            Item item = validationResult.entity;
            // Restrict SubDomain updates
            if (item.NodeType == NodeType.SubDomain)
            {
                // Do NOT update Name, NameSecondLanguage, or Code
                // Only update allowed fields
                item.ApplyDomainChanges = command.ApplyDomainChanges;
                item.ColorId = command.ColorId;
                item.SizeId = command.SizeId;
                item.ItemType = command.ItemType;
                item.MaxDiscount = command.MaxDiscount;
                item.ConditionalDiscount = command.ConditionalDiscount;
                item.DefaultDiscount = command.DefaultDiscount;
                item.DefaultDiscountType = command.DefaultDiscountType;
                item.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                item.Model = command.Model;
                item.Version = command.Version;
                item.CountryOfOrigin = command.CountryOfOrigin;
                await UpdateSuppliers(command, item);
                await UpdateBarCodes(command, item);
                await UpdateManufacturerCompanies(command, item);
                await UpdateSellingPriceDiscounts(command, item);
                await UpdateItemPackingUnits(command, item);
            }
            else
            {
                item.Name = command.Name;
                item.NameSecondLanguage = command.NameSecondLanguage;
                if (item.NodeType == NodeType.Domain)
                {
                    item.ApplyDomainChanges = command.ApplyDomainChanges;
                    item.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;

                    item.Model = command.Model;
                    item.Version = command.Version;
                    item.CountryOfOrigin = command.CountryOfOrigin;
                    item.MaxDiscount = command.MaxDiscount;
                    item.ConditionalDiscount = command.ConditionalDiscount;
                    item.DefaultDiscount = command.DefaultDiscount;
                    item.DefaultDiscountType = command.DefaultDiscountType;
                    item.ItemType = command.ItemType;

                    await UpdateSuppliers(command, item);
                    await UpdateBarCodes(command, item);
                    await UpdateManufacturerCompanies(command, item);
                    await UpdateSellingPriceDiscounts(command, item);
                    await UpdateItemPackingUnits(command, item);
                    if (item.ApplyDomainChanges)
                        await SyncDomainChangesToSubDomains(item, command);
                }
            }
            await _repository.Update(item);

            // Handle SubDomain combinations if this is a Domain item
            if (item.NodeType == NodeType.Domain && command.SubDomainCombinations?.Any() == true)
            {
                var subDomainResult = await CreateSubDomains(item.Id, command);
                if (!subDomainResult.IsSuccess)
                {
                    // Rollback transaction if SubDomain creation fails
                    await _unitOfWork.RollbackAsync();
                    return new ApiResponse<Item> 
                    { 
                        IsSuccess = false, 
                        StatusCode = HttpStatusCode.InternalServerError, 
                        Errors = subDomainResult.Errors 
                    };
                }
            }

            // Commit transaction - everything succeeded
            await _unitOfWork.CommitAsync();
            
            item.ItemCodes = [];
            item.ItemManufacturerCompanies = [];
            item.ItemSuppliers = [];

            return new ApiResponse<Item> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = item };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "OPERATION_FAILD" } } };
        }
    }

    private async Task UpdateSuppliers(IItemCommand command, Item item)
    {
        List<ItemSupplier> existedItemSuppliers = await _unitOfWork.ItemSupplierRepository.GetQuery().Where(e => e.ItemId == item.Id).ToListAsync();
        List<ItemSupplier> itemSupplierToRemove = existedItemSuppliers.Where(e => !command.SuppliersIds.Contains(e.SupplierId)).ToList();
        List<ItemSupplier> itemSuppliersToAdd = command.SuppliersIds.Select(e => new ItemSupplier
        {
            ItemId = item.Id,
            SupplierId = e,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        }).Where(a=>!existedItemSuppliers.Any(e=>e.SupplierId ==a.SupplierId)).ToList();

        if (itemSupplierToRemove != null && itemSupplierToRemove.Any())
            await _unitOfWork.ItemSupplierRepository.Delete(itemSupplierToRemove);
        if (itemSuppliersToAdd != null && itemSuppliersToAdd.Any())
            await _unitOfWork.ItemSupplierRepository.Add(itemSuppliersToAdd);
    }

    private async Task UpdateItemPackingUnits(IItemCommand command, Item item)
    {
        List<ItemPackingUnit> existedItemPackingUnits = await _unitOfWork.ItemPackingUnitRepository.GetQuery().Where(e => e.ItemId == item.Id).ToListAsync();
        List<ItemPackingUnit> itemPackingUnitsToRemove = existedItemPackingUnits.Where(e => !command.PackingUnits.Any(p => p.PackingUnitId == e.PackingUnitId)).ToList();
        List<ItemPackingUnit> itemPackingUnitsToAdd = [];
        foreach (var itempackingUnit in command.PackingUnits)
        {
            var existedItemPackingUnit = existedItemPackingUnits.FirstOrDefault(e => e.PackingUnitId == itempackingUnit.PackingUnitId);
            if (existedItemPackingUnit == null)
            {
                itemPackingUnitsToAdd.Add(new ItemPackingUnit
                {
                    ItemId = item.Id,
                    PackingUnitId = itempackingUnit.PackingUnitId,
                    AverageCostPrice = itempackingUnit.AverageCostPrice,
                    IsDefaultPackingUnit = itempackingUnit.IsDefaultPackingUnit,
                    LastCostPrice = itempackingUnit.LastCostPrice,
                    PartsCount = itempackingUnit.PartsCount,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDefaultSales = itempackingUnit.IsDefaultSales,
                    IsDefaultPurchases = itempackingUnit.IsDefaultPurchases,
                    OrderNumber = itempackingUnit.OrderNumber,
                    ItemPackingUnitSellingPrices = itempackingUnit.SellingPrices.Select(e => new ItemPackingUnitSellingPrice
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
                existedItemPackingUnit.PackingUnitId = itempackingUnit.PackingUnitId;
                existedItemPackingUnit.AverageCostPrice = itempackingUnit.AverageCostPrice;
                existedItemPackingUnit.IsDefaultPackingUnit = itempackingUnit.IsDefaultPackingUnit;
                existedItemPackingUnit.LastCostPrice = itempackingUnit.LastCostPrice;
                existedItemPackingUnit.PartsCount = itempackingUnit.PartsCount;
                existedItemPackingUnit.ModifiedAt = DateTime.Now;
                existedItemPackingUnit.IsDefaultSales = itempackingUnit.IsDefaultSales;
                existedItemPackingUnit.IsDefaultPurchases = itempackingUnit.IsDefaultPurchases;
                existedItemPackingUnit.OrderNumber = itempackingUnit.OrderNumber;

                await _unitOfWork.ItemPackingUnitRepository.Update(existedItemPackingUnit);

                await UpdatePackingUnitSellingPrices(itempackingUnit, existedItemPackingUnit);

            }
        }

        if (itemPackingUnitsToRemove != null && itemPackingUnitsToRemove.Any())
            await _unitOfWork.ItemPackingUnitRepository.Delete(itemPackingUnitsToRemove);
        if (itemPackingUnitsToAdd != null && itemPackingUnitsToAdd.Any())
            await _unitOfWork.ItemPackingUnitRepository.Add(itemPackingUnitsToAdd);
    }

    private async Task UpdatePackingUnitSellingPrices(ItemPackingUnitDto itempackingUnit, ItemPackingUnit? existedItemPackingUnit)
    {
        var existedPackingUnitPrices = await _unitOfWork.ItemPackingUnitSellingPriceRepository.GetQuery().Where(e => e.ItemPackingUnitId == existedItemPackingUnit.Id).ToListAsync();
        List<ItemPackingUnitSellingPrice> packingUnitPricesToAdd = [];
        List<ItemPackingUnitSellingPrice> packingUnitPricesToUpdate = [];
        foreach (var packingUnitPrice in itempackingUnit.SellingPrices)
        {
            var existedPackingUnitPrice = existedPackingUnitPrices.FirstOrDefault(e => e.SellingPriceId == packingUnitPrice.SellingPriceId);
            if (existedPackingUnitPrice == null)
            {
                packingUnitPricesToAdd.Add(
                           new ItemPackingUnitSellingPrice
                           {
                               SellingPriceId = packingUnitPrice.SellingPriceId,
                               CreatedAt = DateTime.Now,
                               ModifiedAt = DateTime.Now,
                               ItemPackingUnitId = existedItemPackingUnit.Id,
                               Amount = packingUnitPrice.Amount
                           });
            }
            else
            {
                existedPackingUnitPrice.Amount = packingUnitPrice.Amount;
                existedPackingUnitPrice.ModifiedAt = DateTime.Now;
                packingUnitPricesToUpdate.Add(existedPackingUnitPrice);
            }
        }

        await _unitOfWork.ItemPackingUnitSellingPriceRepository.Add(packingUnitPricesToAdd);
        await _unitOfWork.ItemPackingUnitSellingPriceRepository.Update(packingUnitPricesToUpdate);
    }

    private async Task UpdateSellingPriceDiscounts(IItemCommand command, Item item)
    {
        List<ItemSellingPriceDiscount> existedItemSuppliers = await _unitOfWork.ItemSellingPriceDiscountRepository.GetQuery().Where(e => e.ItemId == item.Id).ToListAsync();
        List<ItemSellingPriceDiscount> itemSellingPriceDiscountsToAdd = command.SellingPriceDiscounts.Select(e => new ItemSellingPriceDiscount
            {
                ItemId = item.Id,
                SellingPriceId = e.SellingPriceId,
                Discount = e.Discount,
                DiscountType = e.DiscountType,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
        }).Where(e=> !existedItemSuppliers.Any(old=>old.SellingPriceId == e.SellingPriceId)).ToList();

        List<ItemSellingPriceDiscount> itemSellingPricesToUpdate = [];
        List<ItemSellingPriceDiscount> itemSellingPricesToRemove = [];
        foreach (var sellingPriceDiscount in existedItemSuppliers)
        {
            var updatedDiscount = command.SellingPriceDiscounts.FirstOrDefault(e => e.SellingPriceId == sellingPriceDiscount.SellingPriceId);
            if (updatedDiscount != null)
            {
                sellingPriceDiscount.Discount = updatedDiscount.Discount;
                sellingPriceDiscount.DiscountType = updatedDiscount.DiscountType;
                sellingPriceDiscount.ModifiedAt = DateTime.Now;

                itemSellingPricesToUpdate.Add(sellingPriceDiscount);
            }
            else
                itemSellingPriceDiscountsToAdd.Add(sellingPriceDiscount);
        }

        if (itemSellingPriceDiscountsToAdd != null && itemSellingPriceDiscountsToAdd.Any())
            await _unitOfWork.ItemSellingPriceDiscountRepository.Add(itemSellingPriceDiscountsToAdd);
        if (itemSellingPricesToUpdate != null && itemSellingPricesToUpdate.Any())
            await _unitOfWork.ItemSellingPriceDiscountRepository.Update(itemSellingPricesToUpdate);
        if (itemSellingPricesToRemove != null && itemSellingPricesToRemove.Any())
            await _unitOfWork.ItemSellingPriceDiscountRepository.Delete(itemSellingPricesToRemove);
    }


    private async Task UpdateManufacturerCompanies(IItemCommand command, Item item)
    {
        List<ItemManufacturerCompany> existedItemManufacturerCompanies = await _unitOfWork.ItemManufacturerCompanyRepository.GetQuery().Where(e => e.ItemId == item.Id).ToListAsync();
        List<ItemManufacturerCompany> itemManufacturerCompaniesToRemove = existedItemManufacturerCompanies.Where(e => !command.SuppliersIds.Contains(e.ManufacturerCompanyId)).ToList();
        List<ItemManufacturerCompany> itemManufacturerCompaniesToAdd = command.ManufacturerCompaniesIds.Select(e => new ItemManufacturerCompany
        {
            ItemId = item.Id,
            ManufacturerCompanyId = e,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        }).Where(a => !existedItemManufacturerCompanies.Any(e => e.ManufacturerCompanyId == a.ManufacturerCompanyId)).ToList();

        if (itemManufacturerCompaniesToRemove != null && itemManufacturerCompaniesToRemove.Any())
            await _unitOfWork.ItemManufacturerCompanyRepository.Delete(itemManufacturerCompaniesToRemove);
        if (itemManufacturerCompaniesToAdd != null && itemManufacturerCompaniesToAdd.Any())
            await _unitOfWork.ItemManufacturerCompanyRepository.Add(itemManufacturerCompaniesToAdd);
    }

    private async Task UpdateBarCodes(IItemCommand command, Item item)
    {
        List<ItemCode> existedItemBarCodes = await _unitOfWork.ItemCodeRepository.GetQuery().Where(e => e.ItemId == item.Id && e.CodeType == ItemCodeType.BarCode).ToListAsync();
        List<ItemCode> itemBarCodesToRemove = existedItemBarCodes.Where(e => !command.BarCodes.Contains(e.Code)).ToList();
        
        // Get existing barcode codes for comparison
        var existingBarcodeCodes = existedItemBarCodes.Select(e => e.Code).ToList();
        
        // Only add barcodes that don't already exist for this item
        List<ItemCode> itemBarCodesToAdd = command.BarCodes
            .Where(barcode => !existingBarcodeCodes.Contains(barcode))
            .Select(e => new ItemCode
            {
                Code = e,
                CodeType = ItemCodeType.BarCode,
                ItemId = item.Id,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            }).ToList();

        if (itemBarCodesToRemove != null && itemBarCodesToRemove.Any())
            await _unitOfWork.ItemCodeRepository.Delete(itemBarCodesToRemove);
        if (itemBarCodesToAdd != null && itemBarCodesToAdd.Any())
            await _unitOfWork.ItemCodeRepository.Add(itemBarCodesToAdd);
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ItemCreateCommand command)
    {
        var validationResult = await base.ValidateCreate(command);
        var checkRepeatedCode = _codeRepossitory.GetQuery().Any(e => e.Code == command.Code);
        if (checkRepeatedCode)
        {

            validationResult.isValid = false;
            validationResult.errors.Add("ITEM_CODE_IS_DUPLICATED");
        }

        if (command.Gs1Code is not null)
        {
            var checkRepeatedGs1Code = await _codeRepossitory.GetQuery().AnyAsync(e => e.Code == command.Gs1Code);
            if (checkRepeatedGs1Code)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ITEM_GS1_CODE_IS_DUPLICATED");
            }
        }

        if (command.EGSCode is not null)
        {
            var checkRepeatedEgsCode = await _codeRepossitory.GetQuery().AnyAsync(e => e.Code == command.EGSCode);
            if (checkRepeatedEgsCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ITEM_EGS_CODE_IS_DUPLICATED");
            }
        }

        if (command.BarCodes != null && command.BarCodes.Count != command.BarCodes.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_BARCODES");
        }
        if (command.SuppliersIds != null && command.SuppliersIds.Count != command.SuppliersIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS");
        }
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Count != command.ManufacturerCompaniesIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES");
        }

        if (command.BarCodes is not null)
        {
            var checkRepeatedBarCode = await _codeRepossitory.GetQuery().AnyAsync(e => command.BarCodes.Contains(e.Code));
            if (checkRepeatedBarCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ITEM_ONE_OF_BARCODES_IS_DUPLICATED");
            }
        }

        var existedSuppliersCount = await _unitOfWork.SupplierRepository.GetQuery().CountAsync(e => command.SuppliersIds != null && command.SuppliersIds.Contains(e.Id));
        if (command.SuppliersIds != null && command.SuppliersIds.Any() && command.SuppliersIds.Count > existedSuppliersCount)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("ONE_OF_SUPPLIERS_NOT_FOUND");
        }

        var existedManufacturerCompany = await _unitOfWork.ItemManufacturerCompanyRepository.GetQuery().CountAsync(e => command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Contains(e.Id));
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Any() && command.ManufacturerCompaniesIds.Count > existedManufacturerCompany)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("ONE_OF_MANUFACTURER_COMPANIES_NOT_FOUND");
        }

        // Prevent SubDomain as parent
        if (command.ParentId != null)
        {
            var parent = await _repository.Get(command.ParentId.Value);
            if (parent != null && parent.NodeType == NodeType.SubDomain)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("CANNOT_SET_SUBDOMAIN_AS_PARENT");
            }
        }

        return validationResult;
    }

    protected override async Task<(bool isValid, List<string> errors, Item? entity)> ValidateUpdate(ItemUpdateCommand command)
    {
        var validationResult = await base.ValidateUpdate(command);
        if (command.BarCodes != null && command.BarCodes.Count != command.BarCodes.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_BARCODES");
        }
        if (command.SuppliersIds != null && command.SuppliersIds.Count != command.SuppliersIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_SUPPLIERS");
        }
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Count != command.ManufacturerCompaniesIds.Distinct().Count())
        {
            validationResult.isValid = false;
            validationResult.errors.Add("PLEASE_PROVIDE_NON_DUPLICATE_MANUFACTURER_COMPANIES");
        }

        if (command.BarCodes is not null)
        {
            var checkRepeatedBarCode = await _codeRepossitory.GetQuery().AnyAsync(e => e.ItemId != command.Id && command.BarCodes.Contains(e.Code));
            if (checkRepeatedBarCode)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("ITEM_ONE_OF_BARCODES_IS_DUPLICATED");
            }
        }

        var existedSuppliersCount = await _unitOfWork.SupplierRepository.GetQuery().CountAsync(e => command.SuppliersIds != null && command.SuppliersIds.Contains(e.Id));
        if (command.SuppliersIds != null && command.SuppliersIds.Any() && command.SuppliersIds.Count > existedSuppliersCount)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("ONE_OF_SUPPLIERS_NOT_FOUND");
        }

        var existedManufacturerCompany = await _unitOfWork.ItemManufacturerCompanyRepository.GetQuery().CountAsync(e => command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Contains(e.Id));
        if (command.ManufacturerCompaniesIds != null && command.ManufacturerCompaniesIds.Any() && command.ManufacturerCompaniesIds.Count > existedManufacturerCompany)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("ONE_OF_MANUFACTURER_COMPANIES_NOT_FOUND");
        }

        // Prevent SubDomain as parent
        if (command.ParentId != null)
        {
            var parent = await _repository.Get(command.ParentId.Value);
            if (parent != null && parent.NodeType == NodeType.SubDomain)
            {
                validationResult.isValid = false;
                validationResult.errors.Add("CANNOT_SET_SUBDOMAIN_AS_PARENT");
            }
        }
        return validationResult;
    }
    public static string GetNextChildCode(string lastChildCode, string parentCode)
    {
        if (string.IsNullOrEmpty(lastChildCode))
        {
            // Return just parentCode + "1" if parent exists, else just "1"
            return string.IsNullOrEmpty(parentCode) ? "1" : parentCode + "1";
        }

        bool hasParentPrefix = !string.IsNullOrEmpty(parentCode) && lastChildCode.StartsWith(parentCode);
        string suffix = hasParentPrefix
            ? lastChildCode.Substring(parentCode.Length)
            : lastChildCode;

        var match = Regex.Match(suffix, @"^(.*?)(\d+)$");

        string newSuffix;
        if (match.Success)
        {
            string prefix = match.Groups[1].Value;
            string numberPart = match.Groups[2].Value;

            int number = int.Parse(numberPart);
            number++;

            string newNumber = number.ToString().PadLeft(numberPart.Length, '0');
            newSuffix = prefix + newNumber;
        }
        else
        {
            // No digits found, just add "1"
            newSuffix = suffix + "1";
        }

        return hasParentPrefix ? parentCode + newSuffix : newSuffix;
    }


    public async Task<ApiResponse<ItemDto?>> GetItemDtoById(Guid id)
    {
        var item = await _repository.GetDtoById(id);
        
        // If this is a Domain item, get the existing SubDomain combinations
        if (item != null && item.NodeType == NodeType.Domain)
        {
            var existingSubDomains = await _repository.GetQuery()
                .Where(i => i.ParentId == id && i.NodeType == NodeType.SubDomain)
                .ToListAsync();

            var subDomainCombinations = existingSubDomains.Select(subDomain => new ColorSizeCombinationDto
            {
                ColorId = subDomain.ColorId ?? Guid.Empty,
                SizeId = subDomain.SizeId ?? Guid.Empty,
                ApplyDomainChanges = subDomain.ApplyDomainChanges
            }).ToList();

            // Add the combinations to the item DTO
            item.SubDomainCombinations = subDomainCombinations;
        }
        
        return new ApiResponse<ItemDto?>
        {
            StatusCode = item == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
            IsSuccess = item != null,
            Result = item
        };
    }

    public async Task<ApiResponse<List<ItemDto>>> GetItemDtos()
    {
        var items = await _repository.GetQuery().Include(e => e.ItemCodes).ToListAsync();
        var itemDtos = items.Select(e => new ItemDto
        {
            Id = e.Id,
            Name = e.Name,
            NameSecondLanguage = e.NameSecondLanguage,
            Code = e.Code?.Code ?? string.Empty,
            NodeType = e.NodeType,
            ParentId = e.ParentId,
            CreatedAt = e.CreatedAt,
            ModifiedAt = e.ModifiedAt,
            ItemCodes = e.ItemCodes
        }).OrderBy(e=> e.Code).ToList();
        return new ApiResponse<List<ItemDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = itemDtos
        };
    }
    public async Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants()
    {
        var items = await _repository.GetVariants();
        
        return new ApiResponse<IEnumerable<ItemDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
    // Transaction-based SubDomain creation method
    private async Task<ApiResponse<List<Item>>> CreateSubDomains(Guid domainItemId, IItemCommand command)
    {
        try
        {
            var domainItem = await _repository.Get(domainItemId);
            if (domainItem == null || domainItem.NodeType != NodeType.Domain)
            {
                return new ApiResponse<List<Item>>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "InvalidDomainItem" } }
                };
            }

            var createdSubDomains = new List<Item>();
            
            foreach (var combination in command.SubDomainCombinations)
            {
                // Check if SubDomain already exists
                var existingSubDomain = await _repository.GetQuery()
                    .Where(i => i.ParentId == domainItemId && 
                               i.ColorId == combination.ColorId && 
                               i.SizeId == combination.SizeId)
                    .FirstOrDefaultAsync();

                if (existingSubDomain != null)
                    continue; // Skip if already exists

                // Create SubDomain name: ItemName + ColorCode + SizeCode
                var color = await _unitOfWork.ColorRepository.Get(combination.ColorId);
                var size = await _unitOfWork.SizeRepository.Get(combination.SizeId);
                
                var subDomainName = $"{domainItem.Name} - {color?.Code} - {size?.Code}";
                var subDomainNameSecondLanguage = $"{domainItem.NameSecondLanguage} - {color?.Code} - {size?.Code}";

                // Generate code for SubDomain using GetNextChildCode
                var domainCode = await _codeRepossitory.GetQuery()
                    .Where(e => e.ItemId == domainItemId && e.CodeType == ItemCodeType.Code)
                    .Select(e => e.Code)
                    .FirstOrDefaultAsync();

                var lastSubDomainCode = await _repository.GetQuery()
                    .Where(i => i.ParentId == domainItemId)
                    .OrderByDescending(e => e.CreatedAt)
                    .SelectMany(i => i.ItemCodes)
                    .Where(c => c.CodeType == ItemCodeType.Code)
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => c.Code)
                    .FirstOrDefaultAsync();

                var subDomainCode = GetNextChildCode(lastSubDomainCode ?? string.Empty, domainCode ?? string.Empty);

                var subDomainItem = new Item
                {
                    Name = subDomainName,
                    NameSecondLanguage = subDomainNameSecondLanguage,
                    NodeType = NodeType.SubDomain,
                    ParentId = domainItemId, // SubDomain's parent should be the Domain item itself
                    ColorId = combination.ColorId,
                    SizeId = combination.SizeId,
                    ApplyDomainChanges = combination.ApplyDomainChanges,
                    
                    // Copy properties from domain
                    ItemType = domainItem.ItemType,
                    MaxDiscount = domainItem.MaxDiscount,
                    ConditionalDiscount = domainItem.ConditionalDiscount,
                    DefaultDiscount = domainItem.DefaultDiscount,
                    DefaultDiscountType = domainItem.DefaultDiscountType,
                    IsDiscountBasedOnSellingPrice = domainItem.IsDiscountBasedOnSellingPrice,
                    Model = domainItem.Model,
                    Version = domainItem.Version,
                    CountryOfOrigin = domainItem.CountryOfOrigin,
                    
                    // Initialize collections with generated code
                    ItemCodes = [new ItemCode
                    {
                        Code = subDomainCode,
                        CodeType = ItemCodeType.Code,
                    }],
                    ItemSuppliers = [],
                    ItemManufacturerCompanies = [],
                    ItemSellingPriceDiscounts = [],
                    ItemPackingUnitPrices = []
                };

                if (command.IsDiscountBasedOnSellingPrice)
                {
                    HandleDicountBasedOnSellingPrice(command, subDomainItem);
                }
                HandleItemSuppliers(command, subDomainItem);
                HandleItemManufacturerCompanies(command, subDomainItem);
                HandleItemPackingUnitsPrices(command, subDomainItem);

                var createdSubDomain = await _repository.Add(subDomainItem);
                createdSubDomains.Add(createdSubDomain);
            }

            return new ApiResponse<List<Item>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = createdSubDomains
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Item>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<bool>> SyncDomainChangesToSubDomains(Item domainItem, IItemCommand command)
    {
        // Start transaction for Domain-to-SubDomain synchronization        
        try
        {
            if (domainItem == null || domainItem.NodeType != NodeType.Domain)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "InvalidDomainItem" } }
                };
            }

            var subDomains = await _repository.GetQuery()
                .Where(i => i.ParentId == domainItem.Id && i.ApplyDomainChanges)
                .ToListAsync();

            foreach (var subDomain in subDomains)
            {
                // Sync properties from domain to subdomain
                if (subDomain.ApplyDomainChanges)
                {
                    var color = await _unitOfWork.ColorRepository.Get(subDomain.ColorId);
                    var size = await _unitOfWork.SizeRepository.Get(subDomain.SizeId);
                
                    var subDomainName = $"{domainItem.Name} - {color?.Code} - {size?.Code}";
                    var subDomainNameSecondLanguage = $"{domainItem.NameSecondLanguage} - {color?.Code} - {size?.Code}";

                    subDomain.Name = subDomainName;
                    subDomain.NameSecondLanguage = subDomainNameSecondLanguage;
                    subDomain.ItemType = domainItem.ItemType;
                    subDomain.MaxDiscount = domainItem.MaxDiscount;
                    subDomain.ConditionalDiscount = domainItem.ConditionalDiscount;
                    subDomain.DefaultDiscount = domainItem.DefaultDiscount;
                    subDomain.DefaultDiscountType = domainItem.DefaultDiscountType;
                    subDomain.IsDiscountBasedOnSellingPrice = domainItem.IsDiscountBasedOnSellingPrice;
                    subDomain.Model = domainItem.Model;
                    subDomain.Version = domainItem.Version;
                    subDomain.CountryOfOrigin = domainItem.CountryOfOrigin;

                    if (command.IsDiscountBasedOnSellingPrice)
                    {
                        await UpdateSellingPriceDiscounts(command, subDomain);
                    }
                    await UpdateBarCodes(command, subDomain);
                    await UpdateSuppliers(command, subDomain);
                    await UpdateManufacturerCompanies(command, subDomain);
                    await UpdateItemPackingUnits(command, subDomain);
                }

                await _repository.Update(subDomain);
            }

            // Commit transaction - all syncs succeeded

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }
        catch (Exception ex)
        {
            // Rollback transaction on any exception
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }
}