using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Models.DTOS;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.InventoryTransactions;

public class InventoryTransactionUpdateCommand : BaseUpdateCommand<InventoryTransaction>
{
    public InventoryTransactionType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionNumber { get; set; } = string.Empty;
    public Guid TransactionPartyId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public bool IsPosted { get; set; }
    public List<InventoryTransactionItemUpdateDto> Items { get; set; } = new();
}