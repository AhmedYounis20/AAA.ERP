using AAA.ERP.OutputDtos;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Application.Services.Inventory;

public interface IImportTransactionService : IBaseService<InventoryTransaction,ImportTransactionCreateCommand,ImportTransactionUpdateCommand>
{
    Task<ApiResponse<InventoryTransaction>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetAll();
    Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime);
} 