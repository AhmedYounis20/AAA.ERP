using Shared.BaseEntities;
using System.Text.Json.Serialization;
using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace ERP.Domain.Models.Dtos.Inventory;

public class VariantDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? NameSecondLanguage { get; set; }
    public string? Description { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Code { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.Code)?.Code ?? string.Empty; set { } }
    public string? Gs1Code { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.GS1)?.Code ?? string.Empty; set { } }
    public string? EGSCode { get => VariantCodes.FirstOrDefault(e => e.CodeType == VariantCodeType.EGS)?.Code ?? string.Empty; set { } }
    public List<string> BarCodes { get => VariantCodes.Where(e => e.CodeType == VariantCodeType.BarCode).Select(e => e.Code).ToList(); }
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Guid> SuppliersIds { get; set; } = [];
    public List<Guid> SalesTaxIds { get; set; } = [];
    public List<Guid> PurchaseTaxIds { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<VariantSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<VariantPackingUnitDto> PackingUnits { get; set; } = [];
    public List<VariantAttachmentDto> VariantAttachments { get; set; } = [];
    public AttachmentDto? VariantPicture { get; set; }
    public ProductType? ProductType { get; set; }

    // public List<VariantStockBalanceDto> StockBalances { get; set; } = [];
    public List<VariantAttributeValueDto> VariantAttributeValues { get; set; } = [];

    [JsonIgnore]
    public List<VariantCode> VariantCodes { get; set; } = [];

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}
