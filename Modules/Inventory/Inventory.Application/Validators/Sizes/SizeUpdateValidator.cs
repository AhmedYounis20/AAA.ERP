using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.Sizes;

public class SizeUpdateValidator : BaseSettingUpdateValidator<SizeUpdateCommand, Size>
{
    public SizeUpdateValidator() : base()
    {
        RuleFor(e => e.Code).NotEmpty().WithMessage(ValidationMessages.CodeIsRequired);
    }
}
