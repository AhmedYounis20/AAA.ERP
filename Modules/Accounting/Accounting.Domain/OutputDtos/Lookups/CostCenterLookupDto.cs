using AAA.ERP.OutputDtos.BaseDtos;
using ERP.Domain.Models.Entities.Account.CostCenters;
using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class CostCenterLookupDto : LookupDto
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
}