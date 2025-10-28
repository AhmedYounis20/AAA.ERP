using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ImportTransactionsController : BaseController<InventoryTransaction, ImportTransactionCreateCommand, ImportTransactionUpdateCommand>
{
    private IImportTransactionService _service;

    public ImportTransactionsController(IImportTransactionService service,
        ISender sender) : base(service, sender)
        => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ImportTransactionCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, ImportTransactionUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    [HttpGet("GetTransactionNumber")]
    public async Task<IActionResult> GetTransactionNumber([FromQuery] DateTime dateTime)
    {
        var result = await _service.GetTransactionNumber(dateTime);
        return StatusCode((int)result.StatusCode, result);
    }
} 