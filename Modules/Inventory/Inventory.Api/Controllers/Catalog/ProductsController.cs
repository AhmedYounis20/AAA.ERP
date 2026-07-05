using Inventory.Application.Commands.Products;
using Inventory.Application.Services;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product catalog tree (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController(ISender sender, IProductQueries queries) : ControllerBase
{
    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    /// <summary>Returns the next available code.</summary>
    [HttpGet("getNextCode")]
    public async Task<IActionResult> GetNextCode([FromQuery] Guid? parentId)
        => this.ToResult(await queries.GetNextCode(parentId));

    /// <summary>Gets a record by identifier.</summary>
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
        => this.ToResult(await queries.SearchByName(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => this.ToResult(await sender.Send(new ProductDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    /// <summary>Gets all nodes at the given tree depth (root level when level is 0).</summary>
    [HttpGet("GetLevel")]
    public async Task<IActionResult> GetLevel([FromQuery] int level = 0)
        => this.ToResult(await queries.GetLevel(level));

    /// <summary>Gets child nodes under parentId down to the requested depth.</summary>
    [HttpGet("GetChildren/{parentId}")]
    public async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
        => this.ToResult(await queries.GetChildren(parentId, level));
}
