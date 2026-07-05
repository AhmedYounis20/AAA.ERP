using Accounting.Domain.OutputDtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for import transaction.
/// </summary>
public interface IImportTransactionService : IBaseService<InventoryTransaction, ImportTransactionCreateCommand, ImportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
