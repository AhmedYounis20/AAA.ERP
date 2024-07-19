using System.Data;
using Domain.Account.Commands.Roles;
using Domain.Account.Models.Entities.Roles;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;
using Shared.Responses;

namespace Domain.Account.Validators.ComandValidators.AccountGuides;

public class RoleCreateValidator : BaseSettingCreateValidator<RoleCreateCommand, Role>
{
    public RoleCreateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}