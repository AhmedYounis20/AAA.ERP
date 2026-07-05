using Accounting.Domain.OutputDtos;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Services;

public interface IExportTransactionService : IBaseService<InventoryTransaction, ExportTransactionCreateCommand, ExportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
