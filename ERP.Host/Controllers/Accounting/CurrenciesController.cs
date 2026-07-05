using Accounting.Application.Commands.Currencies;
using Accounting.Application.Services;
using Accounting.Application.Validators.Currencies;
using Accounting.Domain.Models.Entities.Currencies;
using Accounting.Application.Dtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.Host.Controllers.Accounting;

/// <summary>CRUD API for currency settings and lookups.</summary>
[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>
{
    IBaseQueryService<Currency, CurrencyLookupDto> _baseQueryService;
    public CurrenciesController(ICurrencyService service, IBaseQueryService<Currency, CurrencyLookupDto> baseQueryService,
        CurrencyInputValidator validator,
        ISender sender) : base(service, sender)
    => _baseQueryService = baseQueryService;

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CurrencyCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>
    /// Gets all currencies (no pagination - for backward compatibility)
    /// </summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>
    /// Gets paginated currencies with filtering support
    /// </summary>
    /// <param name="filter">Filter parameters including PageNumber, PageSize, SearchTerm, etc.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Returns lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<LookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<LookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<LookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CurrencyUpdateCommand input)
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
