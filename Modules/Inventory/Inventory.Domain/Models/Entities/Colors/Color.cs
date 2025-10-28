using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Colors;

public class Color : BaseSettingEntity
{
    public string ColorValue { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
