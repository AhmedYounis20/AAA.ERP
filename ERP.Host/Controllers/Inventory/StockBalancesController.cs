using Inventory.Application.Services;
using Inventory.Application.Commands.StockBalances;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Application.DTOs.Filters;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for stock balances and quantity queries.</summary>
[Route("api/[controller]")]
[ApiController]
public class StockBalancesController : BaseController<StockBalance, StockBalanceCreateCommand, StockBalanceUpdateCommand>
{
    private readonly IStockBalanceService _service;

    public StockBalancesController(
        IStockBalanceService service,
        ISender sender) : base(service, sender)
    {
        _service = service;
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] StockBalanceCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetAllForDisplay();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] StockBalanceFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetDisplayById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] StockBalanceUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Gets stock balance by variant and branch.</summary>
    [HttpGet("byVariantAndBranch")]
    public virtual async Task<IActionResult> GetByVariantAndBranch([FromQuery] Guid variantId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByVariantAndBranch(variantId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets stock balance by variant, packing unit, and branch.</summary>
    [HttpGet("byVariantPackingUnitAndBranch")]
    public virtual async Task<IActionResult> GetByVariantPackingUnitAndBranch([FromQuery] Guid variantId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByVariantPackingUnitAndBranch(variantId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists stock balances for a branch.</summary>
    [HttpGet("byBranch/{branchId}")]
    public virtual async Task<IActionResult> GetByBranch(Guid branchId)
    {
        var result = await _service.GetByBranch(branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists stock balances for a variant.</summary>
    [HttpGet("byVariant/{variantId}")]
    public virtual async Task<IActionResult> GetByVariant(Guid variantId)
    {
        var result = await _service.GetByVariant(variantId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets current quantity for variant, packing unit, and branch.</summary>
    [HttpGet("currentBalance")]
    public virtual async Task<IActionResult> GetCurrentBalance([FromQuery] Guid variantId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetCurrentBalance(variantId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates stock quantity and unit cost.</summary>
    [HttpPost("updateStockBalance")]
    public virtual async Task<IActionResult> UpdateStockBalance([FromBody] UpdateStockBalanceRequest request)
    {
        var result = await _service.UpdateStockBalance(request.VariantId, request.PackingUnitId, request.BranchId, request.Quantity, request.UnitCost, request.IsReceipt);
        return StatusCode((int)result.StatusCode, result);
    }
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
