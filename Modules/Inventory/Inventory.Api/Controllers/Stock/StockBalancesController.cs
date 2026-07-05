using Inventory.Application.Commands.StockBalances;
using Inventory.Application.Services;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Stock;

/// <summary>CRUD API for stock balances and quantity queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StockBalancesController(ISender sender, IStockBalanceQueries queries) : ControllerBase
{
    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StockBalanceCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] StockBalanceFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StockBalanceUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new StockBalanceDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    /// <summary>Gets stock balance by variant and branch.</summary>
    [HttpGet("byVariantAndBranch")]
    public async Task<IActionResult> GetByVariantAndBranch([FromQuery] Guid variantId, [FromQuery] Guid branchId)
        => this.ToResult(await queries.GetByVariantAndBranch(variantId, branchId));

    /// <summary>Gets stock balance by variant, packing unit, and branch.</summary>
    [HttpGet("byVariantPackingUnitAndBranch")]
    public async Task<IActionResult> GetByVariantPackingUnitAndBranch(
        [FromQuery] Guid variantId,
        [FromQuery] Guid packingUnitId,
        [FromQuery] Guid branchId)
        => this.ToResult(await queries.GetByVariantPackingUnitAndBranch(variantId, packingUnitId, branchId));

    /// <summary>Lists stock balances for a branch.</summary>
    [HttpGet("byBranch/{branchId}")]
    public async Task<IActionResult> GetByBranch(Guid branchId)
        => this.ToResult(await queries.GetByBranch(branchId));

    /// <summary>Lists stock balances for a variant.</summary>
    [HttpGet("byVariant/{variantId}")]
    public async Task<IActionResult> GetByVariant(Guid variantId)
        => this.ToResult(await queries.GetByVariant(variantId));

    /// <summary>Gets current quantity for variant, packing unit, and branch.</summary>
    [HttpGet("currentBalance")]
    public async Task<IActionResult> GetCurrentBalance(
        [FromQuery] Guid variantId,
        [FromQuery] Guid packingUnitId,
        [FromQuery] Guid branchId)
        => this.ToResult(await queries.GetCurrentBalance(variantId, packingUnitId, branchId));

    /// <summary>Updates stock quantity and unit cost.</summary>
    [HttpPost("updateStockBalance")]
    public async Task<IActionResult> UpdateStockBalance([FromBody] UpdateStockBalanceRequest request)
        => this.ToResult(await sender.Send(new UpdateStockBalanceCommand(
            request.VariantId,
            request.PackingUnitId,
            request.BranchId,
            request.Quantity,
            request.UnitCost,
            request.IsReceipt)));
}

/// <summary>API for UpdateStockBalanceRequest.</summary>
public class UpdateStockBalanceRequest
{
    public Guid VariantId { get; set; }
    public Guid PackingUnitId { get; set; }
    public Guid BranchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsReceipt { get; set; }
}
