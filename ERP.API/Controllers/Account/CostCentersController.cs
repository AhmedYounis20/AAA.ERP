using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.CostCenters;
using Shared.DTOs;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class CostCentersController : BaseTreeSettingController<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>
{
    IBaseQueryService<CostCenter, CostCenterLookupDto> _baseQueryService;
    public CostCentersController(ICostCenterService service,
            IBaseQueryService<CostCenter, CostCenterLookupDto> baseQueryService,

    IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    => _baseQueryService = baseQueryService;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CostCenterCreateCommand input)
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
        var result = new ApiResponse<IEnumerable<CostCenterLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<CostCenterLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(e => e.NodeType == NodeType.Domain),
                IsSuccess = true,
                ErrorMessages = new List<string>()
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<CostCenterLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CostCenterUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}