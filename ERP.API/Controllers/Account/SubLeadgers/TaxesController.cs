using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Entities;
using ERP.API.Controllers.BaseControllers;
using Accounting.Domain.OutputDtos.Lookups;
using Shared.DTOs;
using Shared.DTOs.Filters;

namespace ERP.API.Controllers.Account.SubLeadgers;

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

    [HttpPost]
    public async Task<IActionResult> Create(TaxCreateCommand input)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TaxUpdateCommand input)
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
