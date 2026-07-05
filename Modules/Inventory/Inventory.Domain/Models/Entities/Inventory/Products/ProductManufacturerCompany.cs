using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductManufacturerCompany : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid ManufacturerCompanyId { get; set; }
    public ManufacturerCompany? ManufacturerCompany { get; set; }
}