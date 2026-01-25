using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantManufacturerCompany : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid ManufacturerCompanyId { get; set; }
    public ManufacturerCompany? ManufacturerCompany { get; set; }
}