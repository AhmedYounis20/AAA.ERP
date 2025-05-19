using AAA.ERP.OutputDtos.BaseDtos;

namespace AAA.ERP.OutputDtos;

public class EntryNumberDto 
{
    public string EntryNumber { get; set; }
    public Guid FinancialPeriodId { get; set; }
    public string FinancialPeriodNumber { get; set; }
}