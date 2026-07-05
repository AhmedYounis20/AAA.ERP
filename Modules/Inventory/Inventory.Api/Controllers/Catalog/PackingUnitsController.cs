using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;
using Inventory.Application.Dtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for packing unit settings and lookup queries.</summary>
[Route("api/[controller]")]
[ApiController]
public class PackingUnitsController : BaseSettingController<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>
{
    IBaseQueryService<PackingUnit, PackingUnitLookupDto> _baseQueryService;

    public PackingUnitsController(IPackingUnitService service,
        IBaseQueryService<PackingUnit, PackingUnitLookupDto> baseQueryService,
        ISender mapper) : base(service, mapper)
    {
        _baseQueryService = baseQueryService;
    }

    /// <summary>Creates a packing unit.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] PackingUnitCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all packing units.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists packing units with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a packing unit by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Returns cached packing-unit lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<PackingUnitLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<PackingUnitLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
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

    /// <summary>Updates a packing unit.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] PackingUnitUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a packing unit.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
