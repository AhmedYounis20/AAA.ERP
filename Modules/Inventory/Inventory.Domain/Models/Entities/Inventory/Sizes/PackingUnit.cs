using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Sizes;

public class Size : BaseSettingEntity
{
    public string Code { get; set; } = string.Empty;
}