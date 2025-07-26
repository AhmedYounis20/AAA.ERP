using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.Domain.Commands.Inventory;

public class InventoryTransferItemCreateCommand
{
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public string? Notes { get; set; }
}

public class InventoryTransferCreateCommand : BaseCreateCommand<InventoryTransfer>
{
    public Guid SourceBranchId { get; set; }
    public Guid DestinationBranchId { get; set; }
    public InventoryTransferType TransferType { get; set; } = InventoryTransferType.Conditional;
    public List<InventoryTransferItemCreateCommand> Items { get; set; } = new();
} 