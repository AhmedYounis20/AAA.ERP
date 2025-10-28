using Account.Domain.Commands.Currencies;
using Account.Domain.Models.Entities.Currencies;
using Shared.API.Controllers;

namespace Account.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>
{
    IBaseQueryService<Currency, CurrencyLookupDto> _baseQueryService;
    public CurrenciesController(ICurrencyService service, IBaseQueryService<Currency,CurrencyLookupDto>baseQueryService,
        ISender sender) : base(service, sender)
    => _baseQueryService = baseQueryService;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CurrencyCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<LookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<LookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true,
                ErrorMessages = new List<string>()
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
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CurrencyUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}