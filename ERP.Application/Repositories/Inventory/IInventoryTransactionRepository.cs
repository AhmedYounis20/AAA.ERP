using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Application.Repositories.Inventory;

public interface IInventoryTransactionRepository : IBaseRepository<InventoryTransaction>
{
    Task<InventoryTransaction?> GetWithItems(Guid id);
    Task<IEnumerable<InventoryTransaction>> GetByBranch(Guid branchId);
    Task<IEnumerable<InventoryTransaction>> GetByTransactionType(InventoryTransactionType transactionType);
} 