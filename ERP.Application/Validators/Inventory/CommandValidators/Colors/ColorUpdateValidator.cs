using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Colors;

public class ColorUpdateValidator : BaseSettingUpdateValidator<ColorUpdateCommand, Color>
{
    public ColorUpdateValidator(): base()
    {
        RuleFor(e => e.ColorCode).NotEmpty().WithMessage("ColorCodeIsRequired");
    }
}