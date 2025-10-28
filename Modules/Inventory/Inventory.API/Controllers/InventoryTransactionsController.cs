using Inventory.Application.Services;
using Inventory.Domain.Commands.InventoryTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Responses;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryTransactionsController : ControllerBase
{
    private readonly IInventoryTransactionService _inventoryTransactionService;
    private readonly IExportTransactionService _exportTransactionService;
    private readonly IImportTransactionService _importTransactionService;
    private readonly ISender _sender;

    public InventoryTransactionsController(
        IInventoryTransactionService inventoryTransactionService,
        IExportTransactionService exportTransactionService,
        IImportTransactionService importTransactionService,
        ISender sender)
    {
        _inventoryTransactionService = inventoryTransactionService;
        _exportTransactionService = exportTransactionService;
        _importTransactionService = importTransactionService;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryTransactionCreateCommand input)
    {
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _inventoryTransactionService.ReadAll();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _inventoryTransactionService.ReadById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] InventoryTransactionUpdateCommand input)
    {
        input.Id = id;
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _inventoryTransactionService.Delete(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("export")]
    public async Task<IActionResult> CreateExport([FromBody] ExportTransactionCreateCommand input)
    {
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> CreateImport([FromBody] ImportTransactionCreateCommand input)
    {
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }
}
