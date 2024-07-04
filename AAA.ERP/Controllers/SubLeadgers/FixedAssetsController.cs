using AAA.ERP.Controllers.BaseControllers;
using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers.SubLeadgers;

[Route("api/[controller]")]
[ApiController]
public class FixedAssetsController : BaseTreeSettingController<FixedAsset,FixedAssetCreateCommand, FixedAssetUpdateCommand>
{
    private IFixedAssetService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;

    public FixedAssetsController(IStringLocalizer<Resource> localizer, IFixedAssetService service, ISender sender)
        : base(service, localizer, sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(FixedAssetCreateCommand input)
    => await CreateRecord(input);

    [HttpGet]
    public async Task<IActionResult> Get()
    => await GetAllRecords();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    => await GetRecord(id);

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, FixedAssetUpdateCommand input)
    => await UpdateRecord(id, input);


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    => await DeleteRecord(id);

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId,[FromQuery]FixedAssetType fixedAssetType)
    {
        return Ok(await _service.GetNextFixedAsset(parentId,fixedAssetType));
    }
}