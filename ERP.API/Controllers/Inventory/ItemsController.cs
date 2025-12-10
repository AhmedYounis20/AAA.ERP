using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Dtos.Inventory;
using Shared.DTOs.Filters;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : BaseTreeSettingController<Item, ItemCreateCommand, ItemUpdateCommand>
{
    IItemService _service;
    public ItemsController(IItemService service,
        ISender mapper) : base(service, mapper)
    {
        _service = service;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ItemCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>
    /// Gets all items (no pagination - for backward compatibility)
    /// </summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetItemDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Gets paginated items with entity-specific filtering
    /// </summary>
    /// <param name="filter">Filter parameters including ItemType, NodeType, SupplierId, ManufacturerCompanyId, etc.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] ItemFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
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

    [HttpGet("{id}/packingUnits")]
    public async Task<IActionResult> GetPackingUnits(Guid id)
    {
        // Fetch packing units for the item
        var item = await _service.GetItemDtoById(id);
        if (item == null || item.Result == null)
            return NotFound();
        return Ok(item.Result.PackingUnits);
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
