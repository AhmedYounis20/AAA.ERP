using Inventory.Domain.Models.Entities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Inventory.Domain.Commands;

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