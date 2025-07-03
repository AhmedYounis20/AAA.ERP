using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Colors;

public class ColorCreateValidator : BaseSettingCreateValidator<ColorCreateCommand, Color>
{
    public ColorCreateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage("ColorValueIsRequired");
    }
}