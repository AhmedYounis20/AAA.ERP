using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services.SubLeadgers;

/// <summary>
/// Application service contract for base sub leadger.
/// </summary>
public interface IBaseSubLeadgerService<TEntity, TCreateCommand, in TUpdateCommand>
    : IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
