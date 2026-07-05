using Shared.BaseEntities;

namespace Inventory.Domain.Models.Dtos;

public class VariantPackingUnitSellingPriceDto
{
    public Guid Id { get; set; }
    public Guid VariantPackingUnitId { get; set; }
    public Guid SellingPriceId { get; set; }
    public string SellingPriceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}











