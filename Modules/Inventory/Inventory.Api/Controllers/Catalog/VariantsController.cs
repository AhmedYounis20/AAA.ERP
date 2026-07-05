using Inventory.Application.Commands.Variants;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product variants (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VariantsController(ISender sender, IVariantReadService reads) : ControllerBase
{
    /// <summary>Lists all records.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    /// <summary>Lists variants for a product.</summary>
    [HttpGet("byProduct/{productId}")]
    public async Task<IActionResult> GetVariantsByProduct(Guid productId)
        => this.ToResult(await reads.GetVariantsByProductId(productId));

    /// <summary>Searches variants by term.</summary>
    [HttpGet("search-variants")]
    public async Task<IActionResult> SearchVariants([FromQuery] string searchTerm)
        => this.ToResult(await reads.SearchVariants(searchTerm));

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] VariantUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new VariantDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
