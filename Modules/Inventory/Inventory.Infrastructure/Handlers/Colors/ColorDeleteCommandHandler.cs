using Inventory.Application.Commands.Colors;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.Colors;

public sealed class ColorDeleteCommandHandler(IColorRepository repository)
    : BaseSettingDeleteHandler<Color, ColorDeleteCommand>(repository);
