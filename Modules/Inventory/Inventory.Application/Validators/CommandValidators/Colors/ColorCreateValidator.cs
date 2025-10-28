using FluentValidation;
using Inventory.Domain.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.Colors;

public class ColorCreateValidator : BaseSettingCreateValidator<ColorCreateCommand, Color>
{
    public ColorCreateValidator() : base()
    {
        RuleFor(e => e.ColorValue).NotEmpty().WithMessage("ColorValueIsRequired");
    }
}