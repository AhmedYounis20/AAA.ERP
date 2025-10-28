using Inventory.Domain.Models.Entities.StockBalances;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Inventory.Domain.Commands.StockBalances;

public class StockBalanceCreateCommand : BaseCreateCommand<StockBalance>
{
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public Guid BranchId { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
} 