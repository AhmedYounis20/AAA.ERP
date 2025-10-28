using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;
using Shared.Domain.OutputDtos;

namespace Inventory.Application.Services;

public interface IInventoryTransactionService : IBaseService<InventoryTransaction, InventoryTransactionCreateCommand, InventoryTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetWithItems(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByTransactionType(InventoryTransactionType transactionType);
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
} 