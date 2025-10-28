using Inventory.Domain.Models.Entities.Sizes;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.Sizes;

public class SizeUpdateCommand : BaseSettingUpdateCommand<Size>
{
    public string Code { get; set; } = string.Empty;
}