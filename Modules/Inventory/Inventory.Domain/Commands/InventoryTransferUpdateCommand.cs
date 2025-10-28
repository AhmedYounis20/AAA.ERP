using Inventory.Domain.Models.Entities;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands;

public class InventoryTransferItemUpdateCommand
{
    public Guid? Id { get; set; } // Null for new items
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public string? Notes { get; set; }
}

public class InventoryTransferUpdateCommand : BaseUpdateCommand<InventoryTransfer>
{
    public Guid SourceBranchId { get; set; }
    public Guid DestinationBranchId { get; set; }
    public InventoryTransferType TransferType { get; set; } = InventoryTransferType.Conditional;
    public List<InventoryTransferItemUpdateCommand> Items { get; set; } = new();
} 