using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class CostCenterLookupDto : LookupDto
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
}