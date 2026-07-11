using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Dtos.Catalog;

/// <summary>Inventory transfer read model with branch display names.</summary>
public class InventoryTransferReadDto
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid SourceBranchId { get; set; }
    public string? SourceBranchName { get; set; }
    public Guid DestinationBranchId { get; set; }
    public string? DestinationBranchName { get; set; }
    public InventoryTransferStatus Status { get; set; }
    public InventoryTransferType TransferType { get; set; }
    public string? Notes { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<InventoryTransferItem> Items { get; set; } = [];
}
