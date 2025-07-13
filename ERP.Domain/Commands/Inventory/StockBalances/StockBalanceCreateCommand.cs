using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.Domain.Commands.Inventory.StockBalances;

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