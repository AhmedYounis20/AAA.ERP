using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.interfaces
{
    public interface IBaseSettingService<TEntity, in TCreateCommand,in TUpdateCommand> 
    : IBaseService<TEntity,TCreateCommand,TUpdateCommand>
    where TEntity : BaseSettingEntity
    where TCreateCommand : BaseSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseSettingUpdateCommand<TEntity>
    {
        public Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string name);
    }
}
