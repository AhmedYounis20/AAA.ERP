using Accounting.Application.Commands.Roles;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Models.Entities.Account.Roles;
using FluentValidation;

namespace Accounting.Application.Validators.Roles;

public class RoleUpdateValidator : BaseSettingUpdateValidator<RoleUpdateCommand, Role>
{
    public RoleUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}
