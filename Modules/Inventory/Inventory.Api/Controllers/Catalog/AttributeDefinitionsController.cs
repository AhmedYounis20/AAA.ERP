using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product attribute definitions (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttributeDefinitionsController(ISender sender, IAttributeDefinitionReadService reads) : ControllerBase
{
    /// <summary>Lists all attribute definitions.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    /// <summary>Lists attribute definitions with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    /// <summary>Lists active attribute definitions for UI pickers.</summary>
    [HttpGet("lookups/active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await reads.GetActiveAttributeDefinitions();
        return Ok(new { success = true, data = result });
    }

    /// <summary>Gets an attribute definition by identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    /// <summary>Gets an attribute definition including its predefined values.</summary>
    [HttpGet("{id}/with-values")]
    public async Task<IActionResult> GetWithPredefinedValues(Guid id)
    {
        var result = await reads.GetWithPredefinedValues(id);
        if (result == null)
            return NotFound(new { success = false, message = "Attribute definition not found" });
        return Ok(new { success = true, data = result });
    }

    /// <summary>Creates an attribute definition.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttributeDefinitionCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    /// <summary>Updates an attribute definition.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AttributeDefinitionUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes an attribute definition.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => this.ToResult(await sender.Send(new AttributeDefinitionDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
