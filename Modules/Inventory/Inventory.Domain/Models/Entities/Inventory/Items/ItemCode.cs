using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public ItemCodeType CodeType { get; set; }
}