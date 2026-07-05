using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.BuildingBlocks.Application.Services.BaseServices
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
