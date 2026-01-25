using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductManufacturerCompany : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid ManufacturerCompanyId { get; set; }
    public ManufacturerCompany? ManufacturerCompany { get; set; }
}