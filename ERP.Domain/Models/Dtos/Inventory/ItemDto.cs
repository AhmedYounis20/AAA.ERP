using Shared.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ERP.Domain.Models.Dtos.Inventory;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }
    public string Code { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.Code)?.Code ?? string.Empty; set { } }
    public string? Gs1Code { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.GS1)?.Code ?? string.Empty; set { } }
    public string? EGSCode { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.EGS)?.Code ?? string.Empty; set { } }
    public ItemType? ItemType { get; set; }
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get; set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public List<Guid> SuppliersIds { get; set; } = [];
    public List<string> BarCodes { get => ItemCodes.Where(e => e.CodeType == ItemCodeType.BarCode).Select(e=>e.Code).ToList(); }
    [JsonIgnore] 
    public List<ItemCode> ItemCodes { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<ItemSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<ItemPackingUnitDto> PackingUnits { get; set; } = [];
    public List<ColorSizeCombinationDto> SubDomainCombinations { get; set; } = [];
    public bool ApplyDomainChanges { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }

}