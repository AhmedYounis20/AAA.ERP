using Account.Domain.Commands.Roles;
using Account.Domain.Models.Entities.Roles;
using FluentValidation;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Account.Application.Validators.CommandValidators.Roles;

public class RoleUpdateValidator : BaseSettingUpdateValidator<RoleUpdateCommand, Role>
{
    public RoleUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}