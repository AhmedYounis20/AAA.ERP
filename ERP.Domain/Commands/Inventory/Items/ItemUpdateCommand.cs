using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Commands.Inventory.Items;

public class ItemUpdateCommand : BaseTreeSettingUpdateCommand<Item>
{
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
    public List<string> BarCodes { get; set; } = [];
    public List<Guid> ManufacturerCompaniesIds { get; set; } = [];
    public List<ItemSellingPriceDiscountDto> SellingPriceDiscounts { get; set; } = [];
    public List<ItemPackingUnitDto> PackingUnits { get; set; } = [];
}