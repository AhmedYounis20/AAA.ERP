using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Colors;

public class Color : BaseSettingEntity
{
    public string ColorCode { get; set; } = string.Empty;
}