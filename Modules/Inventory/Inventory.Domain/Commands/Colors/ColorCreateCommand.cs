using Inventory.Domain.Models.Entities.Colors;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Inventory.Domain.Commands.Colors;

public class ColorCreateCommand : BaseSettingCreateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
}