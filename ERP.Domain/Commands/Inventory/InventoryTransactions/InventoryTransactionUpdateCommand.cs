using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Domain.Commands.Inventory.InventoryTransactions;

public class InventoryTransactionUpdateCommand : BaseUpdateCommand<InventoryTransaction>
{
    public InventoryTransactionType TransactionType { get; set; } = InventoryTransactionType.Receipt;
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public List<InventoryTransactionItemUpdateDto> Items { get; set; } = [];
}

public class InventoryTransactionItemUpdateDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalCost { get; set; }
} 