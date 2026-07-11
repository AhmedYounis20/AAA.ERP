using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product attribute definitions (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttributeDefinitionsController(ISender sender, IAttributeDefinitionQueries queries)
    : BaseSettingController<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<AttributeDefinition>> BuildDeleteCommand(Guid id)
        => new AttributeDefinitionDeleteCommand(id);

    [HttpGet("lookups/active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await queries.GetActiveAttributeDefinitions();
        return Ok(new { success = true, data = result });
    }

    [HttpGet("{id}/with-values")]
    public async Task<IActionResult> GetWithPredefinedValues(Guid id)
    {
        var result = await queries.GetWithPredefinedValues(id);
        if (result == null)
            return NotFound(new { success = false, message = "Attribute definition not found" });
        return Ok(new { success = true, data = result });
    }
}
