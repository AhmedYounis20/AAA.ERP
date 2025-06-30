using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace ERP.Domain.Commands.Inventory.Colors;

public class ColorUpdateCommand : BaseSettingUpdateCommand<Color>
{
    public string ColorCode { get; set; } = string.Empty;
}