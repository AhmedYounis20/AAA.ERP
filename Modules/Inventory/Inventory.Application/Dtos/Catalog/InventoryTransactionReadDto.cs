using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Dtos.Catalog;

/// <summary>Inventory transaction read model with Accounting display names (ADR-007 enrichment).</summary>
public class InventoryTransactionReadDto
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public InventoryTransactionType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public string? TransactionPartyName { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string? Notes { get; set; }
    public List<InventoryTransactionItem> Items { get; set; } = [];
}
