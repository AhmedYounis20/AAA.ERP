namespace Accounting.Contracts.Dtos;

/// <summary>Next inventory transaction number projection for UI forms.</summary>
public class TransactionNumberDto
{
    public string TransactionNumber { get; set; } = string.Empty;
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
}
