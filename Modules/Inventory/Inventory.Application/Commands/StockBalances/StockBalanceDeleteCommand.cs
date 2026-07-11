using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.StockBalances;
using Shared;

namespace Inventory.Application.Commands.StockBalances;

/// <summary>Deletes a stock balance record.</summary>
public record StockBalanceDeleteCommand(Guid Id) : ICommand<ApiResponse<StockBalance>>, IEntityDeleteCommand;
