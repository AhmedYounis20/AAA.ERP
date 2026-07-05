using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Sizes;

public class Size : BaseSettingEntity
{
    public string Code { get; set; } = string.Empty;
}