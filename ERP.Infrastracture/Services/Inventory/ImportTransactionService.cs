using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Infrastracture.Services.Inventory;

public class ImportTransactionService : BaseService<InventoryTransaction,ImportTransactionCreateCommand,ImportTransactionUpdateCommand>, IImportTransactionService
{
    private readonly IInventoryTransactionService _inventoryTransactionService;
    private readonly IInventoryTransactionRepository _repository;

    public ImportTransactionService(
        IInventoryTransactionService inventoryTransactionService,
        IInventoryTransactionRepository repository) 
        : base(repository)
    {
        _inventoryTransactionService = inventoryTransactionService;
        _repository = repository;
    }

    public override async Task<ApiResponse<InventoryTransaction>> Create(ImportTransactionCreateCommand command, bool isValidate = true)
    {
        // Convert to base command
        var baseCommand = new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = command.TransactionDate,
            DocumentNumber = command.DocumentNumber,
            TransactionPartyId = command.TransactionPartyId,
            BranchId = command.BranchId,
            Items = command.Items
        };

        return await _inventoryTransactionService.Create(baseCommand);
    }

    public async Task<ApiResponse<InventoryTransaction>> GetById(Guid id)
    {
        var transaction = await _repository.GetWithItems(id);
        if (transaction == null)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "TransactionNotFound" } }
            };
        }

        if (transaction.TransactionType != InventoryTransactionType.Receipt)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "TransactionIsNotImportTransaction" } }
            };
        }

        return new ApiResponse<InventoryTransaction>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = transaction
        };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll()
    {
        var result = await _inventoryTransactionService.GetByTransactionType(InventoryTransactionType.Receipt);
        if (result.IsSuccess)
        {
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.Result
            };
        }
        return new ApiResponse<IEnumerable<InventoryTransaction>>
        {
            IsSuccess = false,
            StatusCode = result.StatusCode,
            Errors = result.Errors
        };
    }

    public async Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
    {
        return await _inventoryTransactionService.GetTransactionNumber(dateTime);
    }
} 