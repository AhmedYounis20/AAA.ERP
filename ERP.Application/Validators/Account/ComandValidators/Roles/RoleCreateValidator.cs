using System.Data;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;
using FluentValidation;
using Shared.Responses;

namespace ERP.Application.Validators.Account.ComandValidators.Roles;

public class RoleCreateValidator : BaseSettingCreateValidator<RoleCreateCommand, Role>
{
    public RoleCreateValidator() : base()
    {
        _ = RuleFor(e => e.Commission).GreaterThanOrEqualTo(0);
    }
}