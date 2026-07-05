using ERP.BuildingBlocks.Application.DTOs.BaseDtos;
using Shared.BaseEntities;
using Shared.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class SubLeadgerLookupDto : LookupDto
{
    public Guid? ChartOfAccountId { get; set; }
    public string? Code { get; set; }
}