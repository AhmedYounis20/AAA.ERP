using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Application.Repositories;

public interface IInventoryTransactionRepository : IBaseRepository<InventoryTransaction>
{
    Task<InventoryTransaction?> GetWithItems(Guid id);
    Task<IEnumerable<InventoryTransaction>> GetByBranch(Guid branchId);
    Task<IEnumerable<InventoryTransaction>> GetByTransactionType(InventoryTransactionType transactionType);
    Task<bool> TransactionAfterThisDateExist(DateTime date);
} 