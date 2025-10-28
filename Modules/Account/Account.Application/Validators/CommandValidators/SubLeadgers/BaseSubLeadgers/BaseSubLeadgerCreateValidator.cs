using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;
using Account.Domain.Models.Entities.SubLeadgers;
using FluentValidation;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerCreateValidator<TCommand, TEntity> : BaseTreeSettingCreateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerCreateCommand<TEntity>
{
    public BaseSubLeadgerCreateValidator() : base()
    {
        _ = RuleFor(e => e.NodeType).IsInEnum().WithMessage("PleaseChooseValidNodeType");
    }
}