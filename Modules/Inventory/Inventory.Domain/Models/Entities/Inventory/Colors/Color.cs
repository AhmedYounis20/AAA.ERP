using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Colors;

public class Color : BaseSettingEntity
{
    public string ColorValue { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}