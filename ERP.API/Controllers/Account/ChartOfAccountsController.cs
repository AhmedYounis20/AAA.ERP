using ERP.Domain.Commands.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using Shared.DTOs;

namespace ERP.API.Controllers.Account;

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
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ChartOfAccountCreateCommand input)
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
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ChartOfAccountUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

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

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
    [HttpGet("generateNewChildCode")]
    public async Task<IActionResult> GenerateNewChildCode([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GenerateNewCodeForChild(parentId));
    }

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.NextAccountDefaultData(parentId));
    }

}