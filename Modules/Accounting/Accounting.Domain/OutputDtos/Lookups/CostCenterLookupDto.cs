using ERP.BuildingBlocks.Application.Dtos.BaseDtos;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class CostCenterLookupDto : LookupDto
{
    public int Percent { get; set; }

    public CostCenterType CostCenterType { get; set; }
}