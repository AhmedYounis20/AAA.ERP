using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;
using Shared.BaseEntities;

namespace Domain.Account.Validators.ComandValidators.ChartOfAccounts;

public class BaseSubLeadgerCreateValidator<TCommand, TEntity> : BaseTreeSettingCreateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerCreateCommand<TEntity>
{
    public BaseSubLeadgerCreateValidator() : base()
    {
        _ = RuleFor(e => e.NodeType).IsInEnum().WithMessage("PleaseChooseValidNodeType");
    }
}