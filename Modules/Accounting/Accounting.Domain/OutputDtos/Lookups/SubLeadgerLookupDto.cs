using AAA.ERP.OutputDtos.BaseDtos;
using Shared.BaseEntities;
using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class SubLeadgerLookupDto : LookupDto 
{
    public Guid? ChartOfAccountId { get; set; }
    public string? Code { get; set; }  
}