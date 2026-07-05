namespace Accounting.Contracts.Integration;

public record InventoryTransactionPostingRequest(
    Guid SourceDocumentId,
    Guid BranchId,
    Guid TransactionPartyId,
    DateTime TransactionDate,
    Guid FinancialPeriodId,
    decimal TotalAmount,
    InventoryGlDirection Direction);
