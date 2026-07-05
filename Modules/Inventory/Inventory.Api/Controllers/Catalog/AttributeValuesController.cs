using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for predefined attribute values (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttributeValuesController(ISender sender, IAttributeValueQueries queries) : ControllerBase
{
    /// <summary>Creates an attribute value.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttributeValueCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    /// <summary>Lists all attribute values.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    /// <summary>Lists attribute values with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    /// <summary>Gets an attribute value by identifier.</summary>
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
        => this.ToResult(await queries.SearchByName(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    /// <summary>Lists values belonging to the given attribute definition.</summary>
    [HttpGet("byAttribute/{attributeId}")]
    public async Task<IActionResult> GetByAttribute(Guid attributeId)
        => this.ToResult(await queries.GetAttributeValuesByAttributeDefenitionId(attributeId));

    /// <summary>Lists active attribute values.</summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAttributeValues()
        => this.ToResult(await queries.GetActiveAttributeValues());

    /// <summary>Updates an attribute value.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AttributeValueUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes an attribute value.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => this.ToResult(await sender.Send(new AttributeValueDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
