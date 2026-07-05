namespace Accounting.Contracts.Dtos;

/// <summary>Next GL entry number projection for UI forms.</summary>
public class EntryNumberDto
{
    public string EntryNumber { get; set; } = string.Empty;
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; } = string.Empty;
}
