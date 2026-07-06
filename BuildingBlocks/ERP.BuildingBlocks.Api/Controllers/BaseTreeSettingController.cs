using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// CQRS base for tree setting entities: standard CRUD reads plus <c>GetLevel</c> and <c>GetChildren</c>.
/// </summary>
public abstract class BaseTreeSettingController<TEntity, TCreate, TUpdate>(
    ISender sender,
    ITreeSettingQueries<TEntity> queries)
    : BaseSettingController<TEntity, TCreate, TUpdate>(sender, queries)
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreate : BaseTreeSettingCreateCommand<TEntity>
    where TUpdate : BaseTreeSettingUpdateCommand<TEntity>
{
    protected new ITreeSettingQueries<TEntity> Queries { get; } = queries;

    [HttpGet("GetLevel")]
    public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
        => this.ToResult(await Queries.GetLevel(level));

    [HttpGet("GetChildren/{parentId}")]
    public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
        => this.ToResult(await Queries.GetChildren(parentId, level));
}
