using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Application.Services;

public interface IImportTransactionService : IBaseService<InventoryTransaction, ImportTransactionCreateCommand, ImportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
}
