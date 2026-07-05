using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;
using Shared.DTOs.Filters;

namespace ERP.Host.Controllers.Inventory;

/// <summary>API for inter-branch inventory transfers.</summary>
[ApiController]
[Route("api/[controller]")]
public class InventoryTransferController : ControllerBase
{
    private readonly IInventoryTransferService _service;
    public InventoryTransferController(IInventoryTransferService service)
    {
        _service = service;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryTransferCreateCommand command)
    {
        var result = await _service.Create(command);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] InventoryTransferUpdateCommand command)
    {
        var result = await _service.Update(command);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets a record with full detail graph.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWithDetails(Guid id)
    {
        var result = await _service.GetWithDetails(id);
        if (!result.IsSuccess) return NotFound(result);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.ReadAll();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] InventoryTransferFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _service.ReadAllPaginated(filter, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records filtered by status.</summary>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(InventoryTransferStatus status)
    {
        var result = await _service.GetByStatus(status);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Approves a pending transfer.</summary>
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _service.ApproveTransfer(id);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Rejects a pending transfer.</summary>
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> Reject(Guid id, [FromQuery] Guid approverId, [FromQuery] string? reason)
    {
        var result = await _service.RejectTransfer(id, approverId, reason);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int)result.StatusCode, result);
    }
}
