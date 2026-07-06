using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

namespace Inventory.Infrastructure.Handlers.StockBalances;

public sealed class StockBalanceUpdateCommandHandler(IStockBalanceRepository repository)
    : BaseUpdateHandler<StockBalance, StockBalanceUpdateCommand>(repository);
