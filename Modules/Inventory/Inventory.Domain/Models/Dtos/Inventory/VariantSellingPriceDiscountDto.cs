using Shared.BaseEntities;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Models.Dtos.Inventory;

public class VariantSellingPriceDiscountDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid SellingPriceId { get; set; }
    public string SellingPriceName { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}
