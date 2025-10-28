using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.API.Controllers.Inventory;

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

    [HttpGet("byItemAndBranch")]
    public virtual async Task<IActionResult> GetByItemAndBranch([FromQuery] Guid itemId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByItemAndBranch(itemId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byItemPackingUnitAndBranch")]
    public virtual async Task<IActionResult> GetByItemPackingUnitAndBranch([FromQuery] Guid itemId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetByItemPackingUnitAndBranch(itemId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byBranch/{branchId}")]
    public virtual async Task<IActionResult> GetByBranch(Guid branchId)
    {
        var result = await _service.GetByBranch(branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("byItem/{itemId}")]
    public virtual async Task<IActionResult> GetByItem(Guid itemId)
    {
        var result = await _service.GetByItem(itemId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("currentBalance")]
    public virtual async Task<IActionResult> GetCurrentBalance([FromQuery] Guid itemId, [FromQuery] Guid packingUnitId, [FromQuery] Guid branchId)
    {
        var result = await _service.GetCurrentBalance(itemId, packingUnitId, branchId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost("updateStockBalance")]
    public virtual async Task<IActionResult> UpdateStockBalance([FromBody] UpdateStockBalanceRequest request)
    {
        var result = await _service.UpdateStockBalance(request.ItemId, request.PackingUnitId, request.BranchId, request.Quantity, request.UnitCost, request.IsReceipt);
        return StatusCode((int)result.StatusCode, result);
    }
}

public class UpdateStockBalanceRequest
{
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public Guid BranchId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsReceipt { get; set; }
} 