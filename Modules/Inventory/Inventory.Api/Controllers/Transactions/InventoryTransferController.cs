using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Services.Transactions;
using Inventory.Domain.Models.Entities;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Transactions;

/// <summary>API for inter-branch inventory transfers (hybrid CQRS).</summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InventoryTransferController(ISender sender, IInventoryTransferQueries queries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryTransferCreateCommand command)
        => this.ToResult(await sender.Send(command), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] InventoryTransferUpdateCommand command)
        => this.ToResult(await sender.Send(command), successMessageKey: SuccessMessages.UpdatedSuccessfully);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWithDetails(Guid id)
        => this.ToResult(await queries.GetWithDetails(id));

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] InventoryTransferFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(InventoryTransferStatus status)
        => this.ToResult(await queries.GetByStatus(status));

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve(Guid id)
        => this.ToResult(await sender.Send(new ApproveInventoryTransferCommand(id)));

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> Reject(Guid id, [FromQuery] Guid approverId, [FromQuery] string? reason)
        => this.ToResult(await sender.Send(new RejectInventoryTransferCommand(id, approverId, reason)));
}
