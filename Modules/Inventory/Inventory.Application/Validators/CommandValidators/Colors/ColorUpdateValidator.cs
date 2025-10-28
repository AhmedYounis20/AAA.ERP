using FluentValidation;
using Inventory.Domain.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.Colors;

public class ColorUpdateValidator : BaseSettingUpdateValidator<ColorUpdateCommand, Color>
{
    public ColorUpdateValidator(): base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage("ColorValueIsRequired");
    }
}