using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory;
using ERP.Domain.Models.Entities.Inventory;

namespace ERP.API.Controllers.Inventory;

[ApiController]
[Route("api/[controller]")]
public class InventoryTransferController : ControllerBase
{
    private readonly IInventoryTransferService _service;
    public InventoryTransferController(IInventoryTransferService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryTransferCreateCommand command)
    {
        var result = await _service.Create(command);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] InventoryTransferUpdateCommand command)
    {
        var result = await _service.Update(command);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWithDetails(Guid id)
    {
        var result = await _service.GetWithDetails(id);
        if (!result.IsSuccess) return NotFound(result);
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.ReadAll();
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(InventoryTransferStatus status)
    {
        var result = await _service.GetByStatus(status);
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _service.ApproveTransfer(id);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int) result.StatusCode,result);
    }

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> Reject(Guid id, [FromQuery] Guid approverId, [FromQuery] string? reason)
    {
        var result = await _service.RejectTransfer(id, approverId, reason);
        if (!result.IsSuccess) return BadRequest(result);
        return StatusCode((int) result.StatusCode,result);
    }
} 