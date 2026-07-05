using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for packing unit settings and lookup queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PackingUnitsController(
    ISender sender,
    IPackingUnitReadService reads,
    IBaseQueryService<PackingUnit, PackingUnitLookupDto> baseQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PackingUnitCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<PackingUnitLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<PackingUnitLookupDto>>
            {
                Result = await baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<PackingUnitLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PackingUnitUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new PackingUnitDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
