using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Shared.BaseEntities;
using Shared.Responses;

namespace Accounting.Application.Services.SubLeadgers;

public interface IBaseSubLeadgerService<TEntity, TCreateCommand, in TUpdateCommand>
    : IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}