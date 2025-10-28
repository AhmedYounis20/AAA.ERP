using Inventory.Domain.Models.Entities.Colors;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.Colors;

public class ColorUpdateCommand : BaseSettingUpdateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}