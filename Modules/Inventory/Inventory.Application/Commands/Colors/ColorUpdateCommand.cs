using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Commands.Colors;

public class ColorUpdateCommand : BaseSettingUpdateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
