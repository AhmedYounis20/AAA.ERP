using Inventory.Application.Commands.Sizes;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;
using Mapster;

namespace Inventory.Infrastructure.Handlers.Sizes;

public sealed class SizeCreateCommandHandler(ISizeRepository repository)
    : BaseSettingCreateHandler<Size, SizeCreateCommand>(repository)
{
    private ISizeRepository SizeRepository => (ISizeRepository)SettingRepository;

    protected override async Task<Size> MapCreateEntityAsync(
        SizeCreateCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = command.Adapt<Size>();
        var maxCode = await SizeRepository.GetMaxCodeAsync();
        var next = 1;
        if (int.TryParse(maxCode, out var max))
            next = max + 1;
        entity.Code = next.ToString();
        return entity;
    }
}
