using Inventory.Application.Commands.Sizes;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.Sizes;

public sealed class SizeDeleteCommandHandler(ISizeRepository repository)
    : BaseSettingDeleteHandler<Size, SizeDeleteCommand>(repository);
