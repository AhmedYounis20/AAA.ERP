using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Entities;
using Accounting.Application.Dtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs;
using Accounting.Application.Dtos.Filters;

namespace ERP.Host.Controllers.Accounting.SubLeadgers;

/// <summary>CRUD API for tax sub-ledger accounts.</summary>
[Route("api/[controller]")]
[ApiController]
public class TaxesController : BaseTreeSettingController<Tax, TaxCreateCommand, TaxUpdateCommand>
{
    private readonly ITaxService _service;
    private readonly ISender _sender;
    private readonly IBaseQueryService<Tax, SubLeadgerLookupDto> _baseQueryService;

    public TaxesController(ITaxService service, IBaseQueryService<Tax, SubLeadgerLookupDto> baseQueryService, ISender sender)
        : base(service, sender)
    {
        _service = service;
        _sender = sender;
        _baseQueryService = baseQueryService;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(TaxCreateCommand input)
    => await CreateRecord(input);

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SubLeadgerFilterDto filter, CancellationToken cancellationToken)
    => await GetAllRecordsPaginated(filter, cancellationToken);

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);

    /// <summary>Returns lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<SubLeadgerLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<SubLeadgerLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(e => e.NodeType == NodeType.Domain),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<SubLeadgerLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TaxUpdateCommand input)
    => await UpdateRecord(id, input);

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    => await DeleteRecord(id);

    /// <summary>Returns default data for the next account under a parent.</summary>
    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GetNextSubLeadgers(parentId));
    }
}
