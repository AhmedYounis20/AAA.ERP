using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Application.Validators.Colors;

public class ColorCreateValidator : BaseSettingCreateValidator<ColorCreateCommand, Color>
{
    public ColorCreateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage("ColorValueIsRequired");
    }
}
