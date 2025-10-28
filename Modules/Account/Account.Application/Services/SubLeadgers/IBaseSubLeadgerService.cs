using Account.Domain.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Shared.Application.BaseServices;
using Shared.Domain.BaseEntities;
using Shared.Domain.Responses;

namespace Account.Application.Services.SubLeadgers;

public interface IBaseSubLeadgerService<TEntity, TCreateCommand, in TUpdateCommand>
    : IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseSubLeadgerCreateCommand<TEntity>
    where TUpdateCommand : BaseSubLeadgerUpdateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}