using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Commands.InventoryTransactions;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;
using Shared.Domain.OutputDtos;

namespace Inventory.Application.Services;

public interface IImportTransactionService : IBaseService<InventoryTransaction,ImportTransactionCreateCommand,ImportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
} 