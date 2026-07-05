using Accounting.Application.Commands.Customers;
using Accounting.Application.Services;
using ERP.Host.Controllers.BaseControllers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.OutputDtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs.Filters;

namespace ERP.Host.Controllers.Accounting.SubLeadgers;

/// <summary>CRUD API for customer sub-ledger accounts.</summary>
[Route("api/[controller]")]
[ApiController]
public class CustomersController : BaseTreeSettingController<Customer, CustomerCreateCommand, CustomerUpdateCommand>
{
    private ICustomerService _service;
    private ISender _sender;
    IBaseQueryService<Customer, SubLeadgerLookupDto> _baseQueryService;

    public CustomersController(ICustomerService service, IBaseQueryService<Customer, SubLeadgerLookupDto> baseQueryService, ISender sender)
        : base(service, sender)
    {
        _service = service;
        _sender = sender;
        _baseQueryService = baseQueryService;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CustomerCreateCommand input)
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
                IsSuccess = true,
                ErrorMessages = new List<string>()
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
    public async Task<IActionResult> Update(Guid id, CustomerUpdateCommand input)
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
