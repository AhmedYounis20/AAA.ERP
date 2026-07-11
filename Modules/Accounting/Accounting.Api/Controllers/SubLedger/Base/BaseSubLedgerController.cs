using Accounting.Application.Services.SubLedger;
using Accounting.Application.Services.SubLedger.Base;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;

namespace Accounting.Api.Controllers.SubLedger.Base;

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
