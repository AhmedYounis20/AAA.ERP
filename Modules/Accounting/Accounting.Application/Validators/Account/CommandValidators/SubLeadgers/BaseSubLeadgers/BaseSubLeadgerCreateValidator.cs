using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.Account.CommandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerCreateValidator<TCommand, TEntity> : BaseTreeSettingCreateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerCreateCommand<TEntity>
{
    public BaseSubLeadgerCreateValidator() : base()
    {
        _ = RuleFor(e => e.NodeType).IsInEnum().WithMessage(ValidationMessages.PleaseChooseValidNodeType);
    }
}