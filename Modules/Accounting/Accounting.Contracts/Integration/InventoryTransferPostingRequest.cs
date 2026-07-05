namespace Accounting.Contracts.Integration;

public record InventoryTransferPostingRequest(
    Guid SourceDocumentId,
    string SourceDocumentType,
    Guid BranchId,
    Guid DebitAccountId,
    Guid CreditAccountId,
    DateTime EntryDate,
    Guid FinancialPeriodId,
    decimal TotalAmount);
