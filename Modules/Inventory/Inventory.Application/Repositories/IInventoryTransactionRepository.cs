using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for inventory transaction persistence.
/// </summary>
public interface IInventoryTransactionRepository : IBaseRepository<InventoryTransaction>
{
    /// <summary>Gets with items.</summary>
    Task<InventoryTransaction?> GetWithItems(Guid id);
    Task<IEnumerable<InventoryTransaction>> GetByBranch(Guid branchId);
    Task<IEnumerable<InventoryTransaction>> GetByTransactionType(InventoryTransactionType transactionType);
    /// <summary>transaction after this date exist operation.</summary>
    Task<bool> TransactionAfterThisDateExist(DateTime date);
} 
