using ERP.BuildingBlocks.Application.Dtos.BaseDtos;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class ChartOfAccountLookupDto : LookupDto
{
    public string? Code { get; set; }
    public Guid? ParentId { get; set; }
    public Guid AccountGuidId { get; set; }
    public bool IsActiveAccount { get; set; }
    public bool IsPostedAccount { get; set; }
    public bool IsStopDealing { get; set; }
    public bool IsDepreciable { get; set; }
    public bool IsCreatedFromSubLeadger { get; set; }
    public bool IsSubLeadgerBaseAccount { get; set; }
    public AccountNature AccountNature { get; set; }
    public SubLeadgerType? SubLeadgerType { get; set; }
}