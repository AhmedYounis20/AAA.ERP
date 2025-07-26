using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.Domain.Commands.Inventory;

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