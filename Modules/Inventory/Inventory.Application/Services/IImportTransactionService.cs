using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Dtos.Inventory;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for import transaction.
/// </summary>
public interface IImportTransactionService : IBaseService<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction, ImportTransactionCreateCommand, ImportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
