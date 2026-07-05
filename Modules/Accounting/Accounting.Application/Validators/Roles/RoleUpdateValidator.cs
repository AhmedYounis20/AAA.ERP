using Accounting.Application.Commands.Roles;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Roles;
using FluentValidation;

namespace Accounting.Application.Validators.Roles;

public class RoleUpdateValidator : BaseSettingUpdateValidator<RoleUpdateCommand, Role>
{
    public RoleUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}
