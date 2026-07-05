using ERP.BuildingBlocks.Application.Dtos.BaseDtos;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class SubLeadgerLookupDto : LookupDto
{
    public Guid? ChartOfAccountId { get; set; }
    public string? Code { get; set; }
}