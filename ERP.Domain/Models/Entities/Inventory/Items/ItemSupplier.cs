using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemSupplier : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}