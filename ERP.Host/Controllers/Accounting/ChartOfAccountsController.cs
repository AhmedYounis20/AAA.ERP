using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using Accounting.Application.Dtos.Filters;

namespace ERP.Host.Controllers.Accounting;

/// <summary>CRUD API for chart-of-account tree and code generation.</summary>
[Route("api/[controller]")]
[ApiController]
public class ChartOfAccountsController : BaseTreeSettingController<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>
{
    IChartOfAccountService _service;
    IBaseQueryService<ChartOfAccount, ChartOfAccountLookupDto> _baseQueryService;
    public ChartOfAccountsController(IChartOfAccountService service,
    IBaseQueryService<ChartOfAccount, ChartOfAccountLookupDto> baseQueryService,

        ISender sender) : base(service, sender)
    {
        _service = service;
        _baseQueryService = baseQueryService;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ChartOfAccountCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>
    /// Gets all chart of accounts (no pagination - for backward compatibility)
    /// </summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>
    /// Gets paginated chart of accounts with entity-specific filtering
    /// </summary>
    /// <param name="filter">Filter parameters including AccountNature, IsActiveAccount, IsPostedAccount, etc.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] ChartOfAccountFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ChartOfAccountUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Returns lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<ChartOfAccountLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<ChartOfAccountLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<ChartOfAccountLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Generates a new child account code.</summary>
    [HttpGet("generateNewChildCode")]
    public async Task<IActionResult> GenerateNewChildCode([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GenerateNewCodeForChild(parentId));
    }

    /// <summary>Returns default data for the next account under a parent.</summary>
    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.NextAccountDefaultData(parentId));
    }
}
