using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;
using Shared.Responses;

namespace ERP.Application.Validators.Account.ComandValidators.Roles;

public class RoleUpdateValidator : BaseSettingUpdateValidator<RoleUpdateCommand, Role>
{
    public RoleUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}