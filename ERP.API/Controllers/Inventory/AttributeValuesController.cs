using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.API.Controllers.Inventory;

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

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AttributeValueCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpGet("byAttribute/{attributeId}")]
    public virtual async Task<IActionResult> GetByAttribute(Guid attributeId)
    {
        var result = await _service.GetAttributeValuesByAttributeDefenitionId(attributeId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("active")]
    public virtual async Task<IActionResult> GetActiveAttributeValues()
    {
        var result = await _service.GetActiveAttributeValues();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AttributeValueUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}



