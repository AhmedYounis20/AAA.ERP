using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Application.Commands.Colors;

public class ColorCreateCommand : BaseSettingCreateCommand<Color>
{
    public string ColorValue { get; set; } = string.Empty;
}
