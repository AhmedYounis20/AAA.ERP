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

    public ItemService(IItemRepository repository,IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _unitOfWork=  unitOfWork;
        _codeRepossitory = _unitOfWork.ItemCodeRepository;
    }

    public async Task<ApiResponse<string>> GeNextCode(Guid? parentId = null)
    {
        var parent = await _codeRepossitory.GetQuery().Where(e => e.ItemId == parentId && e.CodeType == ItemCodeType.Code).OrderByDescending(e => e.CreatedAt).Select(e => e.Code).FirstOrDefaultAsync();
        var lastSibling = await _repository.GetQuery().Where(e=>e.ParentId == parentId).OrderByDescending(e=>e.CreatedAt).FirstOrDefaultAsync();
        var lastSiblingCode = lastSibling == null ? string.Empty : await _codeRepossitory.GetQuery().Where(e => e.ItemId == lastSibling.Id && e.CodeType == ItemCodeType.Code).Select(e=>e.Code).FirstOrDefaultAsync();
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

            if (command.NodeType == NodeType.Category)
                item = await _repository.Add(item);
            item.ItemCodes = [];
             return new ApiResponse<Item> { IsSuccess = true,StatusCode = HttpStatusCode.OK,Result = item};
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }
    public override async Task<ApiResponse<Item>> Update(ItemUpdateCommand command, bool isValidate = true)
    {
        var validationResult = await  ValidateUpdate(command);
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
            Item item = validationResult.entity;
            item.Name = command.Name;
            item.NameSecondLanguage = command.NameSecondLanguage;
            await _repository.Update(item);
           
            item.ItemCodes = [];
            return new ApiResponse<Item> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = item };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return new ApiResponse<Item> { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, ErrorMessages = ["OPERATION_FAILD"] };
        }
    }
    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ItemCreateCommand command)
    {
        var validationResult= await base.ValidateCreate(command);
        var checkRepeatedCode = _codeRepossitory.GetQuery().Any(e => e.Code == command.Code);
        if (checkRepeatedCode)
        {

            validationResult.isValid = false;
            validationResult.errors.Add("ITEM_CODE_IS_DUPLICATED");
        }

        return validationResult;
    }

    protected override Task<(bool isValid, List<string> errors, Item? entity)> ValidateUpdate(ItemUpdateCommand command)
    {
        return base.ValidateUpdate(command);
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
}