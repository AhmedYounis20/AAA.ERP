using Accounting.Application.Commands.FixedAssets;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for fixed-asset sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FixedAssetsController(
    ISender sender,
    IFixedAssetQueries queries,
    IBaseQueryService<FixedAsset, SubLeadgerLookupDto> lookups)
    : BaseTreeSettingController<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<FixedAsset>> BuildDeleteCommand(Guid id)
        => new FixedAssetDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData(
        [FromQuery] Guid? parentId,
        [FromQuery] FixedAssetType fixedAssetType = FixedAssetType.Lands)
        => this.ToResult(await queries.GetNextFixedAsset(parentId, fixedAssetType));
}
