using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Commands.Inventory.Items;

public class ItemCreateCommand : BaseTreeSettingCreateCommand<Item>, IItemCommand
{
    public string Code { get; set; } = string.Empty;
    public string? Gs1Code { get; set; }
    public string? EGSCode { get; set; }
    public bool ApplyDomainChanges { get; set; } = true;
    public Guid? ColorId { get; set; }
    public Guid? SizeId { get; set; }
    public List<ColorSizeCombinationDto> SubDomainCombinations { get; set; } = [];
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