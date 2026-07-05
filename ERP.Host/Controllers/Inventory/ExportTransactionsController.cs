using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Accounting.Application.Dtos.Filters;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for stock export transactions.</summary>
[Route("api/[controller]")]
[ApiController]
public class ExportTransactionsController : BaseController<InventoryTransaction, ExportTransactionCreateCommand, ExportTransactionUpdateCommand>
{
    private IExportTransactionService _service;

    public ExportTransactionsController(IExportTransactionService service,
        ISender sender) : base(service, sender)
        => _service = service;

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExportTransactionCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] InventoryTransactionFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, ExportTransactionUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Returns the next transaction number for a given date.</summary>
    [HttpGet("GetTransactionNumber")]
    public async Task<IActionResult> GetTransactionNumber([FromQuery] DateTime dateTime)
    {
        var result = await _service.GetTransactionNumber(dateTime);
        return StatusCode((int)result.StatusCode, result);
    }
}
