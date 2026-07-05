using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Shared;

namespace Inventory.Application.Commands.InventoryTransactions;

/// <summary>Deletes an export (issue) inventory transaction.</summary>
public record ExportTransactionDeleteCommand(Guid Id) : ICommand<ApiResponse<InventoryTransaction>>;
