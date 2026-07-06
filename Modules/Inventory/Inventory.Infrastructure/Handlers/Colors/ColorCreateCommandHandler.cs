using Inventory.Application.Commands.Colors;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;
using Mapster;

namespace Inventory.Infrastructure.Handlers.Colors;

public sealed class ColorCreateCommandHandler(IColorRepository repository)
    : BaseSettingCreateHandler<Color, ColorCreateCommand>(repository)
{
    private IColorRepository ColorRepository => (IColorRepository)SettingRepository;

    protected override async Task<CrudValidationResult> ValidateCreateAsync(
        ColorCreateCommand command,
        CancellationToken cancellationToken)
    {
        var baseResult = await base.ValidateCreateAsync(command, cancellationToken);
        var errors = baseResult.Errors.ToList();

        if (await ColorRepository.GetByColorValueExists(command.ColorValue))
            errors.Add(ErrorMessages.ColorValueIsDuplicate);

        return errors.Count == 0
            ? CrudValidationResult.Success
            : CrudValidationResult.Failure(errors);
    }

    protected override async Task<Color> MapCreateEntityAsync(
        ColorCreateCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = command.Adapt<Color>();
        var maxCode = await ColorRepository.GetMaxCodeAsync();
        var next = 1;
        if (int.TryParse(maxCode, out var max))
            next = max + 1;
        entity.Code = next.ToString();
        return entity;
    }
}
