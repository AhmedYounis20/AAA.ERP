using ERP.Application.Services.Account;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Domain.OutputDtos.Lookups;
using Shared.DTOs;

namespace ERP.API.Controllers.Inventory;

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

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] PackingUnitCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

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

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] PackingUnitUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
