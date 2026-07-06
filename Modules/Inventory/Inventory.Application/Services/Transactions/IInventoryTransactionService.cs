using Accounting.Contracts.Dtos;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Inventory transaction read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IInventoryTransactionService
{
    Task<ApiResponse<InventoryTransaction>> GetWithItems(Guid id);

    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByBranch(Guid branchId);

    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByTransactionType(InventoryTransactionType transactionType);

    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
