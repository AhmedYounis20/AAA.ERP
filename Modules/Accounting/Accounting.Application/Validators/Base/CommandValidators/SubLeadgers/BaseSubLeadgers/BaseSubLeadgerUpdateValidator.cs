using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Validators.Account.CommandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerUpdateValidator<TCommand, TEntity> : BaseTreeSettingUpdateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    public BaseSubLeadgerUpdateValidator() : base()
    { }
}