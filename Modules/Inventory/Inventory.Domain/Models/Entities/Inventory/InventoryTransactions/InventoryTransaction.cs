using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

public class InventoryTransaction : BaseEntity
{
    public InventoryTransactionType TransactionType { get; set; } = InventoryTransactionType.Receipt;
    public DateTime TransactionDate { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public ChartOfAccount? TransactionParty { get; set; }
    public Guid BranchId { get; set;}
    public Branch? Branch { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public FinancialPeriod? FinancialPeriod { get; set; }
    public string? Notes { get; set; }

    public List<InventoryTransactionItem> Items { get; set; } = [];

}