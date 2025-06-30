using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
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
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
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
                item.ItemSuppliers = [];
                item.ItemType = command.ItemType;
                item.ConditionalDiscount = command.ConditionalDiscount;
                item.DefaultDiscount = command.DefaultDiscount;
                item.DefaultDiscountType = command.DefaultDiscountType;
                item.IsDiscountBasedOnSellingPrice = command.IsDiscountBasedOnSellingPrice;
                item.MaxDiscount = command.MaxDiscount;

                if (item.IsDiscountBasedOnSellingPrice)
                    item.DefaultDiscountType = 0;


                foreach (var supplierId in command.SuppliersIds)
                    item.ItemSuppliers.Add(
                        new ItemSupplier
                        {
                            SupplierId = supplierId,
                        });
                item.ItemManufacturerCompanies = [];
                foreach (var companyId in command.ManufacturerCompaniesIds)
                    item.ItemManufacturerCompanies.Add(
                        new ItemManufacturerCompany
                        {
                            ManufacturerCompanyId = companyId,
                        });
                foreach (var barcode in command.BarCodes)
                    item.ItemCodes.Add(
                        new ItemCode
                        {
                            Code = barcode,
                            CodeType = ItemCodeType.BarCode
                        });
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

            item = await _repository.Add(item);

            return new ApiResponse<Item> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = item };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }
    public override async Task<ApiResponse<Item>> Update(ItemUpdateCommand command, bool isValidate = true)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid || validationResult.entity == null)
        {
            return new ApiResponse<Item>
            {
                IsSuccess = false,
                ErrorMessages = validationResult.errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            Item item = validationResult.entity;
            item.Name = command.Name;
            item.NameSecondLanguage = command.NameSecondLanguage;

            if (item.NodeType == NodeType.Domain)
            {
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
            }
            await _repository.Update(item);



            await _unitOfWork.CommitAsync();
            item.ItemCodes = [];
            item.ItemManufacturerCompanies = [];
            item.ItemSuppliers = [];

            return new ApiResponse<Item> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = item };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }

    private async Task UpdateSuppliers(ItemUpdateCommand command, Item item)
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

    private async Task UpdateItemPackingUnits(ItemUpdateCommand command, Item item)
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
                    ItemId = command.Id,
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

    private async Task UpdateSellingPriceDiscounts(ItemUpdateCommand command, Item item)
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
        }).Except(existedItemSuppliers).ToList();

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


    private async Task UpdateManufacturerCompanies(ItemUpdateCommand command, Item item)
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

    private async Task UpdateBarCodes(ItemUpdateCommand command, Item item)
    {
        List<ItemCode> existedItemBarCodes = await _unitOfWork.ItemCodeRepository.GetQuery().Where(e => e.ItemId == item.Id && e.CodeType == ItemCodeType.BarCode).ToListAsync();
        List<ItemCode> itemBarCodesToRemove = existedItemBarCodes.Where(e => !command.BarCodes.Contains(e.Code)).ToList();
        List<ItemCode> itemBarCodesToAdd = command.BarCodes.Select(e => new ItemCode
        {
            Code = e,
            CodeType = ItemCodeType.BarCode,
            ItemId = item.Id,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        }).Except(existedItemBarCodes).ToList();

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
        return new ApiResponse<ItemDto?>
        {
            StatusCode = item == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
            IsSuccess = item != null,
            Result = item
        };
    }

    public async Task<ApiResponse<List<ItemDto>>> GetItemDtos()
    {
        var item = await _repository.GetDtos();
        return new ApiResponse<List<ItemDto>>
        {
            StatusCode = item == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
            IsSuccess = item != null,
            Result = item?.ToList()
        };
    }
}