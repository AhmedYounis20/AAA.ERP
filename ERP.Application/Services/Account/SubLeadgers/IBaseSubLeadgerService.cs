using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.SubLeadgers.BaseSubLeadgersCommands;
using Shared.BaseEntities;
using Shared.Responses;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface IBaseSubLeadgerService<TEntity, TCreateCommand, in TUpdateCommand>
    : IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}