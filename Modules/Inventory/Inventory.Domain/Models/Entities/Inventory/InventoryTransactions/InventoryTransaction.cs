using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.InventoryTransactions;

public class InventoryTransaction : BaseEntity
{
    public InventoryTransactionType TransactionType { get; set; } = InventoryTransactionType.Receipt;
    public DateTime TransactionDate { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string? Notes { get; set; }

    public List<InventoryTransactionItem> Items { get; set; } = [];
}
