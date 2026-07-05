using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Validators.Sizes;

public class SizeUpdateValidator : BaseSettingUpdateValidator<SizeUpdateCommand, Size>
{
    public SizeUpdateValidator() : base()
    {
        RuleFor(e => e.Code).NotEmpty().WithMessage("CodeIsRequired");
    }
}
