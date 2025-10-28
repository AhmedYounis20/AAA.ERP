using Inventory.Domain.Models.Entities.InventoryTransactions;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.InventoryTransactions;

public class ExportTransactionCreateCommand : BaseCreateCommand<InventoryTransaction>
{
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public List<InventoryTransactionItemCreateDto> Items { get; set; } = [];
}

public class ExportTransactionUpdateCommand : BaseUpdateCommand<InventoryTransaction>
{
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public List<InventoryTransactionItemCreateDto> Items { get; set; } = [];
} 