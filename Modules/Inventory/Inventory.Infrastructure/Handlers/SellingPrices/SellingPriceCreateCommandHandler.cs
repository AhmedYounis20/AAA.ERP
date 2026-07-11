using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.SellingPrices;

public sealed class SellingPriceCreateCommandHandler(ISellingPriceRepository repository)
    : BaseSettingCreateHandler<SellingPrice, SellingPriceCreateCommand>(repository);
