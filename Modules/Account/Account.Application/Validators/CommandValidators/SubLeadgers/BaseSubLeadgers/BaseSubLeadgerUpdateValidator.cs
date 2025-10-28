using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Validators.CommandValidators.SubLeadgers.BaseSubLeadgers;

public class BaseSubLeadgerUpdateValidator<TCommand, TEntity> : BaseTreeSettingUpdateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    public BaseSubLeadgerUpdateValidator() : base()
    { }
}