using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerCreateValidator<TCommand, TEntity> : BaseTreeSettingCreateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerCreateCommand<TEntity>
{
    public BaseSubLeadgerCreateValidator() : base()
    {
        _ = RuleFor(e => e.NodeType).IsInEnum().WithMessage(ErrorMessages.PleaseChooseValidNodeType);
    }
}