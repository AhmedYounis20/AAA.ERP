using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.FinancialPeriods;

public class FinancialPeriod : BaseEntity
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}