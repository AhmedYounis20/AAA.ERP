using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Domain.OutputDtos.Lookups;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class
    FinancialPeriodsController : BaseController<FinancialPeriod, FinancialPeriodCreateCommand,
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

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] FinancialPeriodCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetDto(id);
        return StatusCode((int)result.StatusCode, result);   
    }

    [HttpGet("nextDefaultdata")]
    public virtual async Task<IActionResult> GetNextDefaultdata()
    {
        var result = await _service.GetNextDefaultdata();
        return StatusCode((int)result.StatusCode, result);   
    }

    [HttpGet("GetCurrentFinancialPeriod")]  
    public virtual async Task<IActionResult> GetCurrentFinancialPeriod()
    {
        var result = await _service.GetCurrentFinancailPeriod();
        return StatusCode((int)result.StatusCode, result);
    }

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

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] FinancialPeriodUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}