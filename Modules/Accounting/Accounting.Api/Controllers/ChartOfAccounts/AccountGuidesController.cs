using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Api.Controllers.ChartOfAccounts;

/// <summary>CRUD API for account guide settings and lookups.</summary>
[Route("api/[controller]")]
[ApiController]
public class AccountGuidesController : BaseSettingController<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>
{
    IBaseQueryService<AccountGuide, LookupDto> _baseQueryService;
    public AccountGuidesController(IAccountGuideService service, IBaseQueryService<AccountGuide, LookupDto> baseQueryService,
        ISender mapper) : base(service, mapper)
    => _baseQueryService = baseQueryService;

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AccountGuideCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>
    /// Gets all account guides (no pagination - for backward compatibility)
    /// </summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>
    /// Gets paginated account guides with filtering support
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AccountGuideUpdateCommand input)
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
