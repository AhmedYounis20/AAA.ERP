using Account.Domain.Models.Entities.CostCenters;
using Shared.Domain.DTOs;

namespace Account.Domain.DTOs.Lookups;

public class CostCenterLookupDto : LookupDto
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
}