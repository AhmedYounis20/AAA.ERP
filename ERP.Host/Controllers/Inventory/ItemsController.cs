using Inventory.Application.Commands.Items;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Dtos;
using ERP.BuildingBlocks.Application.DTOs.Filters;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for legacy item catalog tree.</summary>
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

    /// <summary>Creates a new record.</summary>
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

    /// <summary>Returns the next available code.</summary>
    [HttpGet("getNextCode")]
    public virtual async Task<IActionResult> GetNextCode([FromQuery] Guid? parentId)
    {
        var result = await _service.GeNextCode(parentId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetItemDtoById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists item variants.</summary>
    [HttpGet("variants")]
    public virtual async Task<IActionResult> GetVariants()
    {
        var result = await _service.GetVariants();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists packing units for an item.</summary>
    [HttpGet("{id}/packingUnits")]
    public async Task<IActionResult> GetPackingUnits(Guid id)
    {
        // Fetch packing units for the item
        var item = await _service.GetItemDtoById(id);
        if (item == null || item.Result == null)
            return NotFound();
        return Ok(item.Result.PackingUnits);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ItemUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
