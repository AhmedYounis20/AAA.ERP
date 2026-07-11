using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.StockBalances;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public sealed class StockBalanceDeleteCommandHandler(IStockBalanceRepository repository)
    : BaseDeleteHandler<StockBalance, StockBalanceDeleteCommand>(repository);
