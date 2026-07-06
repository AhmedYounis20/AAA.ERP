using Accounting.Contracts.Dtos;
using Inventory.Application.Dtos.Catalog;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Import transaction read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IImportTransactionService
{
    Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id);

    Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll();

    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
