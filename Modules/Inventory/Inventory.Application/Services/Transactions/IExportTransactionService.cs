using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Dtos.Catalog;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for export transaction.
/// </summary>
public interface IExportTransactionService : IBaseService<Inventory.Domain.Models.Entities.InventoryTransactions.InventoryTransaction, ExportTransactionCreateCommand, ExportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransactionReadDto>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransactionReadDto>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
