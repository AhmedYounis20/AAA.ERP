using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Commands.InventoryTransfers;

public class InventoryTransferItemUpdateCommand
{
    public Guid? Id { get; set; }
    public Guid VariantId { get; set; }
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
