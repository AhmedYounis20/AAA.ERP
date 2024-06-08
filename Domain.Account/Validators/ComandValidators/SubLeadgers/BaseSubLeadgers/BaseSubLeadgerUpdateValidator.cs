using Domain.Account.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

namespace Domain.Account.Validators.ComandValidators.ChartOfAccounts;

public class BaseSubLeadgerUpdateValidator<TCommand,TEntity> : BaseTreeSettingUpdateValidator<TCommand, TEntity>
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    public BaseSubLeadgerUpdateValidator() : base()
    {}
}