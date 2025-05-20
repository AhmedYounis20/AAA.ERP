using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class Item : BaseTreeSettingEntity<Item>
{
    public string? Code { get; set; }
    public string? Gs1Code { get; set; }
    public string? EGSCode { get; set; }
    public ItemType? ItemType { get; set; }
    public decimal MaxDiscount { get; set; }
    public decimal ConditionalDiscount { get; set; }
    public decimal DefaultDiscount { get; set; }
    public DiscountType DefaultDiscountType { get; set; }
    public bool IsDiscountBasedOnSellingPrice { get;set; }
    public string? Model { get; set; }
    public string? Version { get; set; }
    public string? CountryOfOrigin { get; set; }
    public List<ItemBarcode> ItemBarcodes { get; set; } = [];
    public List<ItemSupplier> ItemSuppliers { get; set; } = [];
    public List<ItemManufacturerCompany> ItemManufacturerCompanies { get; set; } = [];
    public List<ItemSellingPriceDiscount> ItemSellingPriceDiscounts { get; set; } = [];
    public List<ItemPackingUnitPrice> ItemPackingUnitPrices { get; set; } = [];
}
