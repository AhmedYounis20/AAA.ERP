using Inventory.Application.Services;
using Inventory.Application.Commands.Products;
using Inventory.Domain.Models.Entities.Products;

namespace ERP.Host.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseTreeSettingController<Product, ProductCreateCommand, ProductUpdateCommand>
{
    private readonly IProductService _service;
    
    public ProductsController(IProductService service,
        ISender sender) : base(service, sender)
    {
        _service = service;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ProductCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetProductDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("getNextCode")]
    public virtual async Task<IActionResult> GetNextCode([FromQuery] Guid? parentId)
    {
        var result = await _service.GetNextCode(parentId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetProductDtoById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateCommand input)
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



