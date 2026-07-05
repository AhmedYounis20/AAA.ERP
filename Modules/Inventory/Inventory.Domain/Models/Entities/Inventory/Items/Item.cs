using Shared.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class Item : BaseTreeSettingEntity<Item>
{
    [NotMapped]
    public ItemCode? Code { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.Code); }
    [NotMapped]
    public ItemCode? Gs1Code { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.GS1); }
    [NotMapped]
    public ItemCode? EGSCode { get => ItemCodes.FirstOrDefault(e => e.CodeType == ItemCodeType.EGS); }
    [NotMapped]
    public List<ItemCode> BarCodes { get => ItemCodes.Where(e => e.CodeType == ItemCodeType.BarCode).ToList(); }
    
    // SubDomain Properties (only for NodeType.SubDomain)
    public bool ApplyDomainChanges { get; set; }  // Whether changes in parent domain apply to this subdomain
    public Guid? ColorId { get; set; } // For SubDomain items
    public Guid? SizeId { get; set; } // For SubDomain items    
    
    public ItemType? ItemType { get; set; }
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get;set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public List<ItemCode> ItemCodes { get; set; } = [];
    public List<ItemSupplier> ItemSuppliers { get; set; } = [];
    public List<ItemManufacturerCompany> ItemManufacturerCompanies { get; set; } = [];
    public List<ItemSellingPriceDiscount> ItemSellingPriceDiscounts { get; set; } = [];
    public List<ItemPackingUnit> ItemPackingUnitPrices { get; set; } = [];
}