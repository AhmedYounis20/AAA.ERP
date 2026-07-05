using Inventory.Application.Services;
using Inventory.Application.Commands.StockBalances;
using Inventory.Domain.Models.Entities.Sizes;
using Shared.DTOs.Filters;

namespace ERP.Host.Controllers.Inventory;

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

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] StockBalanceCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] StockBalanceFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] StockBalanceUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return await DeleteRecord(id);
    }

    [HttpGet("byVariantAndBranch")]
    public virtual async Task<IActionResult> GetByVariantAndBranch([FromQuery] Guid variantId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByVariantAndBranch(variantId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byVariantPackingUnitAndBranch")]
    public virtual async Task<IActionResult> GetByVariantPackingUnitAndBranch([FromQuery] Guid variantId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByVariantPackingUnitAndBranch(variantId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byBranch/{branchId}")]
    public virtual async Task<IActionResult> GetByBranch(Guid branchId)
    {
        var result = await _service.GetByBranch(branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byVariant/{variantId}")]
    public virtual async Task<IActionResult> GetByVariant(Guid variantId)
    {
        var result = await _service.GetByVariant(variantId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("currentBalance")]
    public virtual async Task<IActionResult> GetCurrentBalance([FromQuery] Guid variantId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetCurrentBalance(variantId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("updateStockBalance")]
    public virtual async Task<IActionResult> UpdateStockBalance([FromBody] UpdateStockBalanceRequest request)
    {
        var result = await _service.UpdateStockBalance(request.VariantId, request.PackingUnitId, request.BranchId, request.Quantity, request.UnitCost, request.IsReceipt);
        return StatusCode((int)result.StatusCode, result);
    }
}

public class UpdateStockBalanceRequest
{
    public Guid VariantId { get; set; }
    public Guid PackingUnitId { get; set; }
    public Guid BranchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsReceipt { get; set; }
}
