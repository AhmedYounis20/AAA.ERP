using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Commands.Account.SubLeadgers.Branches;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.API.Controllers.Account.SubLeadgers;

[Route("api/[controller]")]
[ApiController]
public class BranchesController : BaseTreeSettingController<Branch, BranchCreateCommand, BranchUpdateCommand>
{
    private IBranchService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;

    public BranchesController(IStringLocalizer<Resource> localizer, IBranchService service, ISender sender)
        : base(service, localizer, sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(BranchCreateCommand input)
    => await CreateRecord(input);

    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, BranchUpdateCommand input)
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