using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerUpdateValidator<TCommand, TEntity> : BaseTreeSettingUpdateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    public BaseSubLeadgerUpdateValidator() : base()
    { }
}