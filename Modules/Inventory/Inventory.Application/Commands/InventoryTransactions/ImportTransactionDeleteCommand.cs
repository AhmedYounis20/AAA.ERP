using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Shared;

namespace Inventory.Application.Commands.InventoryTransactions;

/// <summary>Deletes an import (receipt) inventory transaction.</summary>
public record ImportTransactionDeleteCommand(Guid Id) : ICommand<ApiResponse<InventoryTransaction>>;
