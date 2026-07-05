using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Application.Validators.Colors;

public class ColorUpdateValidator : BaseSettingUpdateValidator<ColorUpdateCommand, Color>
{
    public ColorUpdateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage("ColorValueIsRequired");
    }
}
