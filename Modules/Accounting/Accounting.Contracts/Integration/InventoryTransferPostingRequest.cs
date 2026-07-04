namespace Accounting.Contracts.Integration;

public record InventoryTransferPostingRequest(
    Guid BranchId,
    Guid DebitAccountId,
    Guid CreditAccountId,
    DateTime EntryDate,
    Guid FinancialPeriodId,
    decimal TotalAmount);
