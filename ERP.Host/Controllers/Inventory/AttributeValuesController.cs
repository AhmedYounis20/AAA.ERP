using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for predefined attribute values.</summary>
[Route("api/[controller]")]
[ApiController]
public class AttributeValuesController : BaseSettingController<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>
{
    private readonly IAttributeValueService _service;

    public AttributeValuesController(IAttributeValueService service,
        ISender mapper) : base(service, mapper)
    {
        _service = service;
    }

    /// <summary>Creates an attribute value.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AttributeValueCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all attribute values.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists attribute values with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets an attribute value by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Lists values belonging to the given attribute definition.</summary>
    [HttpGet("byAttribute/{attributeId}")]
    public virtual async Task<IActionResult> GetByAttribute(Guid attributeId)
    {
        var result = await _service.GetAttributeValuesByAttributeDefenitionId(attributeId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists active attribute values.</summary>
    [HttpGet("active")]
    public virtual async Task<IActionResult> GetActiveAttributeValues()
    {
        var result = await _service.GetActiveAttributeValues();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an attribute value.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AttributeValueUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes an attribute value.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}
