using Inventory.Application.Commands.Products;
using Inventory.Application.Services;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product catalog tree (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController(ISender sender, IProductQueries queries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("getNextCode")]
    public async Task<IActionResult> GetNextCode([FromQuery] Guid? parentId)
        => this.ToResult(await queries.GetNextCode(parentId));

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
        => this.ToResult(await queries.SearchByName(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => this.ToResult(await sender.Send(new ProductDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("GetLevel")]
    public async Task<IActionResult> GetLevel([FromQuery] int level = 0)
        => this.ToResult(await queries.GetLevel(level));

    [HttpGet("GetChildren/{parentId}")]
    public async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
        => this.ToResult(await queries.GetChildren(parentId, level));
}
