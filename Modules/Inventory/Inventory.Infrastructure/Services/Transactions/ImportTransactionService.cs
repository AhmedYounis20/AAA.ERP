using Accounting.Contracts.Integration;
using Inventory.Application.Repositories;
using ERP.Infrastructure.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Application.Dtos.Catalog;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Infrastructure.Mapping;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Services;

public class ImportTransactionService : BaseService<InventoryTransaction, ImportTransactionCreateCommand, ImportTransactionUpdateCommand>, IImportTransactionService
{
    private readonly IInventoryTransactionWorkflowService _inventoryTransactionWorkflow;
    private readonly IInventoryTransactionService _inventoryTransactionService;
    private readonly IInventoryTransactionRepository _repository;
    private readonly IInventoryAccountingDisplayQueries _displayQueries;

    public ImportTransactionService(
        IInventoryTransactionWorkflowService inventoryTransactionWorkflow,
        IInventoryTransactionService inventoryTransactionService,
        IInventoryTransactionRepository repository,
        IInventoryAccountingDisplayQueries displayQueries)
        : base(repository)
    {
        _inventoryTransactionWorkflow = inventoryTransactionWorkflow;
        _inventoryTransactionService = inventoryTransactionService;
        _repository = repository;
        _displayQueries = displayQueries;
    }

    public override async Task<ApiResponse<InventoryTransaction>> Create(ImportTransactionCreateCommand command, bool isValidate = true)
    {
        var baseCommand = new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = command.TransactionDate,
            DocumentNumber = command.DocumentNumber,
            TransactionPartyId = command.TransactionPartyId,
            BranchId = command.BranchId,
            Items = command.Items
        };

        return await _inventoryTransactionWorkflow.Create(baseCommand);
    }

    public async Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id)
    {
        var transaction = await _repository.GetWithItems(id);
        if (transaction == null)
        {
            return new ApiResponse<InventoryTransactionReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransactionNotFound }]
            };
        }

        if (transaction.TransactionType != InventoryTransactionType.Receipt)
        {
            return new ApiResponse<InventoryTransactionReadDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransactionIsNotImportTransaction }]
            };
        }

        var dto = await InventoryReadDtoMapper.ToReadDtoAsync(transaction, _displayQueries);
        return new ApiResponse<InventoryTransactionReadDto>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = dto
        };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll()
    {
        var result = await _inventoryTransactionService.GetByTransactionType(InventoryTransactionType.Receipt);
        if (!result.IsSuccess || result.Result is null)
        {
            return new ApiResponse<IEnumerable<InventoryTransactionReadDto>>
            {
                IsSuccess = false,
                StatusCode = result.StatusCode,
                Errors = result.Errors
            };
        }

        var dtos = await InventoryReadDtoMapper.ToReadDtosAsync(result.Result, _displayQueries);
        return new ApiResponse<IEnumerable<InventoryTransactionReadDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = dtos
        };
    }

    public async Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
    {
        return await _inventoryTransactionService.GetTransactionNumber(dateTime);
    }
}
