using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Models.Entities.SubLeadgers;
using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Account.SubLeadgers;

namespace ERP.API.Controllers.SubLeadgers;

[Route("api/[controller]")]
[ApiController]
public class BanksController : BaseTreeSettingController<Bank,BankCreateCommand, BankUpdateCommand>
{
    private IBankService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;

    public BanksController(IStringLocalizer<Resource> localizer, IBankService service, ISender sender)
        : base(service, localizer, sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(BankCreateCommand input)
    => await CreateRecord(input);

    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, BankUpdateCommand input)
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