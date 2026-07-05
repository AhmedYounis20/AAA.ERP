using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using Accounting.Domain.OutputDtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;

namespace ERP.Host.Controllers.Accounting;

/// <summary>CRUD API for financial periods and current-period queries.</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinancialPeriodsController : BaseController<FinancialPeriod, FinancialPeriodCreateCommand,
    FinancialPeriodUpdateCommand>
{
    IFinancialPeriodService _service;
    IBaseQueryService<FinancialPeriod, FinancialPeriodLookupDto> _baseQueryService;

    public FinancialPeriodsController(IFinancialPeriodService service,
        IBaseQueryService<FinancialPeriod, FinancialPeriodLookupDto> baseQueryService,
        ISender sender)
        : base(service, sender)
    {
        _service = service;
        _baseQueryService = baseQueryService;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] FinancialPeriodCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] FinancialPeriodFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetDto(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Returns default data for the next financial period.</summary>
    [HttpGet("nextDefaultdata")]
    public virtual async Task<IActionResult> GetNextDefaultdata()
    {
        var result = await _service.GetNextDefaultdata();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Returns the current active financial period.</summary>
    [HttpGet("GetCurrentFinancialPeriod")]
    public virtual async Task<IActionResult> GetCurrentFinancialPeriod()
    {
        var result = await _service.GetCurrentFinancailPeriod();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Returns lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<FinancialPeriodLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<FinancialPeriodLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<FinancialPeriodLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] FinancialPeriodUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
