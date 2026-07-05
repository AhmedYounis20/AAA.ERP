using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.DTOs;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product attribute definitions.</summary>
[Route("api/[controller]")]
[ApiController]
public class AttributeDefinitionsController : BaseSettingController<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>
{
    private readonly IAttributeDefinitionService _attributeDefinitionService;

    public AttributeDefinitionsController(IAttributeDefinitionService attributeDefinitionService,
        ISender sender) : base(attributeDefinitionService, sender)
    {
        _attributeDefinitionService = attributeDefinitionService;
    }

    /// <summary>Lists all attribute definitions.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists attribute definitions with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Lists active attribute definitions for UI pickers.</summary>
    [HttpGet("lookups/active")]
    public virtual async Task<IActionResult> GetActive()
    {
        var result = await _attributeDefinitionService.GetActiveAttributeDefinitions();
        return Ok(new { success = true, data = result });
    }

    /// <summary>Gets an attribute definition by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Gets an attribute definition including its predefined values.</summary>
    [HttpGet("{id}/with-values")]
    public virtual async Task<IActionResult> GetWithPredefinedValues(Guid id)
    {
        var result = await _attributeDefinitionService.GetWithPredefinedValues(id);
        if (result == null)
            return NotFound(new { success = false, message = "Attribute definition not found" });
        return Ok(new { success = true, data = result });
    }

    /// <summary>Creates an attribute definition.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AttributeDefinitionCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Updates an attribute definition.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AttributeDefinitionUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes an attribute definition.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}
