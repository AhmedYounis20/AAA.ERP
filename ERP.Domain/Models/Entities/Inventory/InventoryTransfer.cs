using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory;

public enum InventoryTransferStatus
{
    Pending,
    Approved,
    Rejected
}

public enum InventoryTransferType
{
    Conditional = 0,
    Direct = 1
}

public class InventoryTransfer : BaseEntity
{
    public Guid SourceBranchId { get; set; }
    public Branch? SourceBranch { get; set; }
    public Guid DestinationBranchId { get; set; }
    public Branch? DestinationBranch { get; set; }
    public InventoryTransferStatus Status { get; set; } = InventoryTransferStatus.Pending;
    public InventoryTransferType TransferType { get; set; } = InventoryTransferType.Conditional;
    public string? Notes { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<InventoryTransferItem> Items { get; set; } = new();
} 