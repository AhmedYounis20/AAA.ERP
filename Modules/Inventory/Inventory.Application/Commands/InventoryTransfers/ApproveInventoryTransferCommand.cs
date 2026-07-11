using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities;
using Shared;

namespace Inventory.Application.Commands.InventoryTransfers;

/// <summary>Approves a pending staged inventory transfer.</summary>
public record ApproveInventoryTransferCommand(Guid Id) : ICommand<ApiResponse<InventoryTransfer>>;
