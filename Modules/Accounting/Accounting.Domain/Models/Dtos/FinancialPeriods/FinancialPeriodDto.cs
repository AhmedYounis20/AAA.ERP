using ERP.Domain.Models.Entities.Account.FinancialPeriods;
namespace ERP.Domain.Models.Dtos.FinancialPeriods;

public class FinancialPeriodDto
{
    public Guid Id { get; set; }
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public bool IsEditable { get; set; }
    public bool IsDeletable { get; set; }
    public bool IsNameEditable { get; set; }
}