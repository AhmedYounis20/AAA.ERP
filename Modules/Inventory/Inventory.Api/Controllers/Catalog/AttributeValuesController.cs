using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for predefined attribute values (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttributeValuesController(ISender sender, IAttributeValueQueries queries)
    : HybridSettingController<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<AttributeValue>> BuildDeleteCommand(Guid id)
        => new AttributeValueDeleteCommand(id);

    [HttpGet("byAttribute/{attributeId}")]
    public async Task<IActionResult> GetByAttribute(Guid attributeId)
        => this.ToResult(await queries.GetAttributeValuesByAttributeDefenitionId(attributeId));

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAttributeValues()
        => this.ToResult(await queries.GetActiveAttributeValues());
}
