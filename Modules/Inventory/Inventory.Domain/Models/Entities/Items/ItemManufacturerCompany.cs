using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemManufacturerCompany : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid ManufacturerCompanyId { get; set; }
    public ManufacturerCompany? ManufacturerCompany { get; set; }
}
