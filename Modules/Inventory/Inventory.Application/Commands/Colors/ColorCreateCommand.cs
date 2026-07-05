using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Commands.Colors;

public class ColorCreateCommand : BaseSettingCreateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
}
