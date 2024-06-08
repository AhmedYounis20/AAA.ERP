using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;

namespace Domain.Account.Services.BaseServices.interfaces;

public interface IBaseTreeSettingService<TEntity,in TCreateCommand, in TUpdateCommand> 
    : IBaseSettingService<TEntity,TCreateCommand,TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseTreeSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseTreeSettingUpdateCommand<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
}