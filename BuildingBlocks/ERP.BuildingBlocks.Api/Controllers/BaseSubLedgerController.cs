using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// CQRS base for sub-ledger tree entities: tree CRUD reads plus <c>NextAccountDefaultData</c>.
/// </summary>
public abstract class BaseSubLedgerController<TEntity, TCreate, TUpdate>(
    ISender sender,
    ISubLedgerQueries<TEntity, TCreate> queries)
    : BaseTreeSettingController<TEntity, TCreate, TUpdate>(sender, queries)
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreate : BaseTreeSettingCreateCommand<TEntity>
    where TUpdate : BaseTreeSettingUpdateCommand<TEntity>
{
    protected ISubLedgerQueries<TEntity, TCreate> SubLedgerQueries { get; } = queries;

    [HttpGet("NextAccountDefaultData")]
    public virtual async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
        => this.ToResult(await SubLedgerQueries.GetNextSubLedgers(parentId));
}
