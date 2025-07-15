using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.Application.Repositories.Inventory;

public interface IStockBalanceRepository : IBaseRepository<StockBalance>
{
    Task<StockBalance?> GetByItemAndBranch(Guid itemId, Guid branchId);
    Task<StockBalance?> GetByItemPackingUnitAndBranch(Guid itemId, Guid packingUnitId, Guid branchId);
    Task<IEnumerable<StockBalance>> GetByBranch(Guid branchId);
    Task<IEnumerable<StockBalance>> GetByItem(Guid itemId);
    Task<decimal> GetCurrentBalance(Guid itemId, Guid packingUnitId, Guid branchId);
    Task<StockBalance?> GetByItemPackingUnitAndBranchWithoutInclues(Guid itemId, Guid packingUnitId, Guid branchId);

} 