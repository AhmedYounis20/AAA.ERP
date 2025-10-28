using FluentValidation;
using Inventory.Domain.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.Sizes;

public class SizeUpdateValidator : BaseSettingUpdateValidator<SizeUpdateCommand, Size>
{
    public SizeUpdateValidator() : base()
    {
        RuleFor(e => e.Code).NotEmpty().WithMessage("CodeIsRequired");
    }
}