using ERP.BuildingBlocks.Application.DTOs.BaseDtos;
using Accounting.Domain.Models.Entities.CostCenters;
using Shared.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class CostCenterLookupDto : LookupDto
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
}