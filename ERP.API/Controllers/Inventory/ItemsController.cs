using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : BaseTreeSettingController<Item, ItemCreateCommand, ItemUpdateCommand>
{
    IItemService _service;
    public ItemsController(IItemService service,
        IStringLocalizer<Resource> localizer,
        ISender mapper) : base(service, localizer, mapper)
    {
        _service = service;

    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ItemCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetItemDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("getNextCode")]
    public virtual async Task<IActionResult> GetNextCode([FromQuery]Guid? parentId)
    {
        var result = await _service.GeNextCode(parentId);
        return StatusCode((int)result.StatusCode,result);
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetItemDtoById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("variants")]
    public virtual async Task<IActionResult> GetVariants()
    {
        var result = await _service.GetVariants();
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ItemUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

}