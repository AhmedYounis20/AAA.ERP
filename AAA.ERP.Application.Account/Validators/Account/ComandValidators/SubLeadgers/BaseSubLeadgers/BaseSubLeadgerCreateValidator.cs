using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;
using Shared.BaseEntities;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerCreateValidator<TCommand, TEntity> : BaseTreeSettingCreateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerCreateCommand<TEntity>
{
    public BaseSubLeadgerCreateValidator() : base()
    {
        _ = RuleFor(e => e.NodeType).IsInEnum().WithMessage("PleaseChooseValidNodeType");
    }
}