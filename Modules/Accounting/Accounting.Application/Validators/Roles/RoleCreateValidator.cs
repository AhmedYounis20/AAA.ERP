using Accounting.Application.Commands.Roles;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.Roles;
using FluentValidation;

namespace Accounting.Application.Validators.Roles;

public class RoleCreateValidator : BaseSettingCreateValidator<RoleCreateCommand, Role>
{
    public RoleCreateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}
