using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using FluentValidation;

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