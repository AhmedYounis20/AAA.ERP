using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Domain.Commands.Inventory.InventoryTransactions;

public class InventoryTransactionCreateCommand : BaseCreateCommand<InventoryTransaction>
{
    public InventoryTransactionType TransactionType { get; set; } = InventoryTransactionType.Receipt;
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public List<InventoryTransactionItemCreateDto> Items { get; set; } = [];
}

public class InventoryTransactionItemCreateDto
{
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalCost { get; set; }
} 