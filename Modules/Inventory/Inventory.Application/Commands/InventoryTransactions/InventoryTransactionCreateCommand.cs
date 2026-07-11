using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Application.Commands.InventoryTransactions;

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
    public Guid VariantId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalCost { get; set; }
}
