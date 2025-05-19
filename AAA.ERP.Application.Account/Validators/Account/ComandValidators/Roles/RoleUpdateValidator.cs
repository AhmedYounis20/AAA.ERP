using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;
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