using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities;
using Shared;

namespace Inventory.Application.Commands.InventoryTransfers;

/// <summary>Rejects a pending staged inventory transfer.</summary>
public record RejectInventoryTransferCommand(Guid Id, Guid ApproverId, string? Reason)
    : ICommand<ApiResponse<InventoryTransfer>>;
