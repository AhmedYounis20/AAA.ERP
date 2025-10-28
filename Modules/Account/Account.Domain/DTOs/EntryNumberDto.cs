using AAA.ERP.OutputDtos.BaseDtos;

namespace Account.Domain.DTOs;

public class EntryNumberDto 
{
    public string EntryNumber { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; }
}