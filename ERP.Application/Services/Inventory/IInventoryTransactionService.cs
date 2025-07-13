using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Application.Services.Inventory;

public interface IInventoryTransactionService : IBaseService<InventoryTransaction, InventoryTransactionCreateCommand, InventoryTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetWithItems(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByBranch(Guid branchId);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByTransactionType(InventoryTransactionType transactionType);
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
} 