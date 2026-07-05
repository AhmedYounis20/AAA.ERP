using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;
using System.Net;

namespace ERP.Infrastructure.Services.Queries;

/// <summary>
/// Base read queries for tree setting entities. Transitional: delegates to <see cref="IBaseTreeSettingService{TEntity,TCreateCommand,TUpdateCommand}"/>.
/// </summary>
public class BaseTreeSettingQueries<TEntity, TCreateCommand, TUpdateCommand>
    : BaseSettingQueries<TEntity, TCreateCommand, TUpdateCommand>,
      ITreeSettingQueries<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseTreeSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseTreeSettingUpdateCommand<TEntity>
{
    private readonly IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand> _treeService;

    public BaseTreeSettingQueries(IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand> service)
        : base(service)
        => _treeService = service;

    public async Task<ApiResponse<IEnumerable<TEntity>>> GetLevel(int level = 0)
    {
        var items = await _treeService.GetLevel(level);
        return new ApiResponse<IEnumerable<TEntity>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<TEntity>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await _treeService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<TEntity>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }
}
