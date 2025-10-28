using AAA.ERP.OutputDtos.BaseDtos;

namespace Account.Domain.DTOs.Lookups;

public class SubLeadgerLookupDto : LookupDto 
{
    public Guid? ChartOfAccountId { get; set; }
    public string? Code { get; set; }  
}