using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemBarcode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public Guid ItemId { get; set; }
}