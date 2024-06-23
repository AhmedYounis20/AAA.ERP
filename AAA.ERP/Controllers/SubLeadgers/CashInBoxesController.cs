using AAA.ERP.Controllers.BaseControllers;
using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers.SubLeadgers;

[Route("api/[controller]")]
[ApiController]
public class CashInBoxesController : BaseTreeSettingController<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>
{
    private ICashInBoxService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;

    public CashInBoxesController(IStringLocalizer<Resource> localizer, ICashInBoxService service, ISender sender)
        : base(service, localizer, sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CashInBoxCreateCommand input)
    => await CreateRecord(input);

    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CashInBoxUpdateCommand input)
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