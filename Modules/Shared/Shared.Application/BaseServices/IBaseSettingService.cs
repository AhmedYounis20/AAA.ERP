using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Domain.Responses;

namespace Shared.Application.BaseServices
{
    public interface IBaseSettingService<TEntity, in TCreateCommand, in TUpdateCommand>
    : IBaseService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseSettingEntity
    where TCreateCommand : BaseSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseSettingUpdateCommand<TEntity>
    {
        public Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string name);
    }
}
