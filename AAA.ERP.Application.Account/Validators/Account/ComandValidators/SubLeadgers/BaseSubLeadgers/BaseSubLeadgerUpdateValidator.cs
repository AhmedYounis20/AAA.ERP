using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

namespace ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerUpdateValidator<TCommand, TEntity> : BaseTreeSettingUpdateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    public BaseSubLeadgerUpdateValidator() : base()
    { }
}