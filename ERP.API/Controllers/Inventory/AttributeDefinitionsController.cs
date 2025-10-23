using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class AttributeDefinitionsController : BaseSettingController<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    private readonly IAttributeDefinitionService _attributeDefinitionService;

    public AttributeDefinitionsController(IAttributeDefinitionService attributeDefinitionService,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(attributeDefinitionService, localizer, sender)
    {
        _attributeDefinitionService = attributeDefinitionService;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("lookups/active")]
    public virtual async Task<IActionResult> GetActive()
    {
        var result = await _attributeDefinitionService.GetActiveAttributeDefinitions();
        return Ok(new { success = true, data = result });
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpGet("{id}/with-values")]
    public virtual async Task<IActionResult> GetWithPredefinedValues(Guid id)
    {
        var result = await _attributeDefinitionService.GetWithPredefinedValues(id);
        if (result == null)
            return NotFound(new { success = false, message = "Attribute definition not found" });
        return Ok(new { success = true, data = result });
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AttributeDefinitionCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AttributeDefinitionUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}