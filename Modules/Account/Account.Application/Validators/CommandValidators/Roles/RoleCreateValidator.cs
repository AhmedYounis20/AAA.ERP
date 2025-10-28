using System.Data;
using Account.Domain.Commands.Roles;
using Account.Domain.Models.Entities.Roles;
using FluentValidation;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Account.Application.Validators.CommandValidators.Roles;

public class RoleCreateValidator : BaseSettingCreateValidator<RoleCreateCommand, Role>
{
    public RoleCreateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}