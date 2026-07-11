using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities.StockBalances;

namespace Inventory.Application.Commands.StockBalances;

public class StockBalanceUpdateCommand : BaseUpdateCommand<StockBalance>
{
    public Guid VariantId { get; set; }
    public Guid PackingUnitId { get; set; }
    public Guid BranchId { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}
