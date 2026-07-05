using Accounting.Application.Commands.Roles;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Models.Entities.Account.Roles;
using FluentValidation;

namespace Accounting.Application.Validators.Roles;

public class RoleCreateValidator : BaseSettingCreateValidator<RoleCreateCommand, Role>
{
    public RoleCreateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}
