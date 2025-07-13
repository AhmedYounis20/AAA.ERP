using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace ERP.Domain.Commands.Inventory.InventoryTransactions;

public class ImportTransactionCreateCommand : BaseCreateCommand<InventoryTransaction>
{
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public string? Notes { get; set; }
    public List<InventoryTransactionItemCreateDto> Items { get; set; } = [];
}

public class ImportTransactionUpdateCommand : BaseUpdateCommand<InventoryTransaction>
{
    public DateTime TransactionDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Guid TransactionPartyId { get; set; }
    public Guid BranchId { get; set; }
    public string? Notes { get; set; }
    public List<InventoryTransactionItemCreateDto> Items { get; set; } = [];
} 