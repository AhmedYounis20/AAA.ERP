using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace ERP.Domain.Commands.Inventory.Colors;

public class ColorUpdateCommand : BaseSettingUpdateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}