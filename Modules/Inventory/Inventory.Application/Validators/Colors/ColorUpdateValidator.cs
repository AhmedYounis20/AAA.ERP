using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Colors;

public class ColorUpdateValidator : BaseSettingUpdateValidator<ColorUpdateCommand, Color>
{
    public ColorUpdateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage(ValidationMessages.ColorValueIsRequired);
    }
}
