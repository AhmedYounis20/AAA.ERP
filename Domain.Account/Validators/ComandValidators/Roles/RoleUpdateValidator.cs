using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.AccountGuides;

public class RoleUpdateValidator : BaseSettingUpdateValidator<RoleUpdateCommand, Role>
{
    public RoleUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}