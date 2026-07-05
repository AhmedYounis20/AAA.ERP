using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantManufacturerCompany : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid ManufacturerCompanyId { get; set; }
    public ManufacturerCompany? ManufacturerCompany { get; set; }
}