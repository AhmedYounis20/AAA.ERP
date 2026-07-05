using Shared.BaseEntities;
using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class ItemLookupDto : LookupDto
{
    public string? Code { get; set; }
    public NodeType NodeType { get; set; }
    public Guid? ParentId { get; set; }
    public List<ItemPackingUnitLookupDto> PackingUnits { get; set; } = [];
}

public class ItemPackingUnitLookupDto
{
    public Guid PackingUnitId { get; set; }
    public string? PackingUnitName { get; set; }
    public decimal PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
}

