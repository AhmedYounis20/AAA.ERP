using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Commands.StockBalances;

public class StockBalanceCreateCommand : BaseCreateCommand<StockBalance>
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
