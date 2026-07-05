using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Colors;

public class ColorCreateValidator : BaseSettingCreateValidator<ColorCreateCommand, Color>
{
    public ColorCreateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage(ValidationMessages.ColorValueIsRequired);
    }
}
