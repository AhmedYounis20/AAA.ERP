using Inventory.Application.Services;
using Inventory.Application.Commands.Variants;
using Inventory.Domain.Models.Entities.Variants;
using Shared.DTOs;

namespace ERP.API.Controllers.Inventory;

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

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetVariantDtos();
        return StatusCode((int)result.StatusCode, result);
    }
    
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _service.ReadAllDtosPaginated(filter, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetVariantDtoById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byProduct/{productId}")]
    public virtual async Task<IActionResult> GetVariantsByProduct(Guid productId)
    {
        var result = await _service.GetVariantsByProductId(productId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("search-variants")]
    public virtual async Task<IActionResult> SearchVariants([FromQuery] string searchTerm)
    {
        var result = await _service.SearchVariants(searchTerm);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] VariantUpdateCommand input)
    {
        input.Id = id;
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }
}
