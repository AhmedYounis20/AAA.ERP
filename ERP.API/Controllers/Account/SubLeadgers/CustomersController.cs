using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Customers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Domain.OutputDtos.Lookups;
using Shared.DTOs.Filters;

namespace ERP.API.Controllers.Account.SubLeadgers;

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

    [HttpPost]
    public async Task<IActionResult> Create(CustomerCreateCommand input)
    => await CreateRecord(input);


    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SubLeadgerFilterDto filter, CancellationToken cancellationToken)
    => await GetAllRecordsPaginated(filter, cancellationToken);

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);


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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CustomerUpdateCommand input)
    => await UpdateRecord(id, input);


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    => await DeleteRecord(id);

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GetNextSubLeadgers(parentId));
    }
}
