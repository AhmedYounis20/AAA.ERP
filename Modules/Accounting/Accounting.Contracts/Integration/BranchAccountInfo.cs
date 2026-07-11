namespace Accounting.Contracts.Integration;

public record BranchAccountInfo(
    Guid BranchId,
    Guid? ChartOfAccountId);
