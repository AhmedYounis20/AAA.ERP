using Inventory.Application.Services;
using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for product variants.</summary>
[Route("api/[controller]")]
[ApiController]
public class VariantsController : BaseController<Variant, VariantCreateCommand, VariantUpdateCommand>
{
    private readonly IVariantService _service;

    public VariantsController(IVariantService service,
        ISender sender) : base(service, sender)
    {
        _service = service;
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetVariantDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _service.ReadAllDtosPaginated(filter, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetVariantDtoById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists variants for a product.</summary>
    [HttpGet("byProduct/{productId}")]
    public virtual async Task<IActionResult> GetVariantsByProduct(Guid productId)
    {
        var result = await _service.GetVariantsByProductId(productId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Searches variants by term.</summary>
    [HttpGet("search-variants")]
    public virtual async Task<IActionResult> SearchVariants([FromQuery] string searchTerm)
    {
        var result = await _service.SearchVariants(searchTerm);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] VariantUpdateCommand input)
    {
        input.Id = id;
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}
