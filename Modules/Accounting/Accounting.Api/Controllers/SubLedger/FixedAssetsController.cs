using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Application.Services.Queries;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for fixed-asset sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FixedAssetsController(
    ISender sender,
    IFixedAssetReadService reads,
    IBaseQueryService<FixedAsset, SubLeadgerLookupDto> lookups) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FixedAssetCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get() => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SubLeadgerFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => this.ToResult(await reads.GetById(id));

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookUps()
    {
        try
        {
            return this.ToResult(new ApiResponse<IEnumerable<SubLeadgerLookupDto>>
            {
                IsSuccess = true,
                Result = await lookups.GetLookUps(e => e.NodeType == NodeType.Domain)
            });
        }
        catch
        {
            return this.ToResult(new ApiResponse<IEnumerable<SubLeadgerLookupDto>> { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, FixedAssetUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => this.ToResult(await sender.Send(new FixedAssetDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId, [FromQuery] FixedAssetType fixedAssetType)
        => this.ToResult(await reads.GetNextFixedAsset(parentId, fixedAssetType));

    [HttpGet("GetLevel")]
    public async Task<IActionResult> GetLevel([FromQuery] int level = 0) => this.ToResult(await reads.GetLevel(level));

    [HttpGet("GetChildren/{parentId}")]
    public async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
        => this.ToResult(await reads.GetChildren(parentId, level));
}
